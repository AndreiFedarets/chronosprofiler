#include "stdafx.h"
#include "Chronos.DotNet.TracingProfiler.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace TracingProfiler
			{
				// HOOKS ==============================================================================================================
				void ProfilingTypeAdapter::OnFunctionLoadStarted(void* eventArgs)
				{
					FunctionLoadStartedEventArgs* temp = static_cast<FunctionLoadStartedEventArgs*>(eventArgs);
					temp->HookFunction = true;
				}

				void Chronos_DotNet_TracingProfiler_OnFunctionEnter(void* eventArgs)
				{
					FunctionEnterEventArgs* temp = static_cast<FunctionEnterEventArgs*>(eventArgs);
					Chronos::Agent::DotNet::BasicProfiler::FunctionInfo* functionInfo = (Chronos::Agent::DotNet::BasicProfiler::FunctionInfo*)temp->ClientData;
					Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventEnter(EventType::ManagedFunctionCall, functionInfo->Uid);
					/*Chronos::Agent::Common::EventsTree::IEventsTreeLoggerCollection* loggers = Chronos::Agent::DotNet::TracingProfiler::ProfilingTypeAdapter::Current->Loggers;
					loggers->LogEventEnter(EventType::FunctionCall, functionInfo->Uid);*/
				}

				void Chronos_DotNet_TracingProfiler_OnFunctionLeave(void* eventArgs)
				{
					FunctionLeaveEventArgs* temp = static_cast<FunctionLeaveEventArgs*>(eventArgs);
					Chronos::Agent::DotNet::BasicProfiler::FunctionInfo* functionInfo = (Chronos::Agent::DotNet::BasicProfiler::FunctionInfo*)temp->ClientData;
					Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventLeave(EventType::ManagedFunctionCall, functionInfo->Uid);
					/*Chronos::Agent::Common::EventsTree::IEventsTreeLoggerCollection* loggers = Chronos::Agent::DotNet::TracingProfiler::ProfilingTypeAdapter::Current->Loggers;
					loggers->LogEventLeave(EventType::FunctionCall, functionInfo->Uid);*/
				}

				void Chronos_DotNet_TracingProfiler_OnFunctionTailcall(void* eventArgs)
				{
					FunctionTailcallEventArgs* temp = static_cast<FunctionTailcallEventArgs*>(eventArgs);
					Chronos::Agent::DotNet::BasicProfiler::FunctionInfo* functionInfo = (Chronos::Agent::DotNet::BasicProfiler::FunctionInfo*)temp->ClientData;
					Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventLeave(EventType::ManagedFunctionCall, functionInfo->Uid);
					/*Chronos::Agent::Common::EventsTree::IEventsTreeLoggerCollection* loggers = Chronos::Agent::DotNet::TracingProfiler::ProfilingTypeAdapter::Current->Loggers;
					loggers->LogEventLeave(EventType::FunctionCall, functionInfo->Uid);*/
				}

				void Chronos_DotNet_TracingProfiler_OnFunctionException(void* eventArgs)
				{
					FunctionExceptionEventArgs* temp = static_cast<FunctionExceptionEventArgs*>(eventArgs);
					Chronos::Agent::DotNet::BasicProfiler::FunctionInfo* functionInfo = (Chronos::Agent::DotNet::BasicProfiler::FunctionInfo*)temp->ClientData;
					Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventLeave(EventType::ManagedFunctionCall, functionInfo->Uid);
					/*Chronos::Agent::Common::EventsTree::IEventsTreeLoggerCollection* loggers = Chronos::Agent::DotNet::TracingProfiler::ProfilingTypeAdapter::Current->Loggers;
					loggers->LogEventLeave(EventType::FunctionCall, functionInfo->Uid);*/
				}

				void ProfilingTypeAdapter::OnThreadCreated(void* eventArgs)
				{
					//ThreadCreatedEventArgs* temp = static_cast<ThreadCreatedEventArgs*>(eventArgs);
					//Chronos::Agent::DotNet::BasicProfiler::ThreadInfo* threadInfo = _threads->GetUnit(temp->ThreadId);
					//Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventEnterLeave(EventType::ThreadCreate, threadInfo->Uid);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ThreadCreated, eventArgs);
				}

				void ProfilingTypeAdapter::OnThreadDestroyed(void* eventArgs)
				{
					/*ThreadCreatedEventArgs* temp = static_cast<ThreadCreatedEventArgs*>(eventArgs);
					Chronos::Agent::DotNet::BasicProfiler::ThreadInfo* threadInfo = _threads->GetUnit(temp->ThreadId, false);
					Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventEnterLeave(EventType::ThreadDestroy, threadInfo->Uid);*/
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ThreadDestroyed, eventArgs);
				}

				void ProfilingTypeAdapter::OnManagedToUnmanagedTransition(void* eventArgs)
				{
					ManagedToUnmanagedTransitionEventArgs* temp = static_cast<ManagedToUnmanagedTransitionEventArgs*>(eventArgs);
					Chronos::Agent::DotNet::BasicProfiler::FunctionInfo* functionInfo = Functions->GetUnit(temp->FunctionId);
					__uint functionId = 0;
					switch (temp->Reason)
					{
						case COR_PRF_TRANSITION_REASON::COR_PRF_TRANSITION_CALL:
							//Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventEnter(EventType::ManagedToUnmanagedTransition, functionId);
							break;
						case COR_PRF_TRANSITION_REASON::COR_PRF_TRANSITION_RETURN:
							//Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventLeave(EventType::ManagedToUnmanagedTransition, functionId);
							break;
					}
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ManagedToUnmanagedTransition, eventArgs);
				}

				void ProfilingTypeAdapter::OnUnmanagedToManagedTransition(void* eventArgs)
				{
					UnmanagedToManagedTransitionEventArgs* temp = static_cast<UnmanagedToManagedTransitionEventArgs*>(eventArgs);
					Chronos::Agent::DotNet::BasicProfiler::FunctionInfo* functionInfo = Functions->GetUnit(temp->FunctionId);
					__uint functionId = 0;
					switch (temp->Reason)
					{
						case COR_PRF_TRANSITION_REASON::COR_PRF_TRANSITION_CALL:
							//Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventEnter(EventType::UnmanagedToManagedTransition, functionId);
							break;
						case COR_PRF_TRANSITION_REASON::COR_PRF_TRANSITION_RETURN:
							//Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection_LogEventLeave(EventType::UnmanagedToManagedTransition, functionId);
							break;
					}
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::UnmanagedToManagedTransition, eventArgs);
				}

				// CProfilerExtension =================================================================================================
				ProfilingTypeAdapter::ProfilingTypeAdapter(void)
				{
					Current = this;
					_profilingEvents = null;
				}

				ProfilingTypeAdapter::~ProfilingTypeAdapter(void)
				{

				}

				HRESULT ProfilingTypeAdapter::BeginInitialize(ProfilingTypeSettings* settings)
				{
					//Initialize local settings
					_settings = settings;
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ExportServices(ServiceContainer* container)
				{
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ImportServices(ServiceContainer* container)
				{
					//Query shared services
					__RESOLVE_SERVICE(container, RuntimeProfilingEvents, _profilingEvents);
					__RESOLVE_SERVICE(container, Reflection::RuntimeMetadataProvider, _metadataProvider);
					__RESOLVE_SERVICE(container, Chronos::Agent::Common::EventsTree::IEventsTreeLoggerCollection, Loggers);
					__RESOLVE_SERVICE(container, Chronos::Agent::DotNet::BasicProfiler::ThreadCollection, _threads);
					__RESOLVE_SERVICE(container, Chronos::Agent::DotNet::BasicProfiler::FunctionCollection, Functions);
					_subscription = new ProfilingEventsSubscription<ProfilingTypeAdapter>(this, _profilingEvents);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::EndInitialize()
				{
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::SubscribeEvents()
				{
					//Subscribe function events
					_subscription->SubscribeEvent(RuntimeProfilingEvents::FunctionLoadStarted, &ProfilingTypeAdapter::OnFunctionLoadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ThreadCreated, &ProfilingTypeAdapter::OnThreadCreated);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ThreadDestroyed, &ProfilingTypeAdapter::OnThreadDestroyed);
					//_subscription->SubscribeEvent(RuntimeProfilingEvents::ManagedToUnmanagedTransition, &ProfilingTypeAdapter::OnManagedToUnmanagedTransition);
					//_subscription->SubscribeEvent(RuntimeProfilingEvents::UnmanagedToManagedTransition, &ProfilingTypeAdapter::OnUnmanagedToManagedTransition);

					_profilingEvents->SubscribeFunctionEvent(RuntimeProfilingEvents::FunctionEnter, Chronos_DotNet_TracingProfiler_OnFunctionEnter);
					_profilingEvents->SubscribeFunctionEvent(RuntimeProfilingEvents::FunctionLeave, Chronos_DotNet_TracingProfiler_OnFunctionLeave);
					_profilingEvents->SubscribeFunctionEvent(RuntimeProfilingEvents::FunctionTailcall, Chronos_DotNet_TracingProfiler_OnFunctionTailcall);
					_profilingEvents->SubscribeFunctionEvent(RuntimeProfilingEvents::FunctionException, Chronos_DotNet_TracingProfiler_OnFunctionException);
					return S_OK;
				}
			
				HRESULT ProfilingTypeAdapter::FlushData()
				{
					return S_OK;
				}
				
				const ProfilingTypeAdapter* ProfilingTypeAdapter::Current = null;
			}
		}
	}
}