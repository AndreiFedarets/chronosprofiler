#pragma once
#include "Chronos.DotNet.TracingProfiler.Agent.h"


namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace TracingProfiler
			{
// ==================================================================================================================================================
				class ProfilingTypeAdapter : public IProfilingTypeAdapter
				{
					public:
						ProfilingTypeAdapter(void);
						~ProfilingTypeAdapter(void);
						HRESULT BeginInitialize(ProfilingTypeSettings* settings);
						HRESULT ExportServices(ServiceContainer* container);
						HRESULT ImportServices(ServiceContainer* container);
						HRESULT EndInitialize();
						HRESULT SubscribeEvents();
						HRESULT FlushData();
						__bool HookFunction(FunctionID functionId);

						static ProfilingTypeAdapter* Current;
						Chronos::Agent::Common::EventsTree::IEventsTreeLoggerCollection* Loggers;
						Chronos::Agent::DotNet::BasicProfiler::FunctionCollection* Functions;

					private:
						void OnFunctionLoadStarted(void* eventArgs);
						void OnThreadCreated(void* eventArgs);
						void OnThreadDestroyed(void* eventArgs);
						void OnManagedToUnmanagedTransition(void* eventArgs);
						void OnUnmanagedToManagedTransition(void* eventArgs);

					private:
						Chronos::Agent::DotNet::BasicProfiler::ThreadCollection* _threads;
						Chronos::Agent::DotNet::Reflection::RuntimeMetadataProvider* _metadataProvider;
						ProfilingTypeSettings* _settings;
						ProfilingEventsSubscription<ProfilingTypeAdapter>* _subscription;
						RuntimeProfilingEvents* _profilingEvents;
						__vector<__string>* _exclusions;

						const static __guid ExclusionsIndex;

				};

// ==================================================================================================================================================
			}
		}
	}
}