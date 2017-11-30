#include "stdafx.h"
#include "ProfilerEntryPoint.h"

Chronos::Agent::Java::ProfilerEntryPoint EntryPoint;

extern "C" JNIEXPORT jint JNICALL Agent_OnLoad(JavaVM* vm, char* options, void* reserved) 
{
	return EntryPoint.OnLoad(vm);
}

extern "C" JNIEXPORT jint JNICALL Agent_OnAttach(JavaVM* vm, char* options, void* reserved)
{
	return EntryPoint.OnAttach(vm);
}

extern "C" JNIEXPORT jint JNICALL JNI_OnLoad(JavaVM* vm, void* reserved) 
{
	EntryPoint.OnLoad(vm);
	return JNI_VERSION_1_6;
}

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			ProfilerEntryPoint::ProfilerEntryPoint()
			{
				_application = null;
				_metadataProvider = null;
			}

			ProfilerEntryPoint::~ProfilerEntryPoint()
			{
				__FREEOBJ(_application);
			}

			__int ProfilerEntryPoint::OnLoad(JavaVM* javaVM)
			{
				HRESULT result = InitializeInternal(javaVM);
				if (FAILED(result))
				{
					return 0;
				}
				return 0;
			}

			__int ProfilerEntryPoint::OnAttach(JavaVM* javaVM)
			{
				HRESULT result = InitializeInternal(javaVM);
				if (FAILED(result))
				{
					return 0;
				}
				return 0;
			}

			HRESULT ProfilerEntryPoint::InitializeInternal(JavaVM* javaVM)
			{
				HRESULT result;
				result = Reflection::RuntimeMetadataProvider::Initialize(javaVM);
				__RETURN_IF_FAILED(result);

				_application = new Chronos::Agent::Application();

				result = _application->Run();
				__RETURN_IF_FAILED(result);

				//__RESOLVE_SERVICE(_application->Container, Chronos::Agent::DotNet::RuntimeProfilingEvents, GlobalEvents);
				__RESOLVE_SERVICE(_application->Container, Reflection::RuntimeMetadataProvider, _metadataProvider);

				//__int eventsMask = GlobalEvents->GetProfilingEvents();

				//if ((eventsMask & COR_PRF_MONITOR_ENTERLEAVE) == COR_PRF_MONITOR_ENTERLEAVE)
				//{
				//	result = SetupFunctionCallbacks();
				//	__RETURN_IF_FAILED(result);

				//	GlobalFunctions = new Chronos::Agent::DotNet::FunctionInfoCollection();

				//	_exceptionTracers = new Chronos::Agent::DotNet::ThreadScopeDictionary<Chronos::Agent::DotNet::FunctionExceptionTracer*>();
				//	_exceptionTracers->Initialize(_metadataProvider);

				//}
				//result = _metadataProvider->SetEventMask(eventsMask);
				//__RETURN_IF_FAILED(result);

				return S_OK;
			}
		}
	}
}