#include "stdafx.h"
#include "ProfilerEntryPoint.h"

Chronos::Agent::Java::ProfilerEntryPoint EntryPoint;
Chronos::Agent::Java::RuntimeProfilingEvents* GlobalEvents = null;
Chronos::Agent::Java::Reflection::RuntimeMetadataProvider* GlobalMetadataProvider = null;

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

// METHOD EVENTS =========================================================================================================
void JNICALL MethodEntryGlobal(jvmtiEnv* jvmtiEnv, JNIEnv* jniEnv, jthread thread, jmethodID methodId)
{
	Chronos::Agent::Java::MethodEnterEventArgs eventArgs(methodId, thread);
	GlobalEvents->RaiseMethodEvent(Chronos::Agent::Java::RuntimeProfilingEvents::MethodEnter, &eventArgs);
}

void JNICALL MethodExitGlobal(jvmtiEnv* jvmtiEnv, JNIEnv* jniEnv, jthread thread, jmethodID methodId, jboolean exception, jvalue returnValue)
{
	if (exception == JNI_TRUE)
	{
		Chronos::Agent::Java::MethodExceptionEventArgs eventArgs(methodId, thread);
		GlobalEvents->RaiseMethodEvent(Chronos::Agent::Java::RuntimeProfilingEvents::MethodException, &eventArgs);
	}
	else
	{
		Chronos::Agent::Java::MethodExitEventArgs eventArgs(methodId, thread);
		GlobalEvents->RaiseMethodEvent(Chronos::Agent::Java::RuntimeProfilingEvents::MethodExit, &eventArgs);
	}
}

// THREAD EVENTS =========================================================================================================
void JNICALL ThreadStartGlobal(jvmtiEnv* jvmtiEnv, JNIEnv* jniEnv, jthread thread)
{
	//Force metadata initialization
	Chronos::Agent::Java::Reflection::ThreadMetadata* metadata;
	GlobalMetadataProvider->GetThread(thread, &metadata);
	//Notify event
	Chronos::Agent::Java::ThreadStartEventArgs eventArgs(thread);
	GlobalEvents->RaiseMethodEvent(Chronos::Agent::Java::RuntimeProfilingEvents::ThreadStart, &eventArgs);
}

void JNICALL ThreadEndGlobal(jvmtiEnv* jvmtiEnv, JNIEnv* jniEnv, jthread thread)
{
	//Notify event
	Chronos::Agent::Java::ThreadEndEventArgs eventArgs(thread);
	GlobalEvents->RaiseMethodEvent(Chronos::Agent::Java::RuntimeProfilingEvents::ThreadEnd, &eventArgs);
}

//=======================================================================================================================



namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			ProfilerEntryPoint::ProfilerEntryPoint()
			{
				_application = null;
			}

			ProfilerEntryPoint::~ProfilerEntryPoint()
			{
				__FREEOBJ(_application);
			}

			HRESULT ProfilerEntryPoint::OnLoad(JavaVM* javaVM)
			{
				HRESULT result = InitializeInternal(javaVM);
				if (FAILED(result))
				{
					return JNI_OK;
				}
				return JNI_OK;
			}

			HRESULT ProfilerEntryPoint::OnAttach(JavaVM* javaVM)
			{
				HRESULT result = InitializeInternal(javaVM);
				if (FAILED(result))
				{
					return JNI_OK;
				}
				return JNI_OK;
			}

			HRESULT ProfilerEntryPoint::InitializeInternal(JavaVM* jvm)
			{
				//__debugbreak();
				MessageBox(null, L"Attach", L"", MB_OK);
				__RETURN_IF_FAILED( Reflection::RuntimeMetadataProvider::Initialize(jvm) );

				_application = new Chronos::Agent::Application();

				__RETURN_IF_FAILED( _application->Run() );

				__RESOLVE_SERVICE(_application->Container, Chronos::Agent::Java::RuntimeProfilingEvents, GlobalEvents);
				__RESOLVE_SERVICE( _application->Container, Reflection::RuntimeMetadataProvider, GlobalMetadataProvider);
				
				if (SetupEvents(jvm) != jvmtiError::JVMTI_ERROR_NONE)
				{
					return E_FAIL;
				}


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

			jint ProfilerEntryPoint::SetupEvents(JavaVM* jvm)
			{
				jvmtiEnv* jvmtiEnv;
				jvm->GetEnv((void**)&jvmtiEnv, JVMTI_VERSION_1_0);

				jvmtiCapabilities capabilities;
				__JRETURN_IF_FAILED( jvmtiEnv->GetCapabilities(&capabilities) );

				jvmtiEventCallbacks callbacks;
				memset(&callbacks, 0, sizeof(callbacks));

				// METHOD EVENTS =========================================================================================================
				if (GlobalEvents->HookEvent(RuntimeProfilingEvents::MethodEnter))
				{
					capabilities.can_generate_method_entry_events = 1;
					callbacks.MethodEntry = MethodEntryGlobal;
					__JRETURN_IF_FAILED( jvmtiEnv->SetEventNotificationMode(JVMTI_ENABLE, JVMTI_EVENT_METHOD_ENTRY, NULL) );
				}
				if (GlobalEvents->HookEvent(RuntimeProfilingEvents::MethodExit) || 
					GlobalEvents->HookEvent(RuntimeProfilingEvents::MethodException))
				{
					capabilities.can_generate_method_exit_events = 1;
					callbacks.MethodExit = MethodExitGlobal;
					__JRETURN_IF_FAILED( jvmtiEnv->SetEventNotificationMode(JVMTI_ENABLE, JVMTI_EVENT_METHOD_EXIT, NULL) );
				}

				// THREAD EVENTS =========================================================================================================
				if (GlobalEvents->HookEvent(RuntimeProfilingEvents::ThreadStart))
				{
					callbacks.ThreadStart = ThreadStartGlobal;
					__JRETURN_IF_FAILED( jvmtiEnv->SetEventNotificationMode(JVMTI_ENABLE, JVMTI_EVENT_THREAD_START, NULL) );
				}
				if (GlobalEvents->HookEvent(RuntimeProfilingEvents::ThreadEnd))
				{
					callbacks.ThreadEnd = ThreadEndGlobal;
					__JRETURN_IF_FAILED( jvmtiEnv->SetEventNotificationMode(JVMTI_ENABLE, JVMTI_EVENT_THREAD_END, NULL) );
				}
				//=======================================================================================================================

				__JRETURN_IF_FAILED( jvmtiEnv->AddCapabilities(&capabilities) );
				__JRETURN_IF_FAILED( jvmtiEnv->SetEventCallbacks(&callbacks, sizeof(jvmtiEventCallbacks)) );

				return jvmtiError::JVMTI_ERROR_NONE;
			}

			
		}
	}
}