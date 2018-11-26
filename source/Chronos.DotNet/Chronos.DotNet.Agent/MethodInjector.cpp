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
				_metadataProvider = metadataProvider;
				__RETURN_IF_FAILED(_initializeResult = metadataProvider->GetCorProfilerInfo2(&_corProfilerInfo));
				__RETURN_IF_FAILED(_initializeResult = _corProfilerInfo->GetILFunctionBodyAllocator(moduleId, &_methodAlloc));
				__RETURN_IF_FAILED(_initializeResult = _corProfilerInfo->GetModuleMetaData(moduleId, ofRead | ofWrite, IID_IMetaDataEmit, (IUnknown**)&_metadataEmit));
				__RETURN_IF_FAILED(_initializeResult = _corProfilerInfo->GetModuleMetaData(_moduleId, ofRead | ofWrite, IID_IMetaDataImport, (IUnknown **)&_metadataImport));

				Reflection::ModuleMetadata* moduleMetadata;
				__RETURN_IF_FAILED(metadataProvider->GetModule(_moduleId, &moduleMetadata));
				__string mscorlibName(L"mscorlib");
				Reflection::AssemblyReference* mscorlibReference = moduleMetadata->FindReference(&mscorlibName);
				if (mscorlibReference == null)
				{
					_initializeResult = E_FAIL;
					return _initializeResult;
				}

				mdModuleRef pinvokeModule;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineModuleRef(pinvokeModuleName.c_str(), &pinvokeModule));

				mdTypeDef sysObjectToken;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineTypeRefByName(mscorlibReference->GetToken(), L"System.Object", &sysObjectToken));

				mdTypeDef injectedClassToken;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineTypeDef(injectedClassName.c_str(), tdAbstract | tdSealed, sysObjectToken, null, &injectedClassToken));

				//prologMethod ===============================================
				BYTE prologMethodSignature[] = {
					IMAGE_CEE_CS_CALLCONV_DEFAULT, // Callconv
					1,                             // Argument count
					ELEMENT_TYPE_VOID,             // Return type
					ELEMENT_TYPE_STRING            // Argument type: string
				};
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineMethod(injectedClassToken, prologMethodName.c_str(), mdPublic | mdStatic | mdPinvokeImpl, prologMethodSignature,
					sizeof(prologMethodSignature), 0, miIL | miManaged | miPreserveSig, &_prologMethod));

				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefinePinvokeMap(_prologMethod, 0, prologMethodName.c_str(), pinvokeModule));

				mdParamDef prologMethodParam;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->DefineParam(_prologMethod, 1, L"arg0", pdIn | pdHasFieldMarshal, 0, NULL, 0, &prologMethodParam));
				
				BYTE paramType = NATIVE_TYPE_LPWSTR;
				__RETURN_IF_FAILED(_initializeResult = _metadataEmit->SetFieldMarshal(prologMethodParam, &paramType, 1));
				
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

			HRESULT MethodInjector::Inject(FunctionID functionId)
			{
				ModuleID moduleId;
				ClassID classId;
				mdMethodDef methodToken;
				__RETURN_IF_FAILED(_corProfilerInfo->GetFunctionInfo(functionId, &classId, &moduleId, &methodToken));

				IMAGE_COR_ILMETHOD* originalMethod = null;
				ULONG originalMethodSize = 0;

				__RETURN_IF_FAILED(_corProfilerInfo->GetILFunctionBody(_moduleId, methodToken, (LPCBYTE*)&originalMethod, &originalMethodSize));

				PCCOR_SIGNATURE corSignature;
				ULONG corSignatureSize;
				__RETURN_IF_FAILED(_metadataImport->GetMethodProps(methodToken, 0, 0, 0, 0, 0, &corSignature, &corSignatureSize, 0, 0));
				
				Reflection::Emit::Method* method = Reflection::Emit::MethodManager::Read(originalMethod);
				Reflection::Emit::MethodManager::WriteDebug(method);

				mdFieldDef commandTextFieldToken = 0;
				//find _commandText field token
				{
					
					Reflection::TypeMetadata* typeMetadata;
					__RETURN_IF_FAILED(_metadataProvider->GetType(classId, &typeMetadata));
					__string commandTextFieldName(L"_commandText");
					Reflection::FieldMetadata* fieldMetadata = typeMetadata->FindField(&commandTextFieldName);
					if (fieldMetadata == null)
					{
						return E_FAIL;
					}
					commandTextFieldToken = fieldMetadata->GetToken();
				}

				//inject one more local variable of method return type if not VOID
				__bool saveReturn;
				__byte returnLocalIndex;
				{
					Reflection::Emit::Signature* argSignature = Reflection::Emit::SignatureManager::Read(corSignature);
					saveReturn = argSignature->Front->ElementType != CorElementType::ELEMENT_TYPE_VOID;
					if (saveReturn)
					{
						Reflection::Emit::Signature* localSignature = Reflection::Emit::SignatureManager::Read(method->LocalVarSigTok, _metadataImport);
						returnLocalIndex = Reflection::Emit::SignatureManager::InsertElement(localSignature, argSignature->Front);
						mdSignature localVarSigTok = Reflection::Emit::SignatureManager::Write(localSignature, _metadataEmit);
						method->LocalVarSigTok = localVarSigTok;
					}
				}
				//insert prolog
				{
					Reflection::Emit::Instruction* loadThisInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ldarg_0);
					Reflection::Emit::MethodManager::InsertChainBefore(method, method->FrontInstruction, loadThisInstruction);

					Reflection::Emit::Instruction* loadCommandTextInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ldfld, commandTextFieldToken);
					Reflection::Emit::MethodManager::InsertChainAfter(method, loadThisInstruction, loadCommandTextInstruction);

					Reflection::Emit::Instruction* prologCall = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _prologMethod);
					Reflection::Emit::MethodManager::InsertChainAfter(method, loadCommandTextInstruction, prologCall);
				}
				//insert epilog
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

					if (saveReturn)
					{
						Reflection::Emit::Instruction* setLocalInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Stloc_S, returnLocalIndex);
						insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, setLocalInstruction);
					}

					Reflection::Emit::Instruction* epilogCall = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _epilogMethod);
					insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, epilogCall);

					if (saveReturn)
					{
						Reflection::Emit::Instruction* loadLocalInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ldloc_S, returnLocalIndex);
						insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, loadLocalInstruction);
					}
					
					Reflection::Emit::ExceptionHandlerManager::DefineTryFinally(method, method->FrontInstruction, epilogCall->Previous, epilogCall, epilogCall);
				}

				Reflection::Emit::MethodManager::WriteDebug(method);

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