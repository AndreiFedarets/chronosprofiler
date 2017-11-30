#include "stdafx.h"
#include "Chronos.Java.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			namespace Reflection
			{
				RuntimeMetadataProvider::RuntimeMetadataProvider()
				{
					/*_appDomain = new MetadataCollection<AppDomainMetadata>();
					_assemblies = new MetadataCollection<AssemblyMetadata>();
					_modules = new MetadataCollection<ModuleMetadata>();
					_types = new MetadataCollection<TypeMetadata>();
					_methods = new MetadataCollection<MethodMetadata>();
					_threads = new MetadataCollection<ThreadMetadata>();*/
				}

				RuntimeMetadataProvider::~RuntimeMetadataProvider()
				{
					/*__FREEOBJ(_appDomain);
					__FREEOBJ(_assemblies);
					__FREEOBJ(_modules);
					__FREEOBJ(_types);
					__FREEOBJ(_methods);
					__FREEOBJ(_threads);*/
				}

				HRESULT RuntimeMetadataProvider::Initialize(JavaVM* javaVM)
				{
					if (VM == null)
					{
						VM = javaVM;
						return S_OK;
					}
					return E_FAIL;
				}
			
				//HRESULT RuntimeMetadataProvider::GetAppDomain(AppDomainID appDomainId, AppDomainMetadata** metadata)
				//{
				//	*metadata = _appDomain->Find(appDomainId);
				//	if (*metadata == null)
				//	{
				//		*metadata = new AppDomainMetadata(_corProfilerInfo2, this, appDomainId);
				//		_appDomain->Add(appDomainId, *metadata);
				//	}
				//	return S_OK;
				//}

				//HRESULT RuntimeMetadataProvider::GetAssembly(AssemblyID assemblyId, AssemblyMetadata** metadata)
				//{
				//	*metadata = _assemblies->Find(assemblyId);
				//	if (*metadata == null)
				//	{
				//		*metadata = new AssemblyMetadata(_corProfilerInfo2, this, assemblyId);
				//		_assemblies->Add(assemblyId, *metadata);
				//	}
				//	return S_OK;
				//}

				//HRESULT RuntimeMetadataProvider::GetModule(ModuleID moduleId, ModuleMetadata** metadata)
				//{
				//	*metadata = _modules->Find(moduleId);
				//	if (*metadata == null)
				//	{
				//		*metadata = new ModuleMetadata(_corProfilerInfo2, this, moduleId);
				//		_modules->Add(moduleId, *metadata);
				//	}
				//	return S_OK;
				//}

				//HRESULT RuntimeMetadataProvider::GetType(ClassID classId, TypeMetadata** metadata)
				//{
				//	*metadata = _types->Find(classId);
				//	if (*metadata == null)
				//	{
				//		*metadata = new TypeMetadata(_corProfilerInfo2, this, classId);
				//		_types->Add(classId, *metadata);
				//	}
				//	return S_OK;
				//}

				//HRESULT RuntimeMetadataProvider::GetMethod(FunctionID functionId, MethodMetadata** metadata)
				//{
				//	*metadata = _methods->Find(functionId);
				//	if (*metadata == null)
				//	{
				//		*metadata = new MethodMetadata(_corProfilerInfo2, this, functionId);
				//		_methods->Add(functionId, *metadata);
				//	}
				//	return S_OK;
				//}
				//
				//HRESULT RuntimeMetadataProvider::GetThread(ThreadID threadId, ThreadMetadata** metadata)
				//{
				//	*metadata = _threads->Find(threadId);
				//	if (*metadata == null)
				//	{
				//		*metadata = new ThreadMetadata(_corProfilerInfo2, this, threadId);
				//		_threads->Add(threadId, *metadata);
				//	}
				//	return S_OK;
				//}

				//HRESULT RuntimeMetadataProvider::GetClassFromObject(ObjectID objectId, ClassID* classId)
				//{
				//	if (_corProfilerInfo2 == null)
				//	{
				//		return E_NOINTERFACE;
				//	}
				//	return _corProfilerInfo2->GetClassFromObject(objectId, classId);
				//}

				///*ObjectMetadata* RuntimeMetadataProvider::GetObject(ObjectID objectId)
				//{
				//	return new ObjectMetadata(_corProfilerInfo2, this, objectId);
				//}*/

				//HRESULT RuntimeMetadataProvider::GetCorProfilerInfo2(ICorProfilerInfo2** profilerInfo)
				//{
				//	*profilerInfo = _corProfilerInfo2;
				//	if (*profilerInfo == null)
				//	{
				//		return E_NOINTERFACE;
				//	}
				//	return S_OK;
				//}

				//HRESULT RuntimeMetadataProvider::GetCorProfilerInfo3(ICorProfilerInfo3** profilerInfo)
				//{
				//	*profilerInfo = _corProfilerInfo3;
				//	if (*profilerInfo == null)
				//	{
				//		return E_NOINTERFACE;
				//	}
				//	return S_OK;
				//}

				//HRESULT RuntimeMetadataProvider::SetEventMask(DWORD eventsMask)
				//{
				//	if (_corProfilerInfo2 == null)
				//	{
				//		return E_NOT_VALID_STATE;
				//	}
				//	return _corProfilerInfo2->SetEventMask(eventsMask);
				//}

				//HRESULT RuntimeMetadataProvider::GetCurrentThreadId(ThreadID* threadId)
				//{
				//	if (_corProfilerInfo2 == null)
				//	{
				//		return E_NOT_VALID_STATE;
				//	}
				//	return _corProfilerInfo2->GetCurrentThreadID(threadId);
				//}

				//HRESULT RuntimeMetadataProvider::GetHandleFromThread(ThreadID threadId, HANDLE* threadHandle)
				//{
				//	if (_corProfilerInfo2 == null)
				//	{
				//		return E_NOT_VALID_STATE;
				//	}
				//	return _corProfilerInfo2->GetHandleFromThread(threadId, threadHandle);
				//}

				const __guid RuntimeMetadataProvider::ServiceToken = Converter::ConvertStringToGuid(L"{A461EFD4-E3B4-4EC4-9FA1-0636CB92989E}");

				JavaVM* RuntimeMetadataProvider::VM = null;
			}
		}
	}
}