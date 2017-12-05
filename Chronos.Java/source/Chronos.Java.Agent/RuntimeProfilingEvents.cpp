#include "stdafx.h"
#include "Chronos.Java.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			RuntimeProfilingEvents::RuntimeProfilingEvents(void)
			{
				memset(_events, 0, sizeof(_events));
				memset(_methodEvents, 0, sizeof(_methodEvents));
			}

			RuntimeProfilingEvents::~RuntimeProfilingEvents(void)
			{
			}

			//	__int eventsMask = _additionalEvents;

			//	// APPLICATION DOMAIN EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::AppDomainCreationStarted) || HookEvent(RuntimeProfilingEvents::AppDomainCreationFinished) || 
			//		HookEvent(RuntimeProfilingEvents::AppDomainShutdownStarted) || HookEvent(RuntimeProfilingEvents::AppDomainShutdownFinished))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_APPDOMAIN_LOADS;
			//	}

			//	// ASSEMBLY EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::AssemblyLoadStarted) || HookEvent(RuntimeProfilingEvents::AssemblyLoadFinished) || 
			//		HookEvent(RuntimeProfilingEvents::AssemblyUnloadStarted) || HookEvent(RuntimeProfilingEvents::AssemblyUnloadFinished))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_ASSEMBLY_LOADS;
			//	}

			//	// MODULE EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::ModuleLoadStarted) || HookEvent(RuntimeProfilingEvents::ModuleLoadFinished) || 
			//		HookEvent(RuntimeProfilingEvents::ModuleUnloadStarted) || HookEvent(RuntimeProfilingEvents::ModuleUnloadFinished) ||
			//		HookEvent(RuntimeProfilingEvents::ModuleAttachedToAssembly))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_MODULE_LOADS;
			//	}

			//	// CLASS EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::ClassLoadStarted) || HookEvent(RuntimeProfilingEvents::ClassLoadFinished) || 
			//		HookEvent(RuntimeProfilingEvents::ClassUnloadStarted) || HookEvent(RuntimeProfilingEvents::ClassUnloadFinished))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_CLASS_LOADS;
			//	}

			//	// THREAD EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::ThreadCreated) || HookEvent(RuntimeProfilingEvents::ThreadDestroyed) || 
			//		HookEvent(RuntimeProfilingEvents::ThreadNameChanged) || HookEvent(RuntimeProfilingEvents::ThreadAssignedToOSThread))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_THREADS;
			//	}

			//	// FUNCTION EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::FunctionEnter) || HookEvent(RuntimeProfilingEvents::FunctionLeave) || 
			//		HookEvent(RuntimeProfilingEvents::FunctionTailcall) || HookEvent(RuntimeProfilingEvents::FunctionException) || 
			//		HookEvent(RuntimeProfilingEvents::FunctionLoadStarted))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_ENTERLEAVE;
			//		//we also should handle exceptions to monitor finction leaves on exception processing
			//		eventsMask = eventsMask | COR_PRF_MONITOR_EXCEPTIONS;
			//	}
			//	if (HookEvent(RuntimeProfilingEvents::FunctionUnloadStarted))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_FUNCTION_UNLOADS;
			//	}

			//	// EXCEPTION EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::ExceptionThrown) || 
			//		HookEvent(RuntimeProfilingEvents::ExceptionSearchFunctionEnter) || HookEvent(RuntimeProfilingEvents::ExceptionSearchFunctionLeave) || 
			//		HookEvent(RuntimeProfilingEvents::ExceptionSearchFilterEnter) || HookEvent(RuntimeProfilingEvents::ExceptionSearchFilterLeave) || 
			//		HookEvent(RuntimeProfilingEvents::ExceptionSearchCatcherFound) ||
			//		HookEvent(RuntimeProfilingEvents::ExceptionUnwindFunctionEnter) || HookEvent(RuntimeProfilingEvents::ExceptionUnwindFunctionLeave) ||
			//		HookEvent(RuntimeProfilingEvents::ExceptionUnwindFinallyEnter) || HookEvent(RuntimeProfilingEvents::ExceptionUnwindFinallyLeave) ||
			//		HookEvent(RuntimeProfilingEvents::ExceptionCatcherEnter) || HookEvent(RuntimeProfilingEvents::ExceptionCatcherLeave))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_EXCEPTIONS;
			//	}
			//	
			//	// JIT EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::JITCompilationStarted) || HookEvent(RuntimeProfilingEvents::JITCompilationFinished) ||
			//		HookEvent(RuntimeProfilingEvents::JITFunctionPitched) ||
			//		HookEvent(RuntimeProfilingEvents::JITInlining))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_JIT_COMPILATION;
			//	}
			//	if (HookEvent(RuntimeProfilingEvents::JITCachedFunctionSearchStarted) || HookEvent(RuntimeProfilingEvents::JITCachedFunctionSearchFinished))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_CACHE_SEARCHES;
			//	}
			//	

			//	// CONTEXT EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::ManagedToUnmanagedTransition) || 
			//		HookEvent(RuntimeProfilingEvents::UnmanagedToManagedTransition))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_CODE_TRANSITIONS;
			//	}
			//	
			//	// SUSPENSION EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::RuntimeSuspendStarted) || HookEvent(RuntimeProfilingEvents::RuntimeSuspendFinished) ||
			//		HookEvent(RuntimeProfilingEvents::RuntimeSuspendAborted) ||
			//		HookEvent(RuntimeProfilingEvents::RuntimeResumeStarted) || HookEvent(RuntimeProfilingEvents::RuntimeResumeFinished) ||
			//		HookEvent(RuntimeProfilingEvents::RuntimeThreadSuspended) || HookEvent(RuntimeProfilingEvents::RuntimeThreadResumed))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_SUSPENDS;
			//	}
			//	
			//	// GC EVENTS
			//	if (HookEvent(RuntimeProfilingEvents::GarbageCollectionStarted) || HookEvent(RuntimeProfilingEvents::GarbageCollectionFinished) ||
			//		HookEvent(RuntimeProfilingEvents::MovedReferences) ||
			//		HookEvent(RuntimeProfilingEvents::SurvivingReferences) ||
			//		HookEvent(RuntimeProfilingEvents::ObjectReferences) ||
			//		HookEvent(RuntimeProfilingEvents::ObjectsAllocatedByClass) ||
			//		HookEvent(RuntimeProfilingEvents::RootReferences) || HookEvent(RuntimeProfilingEvents::RootReferences2) ||
			//		HookEvent(RuntimeProfilingEvents::HandleCreated) ||	HookEvent(RuntimeProfilingEvents::HandleDestroyed) ||
			//		HookEvent(RuntimeProfilingEvents::FinalizeableObjectQueued))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_GC;
			//	}
			//	if (HookEvent(RuntimeProfilingEvents::ObjectAllocated))
			//	{
			//		eventsMask = eventsMask | COR_PRF_MONITOR_OBJECT_ALLOCATED;
			//	}

			//	return eventsMask;

			ICallback* RuntimeProfilingEvents::SubscribeEvent(__uint eventId, ICallback* callback)
			{
				if (IsMethodEvent(eventId))
				{
					__ASSERT(false, L"RuntimeProfilingEvents::SubscribeEvent: attempt to subscribe function event in unusual way");
				}
				ICallback* temp = _events[eventId];
				_events[eventId] = callback;
				return temp;
			}
			
			MethodEventCallback RuntimeProfilingEvents::SubscribeMethodEvent(__uint eventId, MethodEventCallback callback)
			{
				if (!IsMethodEvent(eventId))
				{
					__ASSERT(false, L"RuntimeProfilingEvents::SubscribeFunctionEvent: attempt to subscribe non-function event in unusual way");
				}
				MethodEventCallback temp = _methodEvents[eventId];
				_methodEvents[eventId] = callback;
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
			
			void RuntimeProfilingEvents::RaiseMethodEvent(__uint eventId, void* eventArgs)
			{
				MethodEventCallback callback = _methodEvents[eventId];
				if (callback != null)
				{
					callback(eventArgs);
				}
			}

			__bool RuntimeProfilingEvents::HookEvent(__uint eventId)
			{
				if (IsMethodEvent(eventId))
				{
					return _methodEvents[eventId] != null;
				}
				return _events[eventId] != null;
			}

			__bool RuntimeProfilingEvents::IsMethodEvent(__uint eventId)
			{
				return eventId == RuntimeProfilingEvents::MethodEnter || 
					   eventId == RuntimeProfilingEvents::MethodExit ||
					   eventId == RuntimeProfilingEvents::MethodException;
					/*eventId == RuntimeProfilingEvents::FunctionTailcall ||
					eventId == RuntimeProfilingEvents::FunctionException;*/
			}
			
			// ==========================================================================================================================
			const __guid RuntimeProfilingEvents::ServiceToken = Converter::ConvertStringToGuid(L"{BF34CD43-7F61-4108-AF9F-8E23084E0B56}");
		}
	}
}