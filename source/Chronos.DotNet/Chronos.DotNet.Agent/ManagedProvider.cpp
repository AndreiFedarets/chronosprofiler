#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			ManagedProvider::ManagedProvider()
			{
				_currentCorProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo3), (void**)&_corProfilerInfo3);
				_currentCorProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo2), (void**)&_corProfilerInfo2);
			}
		
			ManagedProvider::~ManagedProvider(void)
			{
			}
			
			HRESULT ManagedProvider::Initialize(IUnknown* corProfilerInfoUnk)
			{
				if (_currentCorProfilerInfoUnk != null)
				{
					return E_FAIL;
				}
				_currentCorProfilerInfoUnk = corProfilerInfoUnk;
				return S_OK;
			}

			HRESULT ManagedProvider::GetClassFromObject(ObjectID objectId, ClassID* classId)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				HRESULT result = _corProfilerInfo3->GetClassFromObject(objectId, classId);
				return result;
			}

			HRESULT ManagedProvider::GetAppDomainInfo(AppDomainID appDomainId, ProcessID* processId, __string* name)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				HRESULT result;
				if (name == null)
				{
					result = _corProfilerInfo2->GetAppDomainInfo(appDomainId, 0, 0, null, processId);
				}
				else
				{
					const __uint NameMaxLength = 1000;
					__wchar nativeName[NameMaxLength];
					result = _corProfilerInfo2->GetAppDomainInfo(appDomainId, NameMaxLength, 0, (__wchar*)&nativeName, 0);
					name->assign(nativeName);
				}
				return result;
			}

			HRESULT ManagedProvider::GetAssemblyInfo(AssemblyID assemblyId, AppDomainID* appDomainId, __string* name)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				HRESULT result;
				if (name == null)
				{
					result = _corProfilerInfo2->GetAssemblyInfo(assemblyId, 0, 0, null, appDomainId, null);
				}
				else
				{
					const __uint NameMaxLength = 1000;
					__wchar nativeName[NameMaxLength];
					result = _corProfilerInfo2->GetAssemblyInfo(assemblyId, NameMaxLength, 0, nativeName, appDomainId, null);
					name->assign(nativeName);
				}
				return result;
			}

			HRESULT ManagedProvider::GetModuleInfo(ModuleID moduleID, AssemblyID* assemblyId, __string* name)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				HRESULT result;
				if (name == null)
				{
					result = _corProfilerInfo2->GetModuleInfo(moduleID, 0, 0, 0, 0, assemblyId);
				}
				else
				{
					const __uint NameMaxLength = 1000;
					__wchar nativeName[NameMaxLength];
					result = _corProfilerInfo2->GetModuleInfo(moduleID, 0, NameMaxLength, 0, nativeName, assemblyId);
					name->assign(nativeName);
				}
				return result;
			}

			HRESULT ManagedProvider::GetClassInfo(ClassID classId, ModuleID* moduleId, __string* name)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				mdTypeDef typeToken;
			
				HRESULT result = _corProfilerInfo2->GetClassIDInfo(classId, moduleId, &typeToken);
				__RETURN_IF_FAILED(result);

				if (name != null)
				{
					IMetaDataImport2* metaDataImport;
					result = _corProfilerInfo2->GetModuleMetaData(*moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &metaDataImport);
					__RETURN_IF_FAILED(result);
			
					const __uint NameMaxLength = 1000;
					__wchar nativeName[NameMaxLength];
					result = metaDataImport->GetTypeDefProps(typeToken, nativeName, NameMaxLength, 0, 0, 0);
					__RETURN_IF_FAILED(result);

					name->assign(nativeName);
				}
				return result;
			}


			HRESULT ManagedProvider::GetFunctionInfo(FunctionID functionId, AssemblyID* assemblyId, ModuleID* moduleId, ClassID* classId, mdToken* token, __string* name)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				HRESULT result;

				result = _corProfilerInfo2->GetFunctionInfo(functionId, classId, moduleId, token);
				__RETURN_IF_FAILED(result);

				if (classId != 0)
				{
					_corProfilerInfo2->GetModuleInfo(*moduleId, 0, 0, 0, 0, assemblyId);
				}

				if (name != null)
				{
					IMetaDataImport* metaDataImport = null;
					mdMethodDef methodDef = 0;
			
					result = _corProfilerInfo2->GetTokenAndMetaDataFromFunction(functionId, IID_IMetaDataImport, (IUnknown**)&metaDataImport, &methodDef);
					__RETURN_IF_FAILED(result);
			
					const __uint NameMaxLength = 1000;
					__wchar nativeName[NameMaxLength];
					result = metaDataImport->GetMethodProps(methodDef, 0, nativeName, NameMaxLength, 0, 0, 0, 0, 0, 0);
					__RETURN_IF_FAILED(result);
					name->assign(nativeName);
			
					metaDataImport->Release();
				}
				return result;
			}

			HRESULT ManagedProvider::GetCurrentThreadId(ThreadID* threadId)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				return _corProfilerInfo2->GetCurrentThreadID(threadId);
			}

			HRESULT ManagedProvider::GetHandleFromThread(ThreadID threadId, HANDLE* threadHandle)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				return _corProfilerInfo2->GetHandleFromThread(threadId, threadHandle);
			}

			HRESULT ManagedProvider::SetEventMask(DWORD eventsMask)
			{
				if (_corProfilerInfo2 == null)
				{
					return E_NOT_VALID_STATE;
				}
				return _corProfilerInfo2->SetEventMask(eventsMask);
			}

			HRESULT ManagedProvider::QueryInterface(const IID& riid, void** ppvObject)
			{
				if (riid == __uuidof(ICorProfilerInfo3))
				{
					if (_corProfilerInfo3 == null)
					{
						return E_NOINTERFACE;
					}
					*ppvObject = _corProfilerInfo3;
					return S_OK;
				}
				else if (riid == __uuidof(ICorProfilerInfo2))
				{
					*ppvObject = _corProfilerInfo2;
					return S_OK;
				}
				return E_NOINTERFACE;
			}

			HRESULT ManagedProvider::GetILFunctionBody(ModuleID moduleId, mdMethodDef methodToken, LPCBYTE *ppMethodHeader, ULONG *pcbMethodSize)
			{
				return _corProfilerInfo2->GetILFunctionBody(moduleId, methodToken, ppMethodHeader, pcbMethodSize);
			}

			HRESULT ManagedProvider::GetThreadInfo(ThreadID threadID, __uint* osThreadId)
			{
				return _corProfilerInfo2->GetThreadInfo(threadID, (DWORD*)osThreadId);
			}
			
			const __guid ManagedProvider::ServiceToken = Converter::ConvertStringToGuid(L"{3B6C61D5-A994-4DA2-8069-124A1BE877EF}");

			IUnknown* ManagedProvider::_currentCorProfilerInfoUnk = null;
		}
	}
}