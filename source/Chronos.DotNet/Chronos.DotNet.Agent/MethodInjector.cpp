#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"
#include <corhlpr.h>

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			MethodInjector::MethodInjector()
			{
				_metadataEmit = null;
				_metadataImport = null;
				_methodAlloc = null;
				_corProfilerInfo = null;
			}

			MethodInjector::~MethodInjector()
			{
				if (_metadataEmit != null)
				{
					_metadataEmit->Release();
				}
				if (_metadataImport != null)
				{
					_metadataImport->Release();
				}
				if (_methodAlloc != null)
				{
					_methodAlloc->Release();
				}
			}

			HRESULT MethodInjector::Initialize(Reflection::RuntimeMetadataProvider* metadataProvider, ModuleID moduleId, std::wstring pinvokeModuleName, std::wstring injectedClassName, std::wstring prologMethodName, std::wstring epilogMethodName)
			{
				_moduleId = moduleId;

				__RETURN_IF_FAILED(_initializeResult = metadataProvider->GetCorProfilerInfo2(&_corProfilerInfo));
				__RETURN_IF_FAILED(_initializeResult = _corProfilerInfo->GetILFunctionBodyAllocator(moduleId, &_methodAlloc));
				__RETURN_IF_FAILED(_initializeResult = _corProfilerInfo->GetModuleMetaData(moduleId, ofRead | ofWrite, IID_IMetaDataEmit, (IUnknown**)&_metadataEmit));
				__RETURN_IF_FAILED(_initializeResult = _corProfilerInfo->GetModuleMetaData(_moduleId, ofRead | ofWrite, IID_IMetaDataImport, (IUnknown **)&_metadataImport));

				IMetaDataAssemblyEmit* assemblyEmit = null;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->QueryInterface(IID_IMetaDataAssemblyEmit, (void**)&assemblyEmit));

				IMetaDataAssemblyImport* assemblyImport = null;
				__RETURN_IF_FAILED(_initializeResult = _corProfilerInfo->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataAssemblyImport, (IUnknown**)&assemblyImport));

				BYTE* publicKeyToken = null;
				ULONG publicKeyTokenSize = 0;

				HCORENUM hEnumAssembly = NULL;
				mdAssemblyRef rgAssemblyRefs[32]{ 0 };
				ULONG numberOfTokens = 0;
				__RETURN_IF_FAILED(_initializeResult = assemblyImport->EnumAssemblyRefs(&hEnumAssembly, rgAssemblyRefs, _countof(rgAssemblyRefs), &numberOfTokens));

				for (size_t i = 0; i < numberOfTokens; i++)
				{
					wchar_t assemblyName[255] { 0 };
					ULONG assemblyNameLength = 0;
					char* hashValue = null;
					ULONG hashLength = 0;
					DWORD flags = 0;
					ASSEMBLYMETADATA assemblyMetadata{ 0 };
					HRESULT result = assemblyImport->GetAssemblyRefProps(rgAssemblyRefs[i], (const void**)&publicKeyToken, &publicKeyTokenSize, assemblyName, _countof(assemblyName), &assemblyNameLength, &assemblyMetadata, (const void**)&hashValue, &hashLength, &flags);
					if (SUCCEEDED(result) && __string(L"mscorlib").compare(assemblyName) == 0)
					{
						break;
					}
				}


				ASSEMBLYMETADATA amd = {0};
				mdAssemblyRef mscorlibToken;
				__RETURN_IF_FAILED(_initializeResult = assemblyEmit->DefineAssemblyRef(publicKeyToken, publicKeyTokenSize, L"mscorlib", &amd, null, 0, 0, &mscorlibToken));

				mdModuleRef pinvokeModule;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineModuleRef(pinvokeModuleName.c_str(), &pinvokeModule));

				mdTypeDef sysObjectToken;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineTypeRefByName(mscorlibToken, L"System.Object", &sysObjectToken));

				mdTypeDef injectedClassToken;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineTypeDef(injectedClassName.c_str(), tdAbstract | tdSealed, sysObjectToken, null, &injectedClassToken));

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
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineMethod(injectedClassToken, prologMethodName.c_str(), mdPublic | mdStatic | mdPinvokeImpl, prologMethodSignature,
					                           sizeof(prologMethodSignature), 0, miIL|miManaged|miPreserveSig, &_prologMethod));

				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefinePinvokeMap(_prologMethod, 0, prologMethodName.c_str(), pinvokeModule));

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
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineMethod(injectedClassToken, epilogMethodName.c_str(), mdPublic | mdStatic | mdPinvokeImpl, epilogMethodSignature,
					                           sizeof(epilogMethodSignature), 0, miIL|miManaged|miPreserveSig, &_epilogMethod));

				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefinePinvokeMap(_epilogMethod, 0, epilogMethodName.c_str(), pinvokeModule));

				return S_OK;
			}

			HRESULT MethodInjector::InjectById(FunctionID functionId)
			{
				ModuleID moduleId;
				mdMethodDef methodToken;
				__RETURN_IF_FAILED(_corProfilerInfo->GetFunctionInfo(functionId, 0, &moduleId, &methodToken));
				return InjectByToken(methodToken);
			}

			HRESULT MethodInjector::InjectByToken(mdMethodDef methodToken)
			{
				IMAGE_COR_ILMETHOD* originalMethod = null;
				ULONG originalMethodSize = 0;

				__RETURN_IF_FAILED(_corProfilerInfo->GetILFunctionBody(_moduleId, methodToken, (LPCBYTE*)&originalMethod, &originalMethodSize));

				PCCOR_SIGNATURE corSignature;
				ULONG corSignatureSize;
				__RETURN_IF_FAILED(_metadataImport->GetMethodProps(methodToken, 0, 0, 0, 0, 0, &corSignature, &corSignatureSize, 0, 0));
				
				Reflection::Emit::Method* method = Reflection::Emit::MethodManager::Read(originalMethod);
				Reflection::Emit::Signature* argSignature = Reflection::Emit::SignatureManager::Read(corSignature);
				Reflection::Emit::Signature* localSignature = Reflection::Emit::SignatureManager::Read(method->LocalVarSigTok, _metadataImport);
				__byte localIndex = Reflection::Emit::SignatureManager::InsertElement(localSignature, argSignature->Front);

				mdSignature localVarSigTok = Reflection::Emit::SignatureManager::Write(localSignature, _metadataEmit);
				method->LocalVarSigTok = localVarSigTok;

				//Reflection::Emit::MethodManager::WriteDebug(method);
				//Insert prolog
				{
					Reflection::Emit::Instruction* prologCall = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _prologMethod);
					Reflection::Emit::MethodManager::InsertChainBefore(method, method->FrontInstruction, prologCall);
				}
				//Insert epilog
				{
					//NOTE: method could have many RET instructions (e.g. in SWITCH). TODO: handle this case
					Reflection::Emit::Instruction* finalInstruction = Reflection::Emit::InstructionManager::MoveToFinal(method->FrontInstruction);
					Reflection::Emit::Instruction* returnInstruction = Reflection::Emit::InstructionManager::LookBackward(finalInstruction, Reflection::Emit::OpCodes::Ret);
					Reflection::Emit::Instruction* insertAfterInstruction = null;
					if (returnInstruction == null)
					{
						returnInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ret);
						finalInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, finalInstruction, returnInstruction);
					}
					insertAfterInstruction = returnInstruction->Previous;

					Reflection::Emit::Instruction* setLocalInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Stloc_S, localIndex);
					insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, setLocalInstruction);

					Reflection::Emit::Instruction* epilogCall = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _epilogMethod);
					insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, epilogCall);

					Reflection::Emit::Instruction* loadLocalInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ldloc_S, localIndex);
					insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, loadLocalInstruction);
					
					Reflection::Emit::ExceptionHandlerManager::DefineTryFinally(method, method->FrontInstruction, epilogCall->Previous, epilogCall, epilogCall);
				}

				//Reflection::Emit::MethodManager::WriteDebug(method);

				__uint copyMethodSize = Reflection::Emit::MethodManager::GetSize(method);
				BYTE* copyMethodData = (BYTE*)_methodAlloc->Alloc(copyMethodSize);
				Reflection::Emit::MethodManager::WriteTo(method, copyMethodData);
				__RETURN_IF_FAILED(_corProfilerInfo->SetILFunctionBody(_moduleId, methodToken, copyMethodData));

				Reflection::Emit::MethodManager::Release(method);
				return S_OK;
			}
		}
	}
}