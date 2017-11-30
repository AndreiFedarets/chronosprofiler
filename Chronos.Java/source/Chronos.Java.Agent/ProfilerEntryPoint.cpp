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

			HRESULT ProfilerEntryPoint::InitializeInternal(JavaVM* javaVM)
			{
				//__debugbreak();
				MessageBox(null, L"Attach", L"", MB_OK);
				__RETURN_IF_FAILED( Reflection::RuntimeMetadataProvider::Initialize(javaVM) );

				_application = new Chronos::Agent::Application();

				__RETURN_IF_FAILED( _application->Run() );

				//__RESOLVE_SERVICE(_application->Container, Chronos::Agent::DotNet::RuntimeProfilingEvents, GlobalEvents);
				__RESOLVE_SERVICE( _application->Container, Reflection::RuntimeMetadataProvider, _metadataProvider);
				
				jvmtiEnv* javaEnv = null;
				__RETURN_IF_FAILED( javaVM->GetEnv((void**)&javaEnv, JVMTI_VERSION_1_0) );

				jvmtiEventCallbacks callbacks;


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