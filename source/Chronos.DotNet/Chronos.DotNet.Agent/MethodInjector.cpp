#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"
#include <corhlpr.h>

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			MethodInjector::MethodInjector(Reflection::RuntimeMetadataProvider* metadataProvider)
			{
				_metadataProvider = metadataProvider;
			}

			MethodInjector::~MethodInjector()
			{
			}

			HRESULT MethodInjector::Initialize(ModuleID moduleId, std::wstring pinvokeModuleName, std::wstring injectedClassName, std::wstring prologMethodName, std::wstring epilogMethodName)
			{
				_moduleId = moduleId;
				HRESULT result;

				ICorProfilerInfo3* corProfilerInfo = null;
				result = _metadataProvider->GetCorProfilerInfo3(&corProfilerInfo);
				__RETURN_IF_FAILED(result);

				result = corProfilerInfo->GetILFunctionBodyAllocator(moduleId, &_methodAlloc);
				__RETURN_IF_FAILED(result);

				IMetaDataEmit* metaEmit;
				result = corProfilerInfo->GetModuleMetaData(moduleId, ofRead | ofWrite, IID_IMetaDataEmit, (IUnknown**)&metaEmit);
				__RETURN_IF_FAILED(result);

				IMetaDataAssemblyEmit* asmEmit;
				result = metaEmit->QueryInterface(IID_IMetaDataAssemblyEmit, (void**)&asmEmit);
				__RETURN_IF_FAILED(result);


				BYTE* publicKeyToken = null;
				ULONG publicKeyTokenSize = 0;

				IMetaDataAssemblyImport* assemblyImport = null;
				corProfilerInfo->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataAssemblyImport, (IUnknown**)&assemblyImport);

				HCORENUM hEnumAssembly = NULL;
				mdAssemblyRef rgAssemblyRefs[32]{ 0 };
				ULONG numberOfTokens = 0;
				result = assemblyImport->EnumAssemblyRefs(&hEnumAssembly, rgAssemblyRefs, _countof(rgAssemblyRefs),	&numberOfTokens);
				__RETURN_IF_FAILED(result);


				wchar_t assemblyRefNameBuffer[255] { 0 };
				ULONG numChars = 0;
				char* hashVal = NULL;
				ULONG hashLen = 0;
				DWORD flags = 0;
				ASSEMBLYMETADATA amd2{ 0 };
				for (size_t i = 0; i < numberOfTokens; i++)
				{
					result = assemblyImport->GetAssemblyRefProps(rgAssemblyRefs[i], (const void**)&publicKeyToken, &publicKeyTokenSize, assemblyRefNameBuffer, _countof(assemblyRefNameBuffer), &numChars, &amd2, (const void**)&hashVal, &hashLen, &flags);
					if (SUCCEEDED(result) && __string(L"mscorlib").compare(assemblyRefNameBuffer) == 0)
					{
						break;
					}
				}


				ASSEMBLYMETADATA amd = {0};
				mdAssemblyRef mscorlibToken;
				result = asmEmit->DefineAssemblyRef(publicKeyToken, publicKeyTokenSize, L"mscorlib", &amd, null, 0, 0, &mscorlibToken);
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
				//BYTE prologMethodSignature[] = {
				//	0, // Callconv: IMAGE_CEE_CS_CALLCONV_DEFAULT
				//	1, // Argument count: 1
				//	0x1, // Return type: ELEMENT_TYPE_VOID
				//	ELEMENT_TYPE_STRING //Argument type: string
				//};
				BYTE prologMethodSignature[] = {
					IMAGE_CEE_CS_CALLCONV_DEFAULT, // Callconv
					0,                             // Argument count
					ELEMENT_TYPE_VOID              // Return type
				};
				result = metaEmit->DefineMethod(injectedClassToken, prologMethodName.c_str(), mdPublic|mdStatic|mdPinvokeImpl, prologMethodSignature,
					                           sizeof(prologMethodSignature), 0, miIL|miManaged|miPreserveSig, &_prologMethod);
				__RETURN_IF_FAILED(result);

				result = metaEmit->DefinePinvokeMap(_prologMethod, 0, prologMethodName.c_str(), pinvokeModule);
				__RETURN_IF_FAILED(result);

				/*mdParamDef prologMethodParam;
				result = metaEmit->DefineParam(_prologMethod, 1, L"arg0", pdIn|pdHasFieldMarshal, 0, NULL, 0, &prologMethodParam);
				__RETURN_IF_FAILED(result);
				
				BYTE paramType = NATIVE_TYPE_LPWSTR;
				result = metaEmit->SetFieldMarshal(prologMethodParam, &paramType, 1);
				__RETURN_IF_FAILED(result);*/
				
				//epilogMethod ===============================================

				BYTE epilogMethodSignature[] = {
					IMAGE_CEE_CS_CALLCONV_DEFAULT, // Callconv
					0,                             // Argument count
					ELEMENT_TYPE_VOID              // Return type
					};
				result = metaEmit->DefineMethod(injectedClassToken, epilogMethodName.c_str(), mdPublic|mdStatic|mdPinvokeImpl, epilogMethodSignature,
					                           sizeof(epilogMethodSignature), 0, miIL|miManaged|miPreserveSig, &_epilogMethod);
				__RETURN_IF_FAILED(result);

				result = metaEmit->DefinePinvokeMap(_epilogMethod, 0, epilogMethodName.c_str(), pinvokeModule);
				__RETURN_IF_FAILED(result);

				return S_OK;
			}

			HRESULT MethodInjector::InjectById(FunctionID functionId)
			{
				HRESULT result;

				ICorProfilerInfo3* corProfilerInfo = null;
				result = _metadataProvider->GetCorProfilerInfo3(&corProfilerInfo);
				__RETURN_IF_FAILED(result);

				ModuleID moduleId;
				mdMethodDef methodToken;
				result = corProfilerInfo->GetFunctionInfo(functionId, 0, &moduleId, &methodToken);
				__RETURN_IF_FAILED(result);

				return InjectByToken(methodToken);
			}

			HRESULT MethodInjector::InjectByToken(mdMethodDef methodToken)
			{
				IMAGE_COR_ILMETHOD* originalMethod = null;
				ULONG originalMethodSize = 0;
				HRESULT result;

				ICorProfilerInfo3* corProfilerInfo = null;
				result = _metadataProvider->GetCorProfilerInfo3(&corProfilerInfo);
				__RETURN_IF_FAILED(result);

				result = corProfilerInfo->GetILFunctionBody(_moduleId, methodToken, (LPCBYTE*)&originalMethod, &originalMethodSize);
				__RETURN_IF_FAILED(result);

				
				Reflection::Emit::Method* method = Reflection::Emit::MethodManager::Read(originalMethod);

				//Reflection::Emit::Instruction* prolog = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _prologMethod);
				//Reflection::Emit::Instruction* instruction = method->FrontInstruction;
				//method->InsertBefore(instruction, prolog);
				//Reflection::Emit::Instruction* epilog = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _epilogMethod);

				__uint copyMethodSize = Reflection::Emit::MethodManager::GetSize(method);
				BYTE* copyMethodData = (BYTE*)_methodAlloc->Alloc(copyMethodSize);
				Reflection::Emit::MethodManager::WriteTo(method, copyMethodData);
				corProfilerInfo->SetILFunctionBody(_moduleId, methodToken, copyMethodData);

				Reflection::Emit::MethodManager::Release(method);

				//#include <pshpack1.h>
				//struct { BYTE call; DWORD method_token; } prologCode;
				//struct { BYTE leave_s; BYTE leave_s_delta; } preEpilogCode;
				//struct { BYTE call; DWORD method_token; BYTE endfinally; } epilogCode;
				//#include <poppack.h>

				//prologCode.call = Reflection::Emit::OpCodes::Call->Token;
				//prologCode.method_token = _prologMethod;

				//preEpilogCode.leave_s = Reflection::Emit::OpCodes::Leave_S->Token;
				//preEpilogCode.leave_s_delta = sizeof(epilogCode);

				//epilogCode.call = Reflection::Emit::OpCodes::Call->Token;
				//epilogCode.method_token = _epilogMethod;
				//epilogCode.endfinally = Reflection::Emit::OpCodes::Endfinally->Token;

				//method->InsertBegin((__byte*)&prologCode, sizeof(prologCode));
				//method->InsertEnd((__byte*)&preEpilogCode, sizeof(preEpilogCode));
				//method->InsertEnd((__byte*)&epilogCode, sizeof(epilogCode));

				//Reflection::Emit::MethodExceptionSection* section = method->GetOrCreateExceptionSection();
				//__byte lastOpCode = method->GetLastOpCode();
				//__uint tryOffset = 0;
				//__uint tryLength = method->CodeSize - sizeof(epilogCode);
				//if (lastOpCode == Reflection::Emit::OpCodes::Ret->Token || lastOpCode == Reflection::Emit::OpCodes::Throw->Token)
				//{
				//	tryLength--;
				//}
				//__uint handlerOffset = tryOffset + tryLength;
				//__uint handlerLength = sizeof(epilogCode);
				//section->AddFinally(tryOffset, tryLength, handlerOffset, handlerLength);

				//__uint copyMethodSize = method->GetFullSize();
				//BYTE* copyMethodData = (BYTE*)_methodAlloc->Alloc(copyMethodSize);
				//method->Copy(copyMethodData);
				//corProfilerInfo->SetILFunctionBody(_moduleId, methodToken, copyMethodData);
				







				//IMAGE_COR_ILMETHOD* newMethod = (IMAGE_COR_ILMETHOD*) _methodAlloc->Alloc(methodSize + injectedCodeSize);
				//if (newMethod == null)
				//{
				//	return E_FAIL;
				//}
				//COR_ILMETHOD_FAT* newFatImage = (COR_ILMETHOD_FAT*)&newMethod->Fat;
				////Write header
				//memcpy((BYTE*)newFatImage, (BYTE*)originalFatMethod, originalFatMethod->Size * sizeof(DWORD));
				//	
				//BYTE* code = newFatImage->GetCode();
				////Write prolog code
				//memcpy(code, (BYTE*)&prologCode, sizeof(prologCode));
				//code += sizeof(prologCode);

				////Write old code
				//memcpy(code, originalFatMethod->GetCode(), originalFatMethod->CodeSize);
				//code += originalFatMethod->CodeSize;

				////Write epilog code
				//memcpy(code, (BYTE*)&epilogCode, sizeof(epilogCode));
				//code += sizeof(epilogCode);

				//newFatImage->CodeSize += injectedCodeSize;

				//_corProfilerInfo2->SetILFunctionBody(_moduleId, methodToken, (LPCBYTE)newMethod);
				//-----------------------------------------------------------------------------------------------
				//if (originalFatMethod->IsFat())
				//{
				//	IMAGE_COR_ILMETHOD* newMethod = (IMAGE_COR_ILMETHOD*) _methodAlloc->Alloc(methodSize + injectedCodeSize);
				//	if (newMethod == null)
				//	{
				//		return E_FAIL;
				//	}
				//	COR_ILMETHOD_FAT* newFatImage = (COR_ILMETHOD_FAT*)&newMethod->Fat;
				//	//Write header
				//	memcpy((BYTE*)newFatImage, (BYTE*)originalFatMethod, originalFatMethod->Size * sizeof(DWORD));
				//	
				//	BYTE* code = newFatImage->GetCode();
				//	//Write prolog code
				//	memcpy(code, (BYTE*)&prologCode, sizeof(prologCode));
				//	code += sizeof(prologCode);

				//	//Write old code
				//	memcpy(code, originalFatMethod->GetCode(), originalFatMethod->CodeSize);
				//	code += originalFatMethod->CodeSize;

				//	//Write epilog code
				//	memcpy(code, (BYTE*)&epilogCode, sizeof(epilogCode));
				//	code += sizeof(epilogCode);

				//	newFatImage->CodeSize += injectedCodeSize;
 
				//	_corProfilerInfo2->SetILFunctionBody(_moduleId, methodToken, (LPCBYTE)newMethod);
				//}
				//else
				//{
				//	if (originalTinyMethod->GetCodeSize() + injectedCodeSize < 64)
				//	{
				//		IMAGE_COR_ILMETHOD* modifiedMethod = (IMAGE_COR_ILMETHOD*)_methodAlloc->Alloc(methodSize + sizeof(epilogCode) + sizeof(prologCode));
				//		if (modifiedMethod == null)
				//		{
				//			return E_FAIL;
				//		}
				//		COR_ILMETHOD_TINY* modifiedTinyMethod = (COR_ILMETHOD_TINY*)&modifiedMethod->Tiny;

				//		//Write header
				//		memcpy((BYTE*)modifiedTinyMethod, (BYTE*)originalTinyMethod, sizeof(COR_ILMETHOD_TINY));

				//		//Get code
				//		BYTE* originalCode = originalTinyMethod->GetCode();
				//		BYTE* modifiedCode = modifiedTinyMethod->GetCode();
				//		__byte originalCodeSize = originalTinyMethod->GetCodeSize();
				//		BYTE retCode = 0x2A;
				//		__bool endsWithRet = (originalCode[originalCodeSize - 1] == retCode);

				//		//Write prolog code
				//		memcpy(modifiedCode, (BYTE*)&prologCode, sizeof(prologCode));
				//		modifiedCode += sizeof(prologCode);

				//		if (endsWithRet)
				//		{
				//			//Write old code (without last opcode RET)
				//			memcpy(modifiedCode, originalCode, originalCodeSize - 1);
				//			modifiedCode += originalCodeSize - 1;
				//		}
				//		else
				//		{
				//			//Write old code
				//			memcpy(modifiedCode, originalCode, originalCodeSize);
				//			modifiedCode += originalCodeSize;
				//		}

				//		//Write epilog code
				//		memcpy(modifiedCode, (BYTE*)&epilogCode, sizeof(epilogCode));
				//		modifiedCode += sizeof(epilogCode);

				//		//Write RET code
				//		memcpy(modifiedCode, (BYTE*)&retCode, sizeof(BYTE));
				//		modifiedCode += sizeof(BYTE);

				//		if (!endsWithRet)
				//		{
				//			originalCodeSize++;
				//		}

				//		__byte codeSize = originalCodeSize + sizeof(prologCode) + sizeof(epilogCode);
				//		modifiedTinyMethod->Flags_CodeSize = (codeSize << (CorILMethod_FormatShift - 1)) | CorILMethod_TinyFormat;

				//		_corProfilerInfo2->SetILFunctionBody(_moduleId, methodToken, (LPCBYTE)modifiedMethod);

				//		//IMAGE_COR_ILMETHOD* newMethod = (IMAGE_COR_ILMETHOD*)_methodAlloc->Alloc(methodSize + injectedCodeSize);
				//		//if (newMethod == null)
				//		//{
				//		//	return E_FAIL;
				//		//}
				//		//COR_ILMETHOD_TINY* newTinyImage = (COR_ILMETHOD_TINY*)&newMethod->Tiny;

				//		////Write header
				//		//memcpy((BYTE*)newTinyImage, (BYTE*)tinyImage, sizeof(COR_ILMETHOD_TINY));

				//		//BYTE* code = newTinyImage->GetCode();
				//		////Write prolog code
				//		//memcpy(code, (BYTE*)&prologCode, sizeof(prologCode));
				//		//code += sizeof(prologCode);

				//		////Write old code
				//		//memcpy(code, tinyImage->GetCode(), tinyImage->GetCodeSize());
				//		//code += tinyImage->GetCodeSize();

				//		////Write epilog code
				//		//memcpy(code, (BYTE*)&epilogCode, sizeof(epilogCode));
				//		//code += sizeof(epilogCode);

				//		//int codeSize = tinyImage->GetCodeSize() + injectedCodeSize - sizeof(COR_ILMETHOD_TINY);
				//		//((BYTE*)&newTinyImage)[0] = CorILMethod_TinyFormat1 | ((codeSize & 0xff) << 2);

				//		//_corProfilerInfo2->SetILFunctionBody(_moduleId, methodToken, (LPCBYTE)newMethod);
				//	}
				//	else
				//	{


				//		//IMAGE_COR_ILMETHOD* newMethod = (IMAGE_COR_ILMETHOD*)_methodAlloc->Alloc(methodSize + injectedCodeSize);
				//		//if (newMethod == null)
				//		//{
				//		//	return E_FAIL;
				//		//}
				//		//COR_ILMETHOD_FAT* newFatImage = (COR_ILMETHOD_FAT*)&newMethod->Fat;
				//		////Write header
				//		//memcpy((BYTE*)newFatImage, (BYTE*)tinyImage, tinyImage->GetCodeSize() * sizeof(DWORD));

				//		//BYTE* code = newFatImage->GetCode();
				//		////Write prolog code
				//		//memcpy(code, (BYTE*)&prologCode, sizeof(prologCode));
				//		//code += sizeof(prologCode);

				//		////Write old code
				//		//memcpy(code, tinyImage->GetCode(), tinyImage->CodeSize);
				//		//code += fatImage->CodeSize;

				//		////Write epilog code
				//		//memcpy(code, (BYTE*)&epilogCode, sizeof(epilogCode));
				//		//code += sizeof(epilogCode);

				//		////newFatImage->CodeSize += injectedCodeSize;

				//		////_corProfilerInfo2->SetILFunctionBody(_moduleId, methodToken, (LPCBYTE)newMethod);
				//	}
				//}
				return S_OK;
			}
		}
	}
}