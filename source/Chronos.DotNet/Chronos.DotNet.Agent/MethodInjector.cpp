#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"
#include <corhlpr.h>

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			MethodInjector::MethodInjector(ICorProfilerInfo2* corProfilerInfo2)
			{
				_corProfilerInfo2 = corProfilerInfo2;
			}

			MethodInjector::~MethodInjector()
			{
			}

			HRESULT MethodInjector::Initialize(ModuleID moduleId, std::wstring pinvokeModuleName, std::wstring injectedClassName, std::wstring prologMethodName, std::wstring epilogMethodName)
			{
				_moduleId = moduleId;
				HRESULT result;
				result = _corProfilerInfo2->GetILFunctionBodyAllocator(moduleId, &_methodAlloc);
				__RETURN_IF_FAILED(result);

				IMetaDataEmit* metaEmit;
				result = _corProfilerInfo2->GetModuleMetaData(moduleId, ofRead | ofWrite, IID_IMetaDataEmit, (IUnknown**)&metaEmit);
				__RETURN_IF_FAILED(result);

				IMetaDataAssemblyEmit* asmEmit;
				result = metaEmit->QueryInterface(IID_IMetaDataAssemblyEmit, (void**)&asmEmit);
				__RETURN_IF_FAILED(result);


				ASSEMBLYMETADATA amd = {0};
				mdAssemblyRef mscorlibToken;
				result = asmEmit->DefineAssemblyRef(null, 0, L"mscorlib", &amd, null, 0, 0, &mscorlibToken);
				__RETURN_IF_FAILED(result);

				mdModuleRef pinvokeModule;
				result = metaEmit->DefineModuleRef(pinvokeModuleName.c_str(), &pinvokeModule);
				__RETURN_IF_FAILED(result);

				mdTypeDef sysObjectToken;
				result = metaEmit->DefineTypeRefByName(mscorlibToken, L"System.Object", &sysObjectToken);
				__RETURN_IF_FAILED(result);

				mdTypeDef injectedClassToken;
				result = metaEmit->DefineTypeDef(injectedClassName.c_str(), tdAbstract|tdSealed, sysObjectToken, null, &injectedClassToken);
				__RETURN_IF_FAILED(result);

				//prologMethod ===============================================
				BYTE prologMethodSignature[] = {
					0, // Callconv: IMAGE_CEE_CS_CALLCONV_DEFAULT
					1, // Argument count: 1
					0x1, // Return type: ELEMENT_TYPE_VOID
					ELEMENT_TYPE_STRING //Argument type: string
				};
				result = metaEmit->DefineMethod(injectedClassToken, prologMethodName.c_str(), mdPublic|mdStatic|mdPinvokeImpl, prologMethodSignature,
												sizeof(prologMethodSignature), 0, 0, &_prologMethod);
				__RETURN_IF_FAILED(result);

				result = metaEmit->DefinePinvokeMap(_prologMethod, 0, prologMethodName.c_str(), pinvokeModule);
				__RETURN_IF_FAILED(result);

				mdParamDef prologMethodParam;
				result = metaEmit->DefineParam(_prologMethod, 1, L"arg0", pdIn|pdHasFieldMarshal, 0, NULL, 0, &prologMethodParam);
				__RETURN_IF_FAILED(result);
				
				BYTE paramType = NATIVE_TYPE_LPWSTR;
				result = metaEmit->SetFieldMarshal(prologMethodParam, &paramType, 1);
				__RETURN_IF_FAILED(result);
				
				//epilogMethod ===============================================

				BYTE epilogMethodSignature[] = {
					0, // Callconv: IMAGE_CEE_CS_CALLCONV_DEFAULT
					0, // Argument count: 0
					0x1 // Return type: ELEMENT_TYPE_VOID
					};
				result = metaEmit->DefineMethod(injectedClassToken, epilogMethodName.c_str(), mdPublic|mdStatic|mdPinvokeImpl, epilogMethodSignature,
												sizeof(epilogMethodSignature), 0, 0, &_epilogMethod);
				__RETURN_IF_FAILED(result);

				result = metaEmit->DefinePinvokeMap(_epilogMethod, 0, epilogMethodName.c_str(), pinvokeModule);
				__RETURN_IF_FAILED(result);


				return S_OK;
			}

			HRESULT MethodInjector::InjectById(FunctionID functionId)
			{
				ModuleID moduleId;
				mdMethodDef methodToken;
				HRESULT result = _corProfilerInfo2->GetFunctionInfo(functionId, 0, &moduleId, &methodToken);
				__RETURN_IF_FAILED(result);

				return InjectByToken(methodToken);
			}

			HRESULT MethodInjector::InjectByToken(mdMethodDef methodToken)
			{
				LPCBYTE methodHeader = NULL;
				ULONG methodSize = 0;
				HRESULT result;

				result = _corProfilerInfo2->GetILFunctionBody(_moduleId, methodToken, &methodHeader, &methodSize);
				__RETURN_IF_FAILED(result);

				if(((COR_ILMETHOD_FAT*)&((IMAGE_COR_ILMETHOD*)methodHeader)->Fat)->IsFat())
				{
					#include <pshpack1.h>
					struct { BYTE call; DWORD method_token; } prologCode;
					struct { BYTE call; DWORD method_token; } epilogCode;
					#include <poppack.h>

					prologCode.call = 0x28;
					prologCode.method_token = _prologMethod; //_prologMethod
					
					epilogCode.call = 0x28;
					epilogCode.method_token = _epilogMethod; //_prologMethod

					DWORD injectedCodeSize = sizeof(prologCode) + sizeof(epilogCode);

					COR_ILMETHOD_FAT* fatImage = (COR_ILMETHOD_FAT*)&((IMAGE_COR_ILMETHOD*)methodHeader)->Fat;
					IMAGE_COR_ILMETHOD* newMethod = (IMAGE_COR_ILMETHOD*) _methodAlloc->Alloc(methodSize + injectedCodeSize);
					if (newMethod == null)
					{
						return E_FAIL;
					}
					COR_ILMETHOD_FAT* newFatImage = (COR_ILMETHOD_FAT*)&newMethod->Fat;
					//Write header
					memcpy((BYTE*)newFatImage, (BYTE*)fatImage, fatImage->Size * sizeof(DWORD));
					
					BYTE* code = newFatImage->GetCode();
					//Write prolog code
					memcpy(code, (BYTE*)&prologCode, sizeof(prologCode));
					code += sizeof(prologCode);

					//Write old code
					memcpy(code, fatImage->GetCode(),  fatImage->CodeSize);
					code += fatImage->CodeSize;

					//Write epilog code
					memcpy(code, (BYTE*)&epilogCode, sizeof(epilogCode));
					code += sizeof(epilogCode);

					newFatImage->CodeSize += injectedCodeSize;
 
					_corProfilerInfo2->SetILFunctionBody(_moduleId, methodToken, (LPCBYTE)newMethod);
				}
				return S_OK;
			}
		}
	}
}