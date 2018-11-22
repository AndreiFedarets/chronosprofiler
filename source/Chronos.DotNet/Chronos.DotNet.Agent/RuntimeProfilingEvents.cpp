#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			RuntimeProfilingEvents::RuntimeProfilingEvents(void)
			{
				_additionalEvents = 0;
				memset(_events, 0, sizeof(_events));
				memset(_functionEvents, 0, sizeof(_functionEvents));
			}

			RuntimeProfilingEvents::~RuntimeProfilingEvents(void)
			{
			}

			void RuntimeProfilingEvents::SetAdditionalEventsMask(__int eventsMask)
			{
				_additionalEvents = _additionalEvents | eventsMask;
			}

			__int RuntimeProfilingEvents::GetProfilingEvents()
			{
				__int eventsMask = _additionalEvents;

				// APPLICATION DOMAIN EVENTS
				if (HookEvent(RuntimeProfilingEvents::AppDomainCreationStarted) || HookEvent(RuntimeProfilingEvents::AppDomainCreationFinished) || 
					HookEvent(RuntimeProfilingEvents::AppDomainShutdownStarted) || HookEvent(RuntimeProfilingEvents::AppDomainShutdownFinished))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_APPDOMAIN_LOADS;
				}

				// ASSEMBLY EVENTS
				if (HookEvent(RuntimeProfilingEvents::AssemblyLoadStarted) || HookEvent(RuntimeProfilingEvents::AssemblyLoadFinished) || 
					HookEvent(RuntimeProfilingEvents::AssemblyUnloadStarted) || HookEvent(RuntimeProfilingEvents::AssemblyUnloadFinished))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_ASSEMBLY_LOADS;
				}

				// MODULE EVENTS
				if (HookEvent(RuntimeProfilingEvents::ModuleLoadStarted) || HookEvent(RuntimeProfilingEvents::ModuleLoadFinished) || 
					HookEvent(RuntimeProfilingEvents::ModuleUnloadStarted) || HookEvent(RuntimeProfilingEvents::ModuleUnloadFinished) ||
					HookEvent(RuntimeProfilingEvents::ModuleAttachedToAssembly))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_MODULE_LOADS;
				}

				// CLASS EVENTS
				if (HookEvent(RuntimeProfilingEvents::ClassLoadStarted) || HookEvent(RuntimeProfilingEvents::ClassLoadFinished) || 
					HookEvent(RuntimeProfilingEvents::ClassUnloadStarted) || HookEvent(RuntimeProfilingEvents::ClassUnloadFinished))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_CLASS_LOADS;
				}

				// THREAD EVENTS
				if (HookEvent(RuntimeProfilingEvents::ThreadCreated) || HookEvent(RuntimeProfilingEvents::ThreadDestroyed) || 
					HookEvent(RuntimeProfilingEvents::ThreadNameChanged) || HookEvent(RuntimeProfilingEvents::ThreadAssignedToOSThread))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_THREADS;
				}

				// FUNCTION EVENTS
				if (HookEvent(RuntimeProfilingEvents::FunctionEnter) || HookEvent(RuntimeProfilingEvents::FunctionLeave) || 
					HookEvent(RuntimeProfilingEvents::FunctionTailcall) || HookEvent(RuntimeProfilingEvents::FunctionException) || 
					HookEvent(RuntimeProfilingEvents::FunctionLoadStarted))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_ENTERLEAVE;
					//we also should handle exceptions to monitor finction leaves on exception processing
					eventsMask = eventsMask | COR_PRF_MONITOR_EXCEPTIONS;
				}
				if (HookEvent(RuntimeProfilingEvents::FunctionUnloadStarted))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_FUNCTION_UNLOADS;
				}

				// EXCEPTION EVENTS
				if (HookEvent(RuntimeProfilingEvents::ExceptionThrown) || 
					HookEvent(RuntimeProfilingEvents::ExceptionSearchFunctionEnter) || HookEvent(RuntimeProfilingEvents::ExceptionSearchFunctionLeave) || 
					HookEvent(RuntimeProfilingEvents::ExceptionSearchFilterEnter) || HookEvent(RuntimeProfilingEvents::ExceptionSearchFilterLeave) || 
					HookEvent(RuntimeProfilingEvents::ExceptionSearchCatcherFound) ||
					HookEvent(RuntimeProfilingEvents::ExceptionUnwindFunctionEnter) || HookEvent(RuntimeProfilingEvents::ExceptionUnwindFunctionLeave) ||
					HookEvent(RuntimeProfilingEvents::ExceptionUnwindFinallyEnter) || HookEvent(RuntimeProfilingEvents::ExceptionUnwindFinallyLeave) ||
					HookEvent(RuntimeProfilingEvents::ExceptionCatcherEnter) || HookEvent(RuntimeProfilingEvents::ExceptionCatcherLeave))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_EXCEPTIONS;
				}
				
				// JIT EVENTS
				if (HookEvent(RuntimeProfilingEvents::JITCompilationStarted) || HookEvent(RuntimeProfilingEvents::JITCompilationFinished) ||
					HookEvent(RuntimeProfilingEvents::JITFunctionPitched) ||
					HookEvent(RuntimeProfilingEvents::JITInlining))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_JIT_COMPILATION | COR_PRF_MONITOR_CACHE_SEARCHES;
				}
				if (HookEvent(RuntimeProfilingEvents::JITCachedFunctionSearchStarted) || HookEvent(RuntimeProfilingEvents::JITCachedFunctionSearchFinished))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_CACHE_SEARCHES;
				}
				

				// CONTEXT EVENTS
				if (HookEvent(RuntimeProfilingEvents::ManagedToUnmanagedTransition) || 
					HookEvent(RuntimeProfilingEvents::UnmanagedToManagedTransition))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_CODE_TRANSITIONS;
				}
				
				// SUSPENSION EVENTS
				if (HookEvent(RuntimeProfilingEvents::RuntimeSuspendStarted) || HookEvent(RuntimeProfilingEvents::RuntimeSuspendFinished) ||
					HookEvent(RuntimeProfilingEvents::RuntimeSuspendAborted) ||
					HookEvent(RuntimeProfilingEvents::RuntimeResumeStarted) || HookEvent(RuntimeProfilingEvents::RuntimeResumeFinished) ||
					HookEvent(RuntimeProfilingEvents::RuntimeThreadSuspended) || HookEvent(RuntimeProfilingEvents::RuntimeThreadResumed))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_SUSPENDS;
				}
				
				// GC EVENTS
				if (HookEvent(RuntimeProfilingEvents::GarbageCollectionStarted) || HookEvent(RuntimeProfilingEvents::GarbageCollectionFinished) ||
					HookEvent(RuntimeProfilingEvents::MovedReferences) ||
					HookEvent(RuntimeProfilingEvents::SurvivingReferences) ||
					HookEvent(RuntimeProfilingEvents::ObjectReferences) ||
					HookEvent(RuntimeProfilingEvents::ObjectsAllocatedByClass) ||
					HookEvent(RuntimeProfilingEvents::RootReferences) || HookEvent(RuntimeProfilingEvents::RootReferences2) ||
					HookEvent(RuntimeProfilingEvents::HandleCreated) ||	HookEvent(RuntimeProfilingEvents::HandleDestroyed) ||
					HookEvent(RuntimeProfilingEvents::FinalizeableObjectQueued))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_GC;
				}
				if (HookEvent(RuntimeProfilingEvents::ObjectAllocated))
				{
					eventsMask = eventsMask | COR_PRF_MONITOR_OBJECT_ALLOCATED;
				}

				return eventsMask;
			}

			ICallback* RuntimeProfilingEvents::SubscribeEvent(__uint eventId, ICallback* callback)
			{
				if (IsFunctionEvent(eventId))
				{
					__ASSERT(false, L"RuntimeProfilingEvents::SubscribeEvent: attempt to subscribe function event in unusual way");
				}
				ICallback* temp = _events[eventId];
				_events[eventId] = callback;
				return temp;
			}
			
			FunctionEventCallback RuntimeProfilingEvents::SubscribeFunctionEvent(__uint eventId, FunctionEventCallback callback)
			{
				if (!IsFunctionEvent(eventId))
				{
					__ASSERT(false, L"RuntimeProfilingEvents::SubscribeFunctionEvent: attempt to subscribe non-function event in unusual way");
				}
				FunctionEventCallback temp = _functionEvents[eventId];
				_functionEvents[eventId] = callback;
				return temp;
			}

			void RuntimeProfilingEvents::RaiseEvent(__uint eventId, void* eventArgs)
			{
				ICallback* callback = _events[eventId];
				if (callback != null)
				{
					callback->Call(eventArgs);
				}
			}
			
			void RuntimeProfilingEvents::RaiseFunctionEvent(__uint eventId, void* eventArgs)
			{
				FunctionEventCallback callback = _functionEvents[eventId];
				if (callback != null)
				{
					callback(eventArgs);
				}
			}

			__bool RuntimeProfilingEvents::HookEvent(__uint eventId)
			{
				if (IsFunctionEvent(eventId))
				{
					return _functionEvents[eventId] != null;
				}
				return _events[eventId] != null;
			}

			__bool RuntimeProfilingEvents::IsFunctionEvent(__uint eventId)
			{
				return eventId == RuntimeProfilingEvents::FunctionEnter || 
					eventId == RuntimeProfilingEvents::FunctionLeave ||
					eventId == RuntimeProfilingEvents::FunctionTailcall ||
					eventId == RuntimeProfilingEvents::FunctionException;
			}
			
			/*FunctionEventCallback SusbcribeFunctionEnterEvent(FunctionEventCallback callback);
			__bool HookFunctionEnterEvent();
			void RaiseFunctionEnterEvent(FunctionEnterEventArgs* eventArgs);
					
			FunctionEventCallback SusbcribeFunctionLeaveEvent(FunctionEventCallback callback);
			__bool HookFunctionLeaveEvent();
			void RaiseFunctionLeaveEvent(FunctionLeaveEventArgs* eventArgs);
					
			FunctionEventCallback SusbcribeFunctionTailcallEvent(FunctionEventCallback callback);
			__bool HookFunctionTailcallEvent();
			void RaiseFunctionTailcallEvent(FunctionTailcallEventArgs* eventArgs);
					
			FunctionEventCallback SusbcribeFunctionExceptionEvent(FunctionEventCallback callback);
			__bool HookFunctionExceptionEvent();
			void RaiseFunctionExceptionEvent(FunctionExceptionEventArgs* eventArgs);*/

			// ==========================================================================================================================
			const __guid RuntimeProfilingEvents::ServiceToken = Converter::ConvertStringToGuid(RuntimeProfilingEventsServiceToken);
		}
	}
}