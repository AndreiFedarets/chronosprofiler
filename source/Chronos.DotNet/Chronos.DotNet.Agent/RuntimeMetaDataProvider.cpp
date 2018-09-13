#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace Reflection
			{
				RuntimeMetadataProvider::RuntimeMetadataProvider()
				{
					_corProfilerInfo3 = null;
					_corProfilerInfo2 = null;
					_appDomain = new MetadataCollection<AppDomainMetadata>();
					_assemblies = new MetadataCollection<AssemblyMetadata>();
					_modules = new MetadataCollection<ModuleMetadata>();
					_types = new MetadataCollection<TypeMetadata>();
					_methods = new MetadataCollection<MethodMetadata>();
					_threads = new MetadataCollection<ThreadMetadata>();
				}

				RuntimeMetadataProvider::~RuntimeMetadataProvider()
				{
					__FREEOBJ(_appDomain);
					__FREEOBJ(_assemblies);
					__FREEOBJ(_modules);
					__FREEOBJ(_types);
					__FREEOBJ(_methods);
					__FREEOBJ(_threads);
				}

				HRESULT RuntimeMetadataProvider::Initialize(IUnknown* corProfilerInfo)
				{
					if (ÑorProfilerInfoUnk == null)
					{
						ÑorProfilerInfoUnk = corProfilerInfo;
						return S_OK;
					}
					return E_FAIL;
				}

				ICorProfilerInfo2* RuntimeMetadataProvider::GetCorProfilerInfo2()
				{
					if (_corProfilerInfo2 == null)
					{
						ÑorProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo3), (void**)&_corProfilerInfo2);
					}
					return _corProfilerInfo2;
				}

				ICorProfilerInfo3* RuntimeMetadataProvider::GetCorProfilerInfo3()
				{
					if (_corProfilerInfo3 == null)
					{
						ÑorProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo3), (void**)&_corProfilerInfo3);
					}
					return _corProfilerInfo3;
				}
			
				HRESULT RuntimeMetadataProvider::GetAppDomain(AppDomainID appDomainId, AppDomainMetadata** metadata)
				{
					*metadata = _appDomain->Find(appDomainId);
					if (*metadata == null)
					{
						*metadata = new AppDomainMetadata(GetCorProfilerInfo2(), this, appDomainId);
						_appDomain->Add(appDomainId, *metadata);
					}
					return S_OK;
				}

				HRESULT RuntimeMetadataProvider::GetAssembly(AssemblyID assemblyId, AssemblyMetadata** metadata)
				{
					*metadata = _assemblies->Find(assemblyId);
					if (*metadata == null)
					{
						*metadata = new AssemblyMetadata(GetCorProfilerInfo2(), this, assemblyId);
						_assemblies->Add(assemblyId, *metadata);
					}
					return S_OK;
				}

				HRESULT RuntimeMetadataProvider::GetModule(ModuleID moduleId, ModuleMetadata** metadata)
				{
					*metadata = _modules->Find(moduleId);
					if (*metadata == null)
					{
						*metadata = new ModuleMetadata(GetCorProfilerInfo2(), this, moduleId);
						_modules->Add(moduleId, *metadata);
					}
					return S_OK;
				}

				HRESULT RuntimeMetadataProvider::GetType(ClassID classId, TypeMetadata** metadata)
				{
					*metadata = _types->Find(classId);
					if (*metadata == null)
					{
						*metadata = new TypeMetadata(GetCorProfilerInfo2(), this, classId);
						_types->Add(classId, *metadata);
					}
					return S_OK;
				}

				HRESULT RuntimeMetadataProvider::GetMethod(FunctionID functionId, MethodMetadata** metadata)
				{
					*metadata = _methods->Find(functionId);
					if (*metadata == null)
					{
						*metadata = new MethodMetadata(GetCorProfilerInfo2(), this, functionId);
						_methods->Add(functionId, *metadata);
					}
					return S_OK;
				}
				
				HRESULT RuntimeMetadataProvider::GetThread(ThreadID threadId, ThreadMetadata** metadata)
				{
					*metadata = _threads->Find(threadId);
					if (*metadata == null)
					{
						*metadata = new ThreadMetadata(GetCorProfilerInfo2(), this, threadId);
						_threads->Add(threadId, *metadata);
					}
					return S_OK;
				}

				HRESULT RuntimeMetadataProvider::GetClassFromObject(ObjectID objectId, ClassID* classId)
				{
					if (GetCorProfilerInfo2() == null)
					{
						return E_NOINTERFACE;
					}
					return GetCorProfilerInfo2()->GetClassFromObject(objectId, classId);
				}

				/*ObjectMetadata* RuntimeMetadataProvider::GetObject(ObjectID objectId)
				{
					return new ObjectMetadata(GetCorProfilerInfo2(), this, objectId);
				}*/

				HRESULT RuntimeMetadataProvider::GetCorProfilerInfo2(ICorProfilerInfo2** profilerInfo)
				{
					*profilerInfo = GetCorProfilerInfo2();
					if (*profilerInfo == null)
					{
						return E_NOINTERFACE;
					}
					return S_OK;
				}

				HRESULT RuntimeMetadataProvider::GetCorProfilerInfo3(ICorProfilerInfo3** profilerInfo)
				{
					*profilerInfo = GetCorProfilerInfo3();
					if (*profilerInfo == null)
					{
						return E_NOINTERFACE;
					}
					return S_OK;
				}

				HRESULT RuntimeMetadataProvider::SetEventMask(DWORD eventsMask)
				{
					if (GetCorProfilerInfo2() == null)
					{
						return E_NOT_VALID_STATE;
					}
					return GetCorProfilerInfo2()->SetEventMask(eventsMask);
				}

				HRESULT RuntimeMetadataProvider::GetCurrentThreadId(ThreadID* threadId)
				{
					if (GetCorProfilerInfo2() == null)
					{
						return E_NOT_VALID_STATE;
					}
					return GetCorProfilerInfo2()->GetCurrentThreadID(threadId);
				}

				HRESULT RuntimeMetadataProvider::GetHandleFromThread(ThreadID threadId, HANDLE* threadHandle)
				{
					if (GetCorProfilerInfo2() == null)
					{
						return E_NOT_VALID_STATE;
					}
					return GetCorProfilerInfo2()->GetHandleFromThread(threadId, threadHandle);
				}

				const __guid RuntimeMetadataProvider::ServiceToken = Converter::ConvertStringToGuid(RuntimeMetadataProviderServiceToken);

				IUnknown* RuntimeMetadataProvider::ÑorProfilerInfoUnk = null;
			}
		}
	}
}