#include "stdafx.h"
#include "ProfilerEntryPoint.h"

//=================================================================================================
//msbuild $(ProjectPath) /property:configuration=$(Configuration) /property:platform=x64
#ifdef _M_IX86
#include "FunctionCallbacks32Naked.h"
#elif defined(_WIN64)
//EXTERN_C void FunctionEnterNaked(FunctionID functionId);
//EXTERN_C void FunctionLeaveNaked(FunctionID functionId);
//EXTERN_C void FunctionTailcallNaked(FunctionID functionId);
//
//EXTERN_C void FunctionEnter2Naked(FunctionID funcId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
//EXTERN_C void FunctionLeave2Naked(FunctionID funcId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
//EXTERN_C void FunctionTailcall2Naked(FunctionID funcId, UINT_PTR clientData, COR_PRF_FRAME_INFO func);
//
//EXTERN_C void FunctionEnter3Naked(FunctionIDOrClientID functionIDOrClientID);
//EXTERN_C void FunctionLeave3Naked(FunctionIDOrClientID functionIDOrClientID);
//EXTERN_C void FunctionTailcall3Naked(FunctionIDOrClientID functionIDOrClientID);
//
//EXTERN_C void FunctionEnter3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//EXTERN_C void FunctionLeave3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//EXTERN_C void FunctionTailcall3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
#endif

//=================================================================================================

Chronos::Agent::DotNet::RuntimeProfilingEvents* GlobalEvents = null;
Chronos::Agent::DotNet::FunctionInfoCollection* GlobalFunctions = null;

//=================================================================================================
UINT_PTR _stdcall FunctionMapperGlobal(FunctionID functionId, BOOL *hookFunction)
{
	Chronos::Agent::DotNet::FunctionLoadStartedEventArgs eventArgs(functionId, false, null);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionLoadStarted, &eventArgs);
	UINT_PTR clientDataPointer = reinterpret_cast<UINT_PTR>(eventArgs.ClientData);
	if (clientDataPointer == 0)
	{
		clientDataPointer = functionId;
	}
	GlobalFunctions->CreateFunction(functionId, clientDataPointer, eventArgs.HookFunction);
	*hookFunction = (BOOL)eventArgs.HookFunction;
	return clientDataPointer;
}
//=================================================================================================

ProfilerEntryPoint::ProfilerEntryPoint(void)
	: _metadataProvider(null), _application(null), _exceptionTracers(null)
{
}

ProfilerEntryPoint::~ProfilerEntryPoint(void)
{
	__FREEOBJ(_application);
	__FREEOBJ(_exceptionTracers);
}

HRESULT ProfilerEntryPoint::InitializeInternal(IUnknown* corProfilerInfoUnk)
{
	HRESULT result;
	result = Chronos::Agent::DotNet::Reflection::RuntimeMetadataProvider::Initialize(corProfilerInfoUnk);
	__RETURN_IF_FAILED(result);

	_application = new Chronos::Agent::Application();
	
	result = _application->Run();
	__RETURN_IF_FAILED(result);

	__RESOLVE_SERVICE(_application->Container, Chronos::Agent::DotNet::RuntimeProfilingEvents, GlobalEvents);
	__RESOLVE_SERVICE(_application->Container, Chronos::Agent::DotNet::Reflection::RuntimeMetadataProvider, _metadataProvider);

	__int eventsMask = GlobalEvents->GetProfilingEvents();
	
	if ((eventsMask & COR_PRF_MONITOR_ENTERLEAVE) == COR_PRF_MONITOR_ENTERLEAVE)
	{
		result = SetupFunctionCallbacks();
		__RETURN_IF_FAILED(result);
		
		GlobalFunctions = new Chronos::Agent::DotNet::FunctionInfoCollection();

		_exceptionTracers = new Chronos::Agent::DotNet::ThreadScopeDictionary<Chronos::Agent::DotNet::FunctionExceptionTracer*>();
		_exceptionTracers->Initialize(_metadataProvider);

	}
	result = _metadataProvider->SetEventMask(eventsMask);
	__RETURN_IF_FAILED(result);

	return S_OK;
}

HRESULT ProfilerEntryPoint::SetupFunctionCallbacks()
{
	ICorProfilerInfo3* _corProfilerInfo3 = null;
	__RETURN_IF_FAILED( _metadataProvider->GetCorProfilerInfo3(&_corProfilerInfo3) );

#ifdef _M_IX86
	//__RETURN_IF_FAILED( _corProfilerInfo3->SetEnterLeaveFunctionHooks(FunctionEnterNaked, FunctionLeaveNaked, FunctionTailcallNaked) );
	//__RETURN_IF_FAILED( _corProfilerInfo3->SetEnterLeaveFunctionHooks2(FunctionEnter2Naked, FunctionLeave2Naked, FunctionTailcall2Naked) );
	__RETURN_IF_FAILED( _corProfilerInfo3->SetEnterLeaveFunctionHooks3((FunctionEnter3*)FunctionEnter3Naked, (FunctionLeave3*)FunctionLeave3Naked, (FunctionTailcall3*)FunctionTailcall3Naked) );
#elif defined(_WIN64)
	//__RETURN_IF_FAILED( _corProfilerInfo3->SetEnterLeaveFunctionHooks2(FunctionEnter2Naked, FunctionLeave2Naked, FunctionTailcall2Naked) );
#endif

	__RETURN_IF_FAILED( _corProfilerInfo3->SetFunctionIDMapper(FunctionMapperGlobal) );
	return S_OK;
}

// STARTUP EVENTS
STDMETHODIMP ProfilerEntryPoint::Initialize(IUnknown* corProfilerInfoUnk)
{
	//MessageBox(NULL, L"ATTACHED", L"", MB_OK);
	HRESULT result = InitializeInternal(corProfilerInfoUnk);
	if (FAILED(result))
	{
		return S_OK;
	}
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::Shutdown()
{
	HRESULT result = _application->Shutdown();
	__FREEOBJ(_exceptionTracers);
	if (FAILED(result))
	{
		return S_OK;
	}
	return S_OK;
}

// APPLICATION DOMAIN EVENTS
STDMETHODIMP ProfilerEntryPoint::AppDomainCreationStarted(AppDomainID appDomainID)
{
	Chronos::Agent::DotNet::AppDomainCreationStartedEventArgs eventArgs(appDomainID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AppDomainCreationStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::AppDomainCreationFinished(AppDomainID appDomainID, HRESULT status)
{
	Chronos::Agent::DotNet::AppDomainCreationFinishedEventArgs eventArgs(appDomainID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AppDomainCreationFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::AppDomainShutdownStarted(AppDomainID appDomainID)
{
	Chronos::Agent::DotNet::AppDomainShutdownStartedEventArgs eventArgs(appDomainID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AppDomainShutdownStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::AppDomainShutdownFinished(AppDomainID appDomainID, HRESULT status)
{
	Chronos::Agent::DotNet::AppDomainShutdownFinishedEventArgs eventArgs(appDomainID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AppDomainShutdownFinished, &eventArgs);
	return S_OK;
}

// ASSEMBLY EVENTS
STDMETHODIMP ProfilerEntryPoint::AssemblyLoadStarted(AssemblyID assemblyID)
{
	Chronos::Agent::DotNet::AssemblyLoadStartedEventArgs eventArgs(assemblyID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AssemblyLoadStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::AssemblyLoadFinished(AssemblyID assemblyID, HRESULT status)
{
	Chronos::Agent::DotNet::AssemblyLoadFinishedEventArgs eventArgs(assemblyID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AssemblyLoadFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::AssemblyUnloadStarted(AssemblyID assemblyID)
{
	Chronos::Agent::DotNet::AssemblyUnloadStartedEventArgs eventArgs(assemblyID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AssemblyUnloadStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::AssemblyUnloadFinished(AssemblyID assemblyID, HRESULT status)
{
	Chronos::Agent::DotNet::AssemblyUnloadFinishedEventArgs eventArgs(assemblyID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::AssemblyUnloadFinished, &eventArgs);
	return S_OK;
}

// MODULE EVENTS
STDMETHODIMP ProfilerEntryPoint::ModuleLoadStarted(ModuleID moduleID)
{
	Chronos::Agent::DotNet::ModuleLoadStartedEventArgs eventArgs(moduleID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ModuleLoadStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ModuleLoadFinished(ModuleID moduleID, HRESULT status)
{
	Chronos::Agent::DotNet::ModuleLoadFinishedEventArgs eventArgs(moduleID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ModuleLoadFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ModuleUnloadStarted(ModuleID moduleID)
{
	Chronos::Agent::DotNet::ModuleUnloadStartedEventArgs eventArgs(moduleID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ModuleUnloadStarted, &eventArgs);
	return S_OK;
}
	  
STDMETHODIMP ProfilerEntryPoint::ModuleUnloadFinished(ModuleID moduleID, HRESULT status)
{
	Chronos::Agent::DotNet::ModuleUnloadFinishedEventArgs eventArgs(moduleID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ModuleUnloadFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ModuleAttachedToAssembly(ModuleID moduleID, AssemblyID assemblyID)
{
	Chronos::Agent::DotNet::ModuleAttachedToAssemblyEventArgs eventArgs(moduleID, assemblyID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ModuleAttachedToAssembly, &eventArgs);
	return S_OK;
}

// CLASS EVENTS
STDMETHODIMP ProfilerEntryPoint::ClassLoadStarted(ClassID classID)
{
	Chronos::Agent::DotNet::ClassLoadStartedEventArgs eventArgs(classID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ClassLoadStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ClassLoadFinished(ClassID classID, HRESULT status)
{
	Chronos::Agent::DotNet::ClassLoadFinishedEventArgs eventArgs(classID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ClassLoadFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ClassUnloadStarted(ClassID classID)
{
	Chronos::Agent::DotNet::ClassUnloadStartedEventArgs eventArgs(classID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ClassUnloadStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ClassUnloadFinished(ClassID classID, HRESULT status)
{
	Chronos::Agent::DotNet::ClassUnloadFinishedEventArgs eventArgs(classID, status);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ClassUnloadFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::FunctionUnloadStarted(FunctionID functionID)
{
	Chronos::Agent::DotNet::FunctionUnloadStartedEventArgs eventArgs(functionID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionUnloadStarted, &eventArgs);
	return S_OK;
}

// JIT EVENTS
STDMETHODIMP ProfilerEntryPoint::JITCompilationStarted(FunctionID functionID, BOOL fIsSafeToBlock)
{
	Chronos::Agent::DotNet::JITCompilationStartedEventArgs eventArgs(functionID, fIsSafeToBlock);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::JITCompilationStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::JITCompilationFinished(FunctionID functionID, HRESULT status, BOOL fIsSafeToBlock)
{
	Chronos::Agent::DotNet::JITCompilationFinishedEventArgs eventArgs(functionID, status, fIsSafeToBlock);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::JITCompilationFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::JITCachedFunctionSearchStarted(FunctionID functionID, BOOL *pbUseCachedFunction)
{
	Chronos::Agent::DotNet::JITCachedFunctionSearchStartedEventArgs eventArgs(functionID, *pbUseCachedFunction);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::JITCachedFunctionSearchStarted, &eventArgs);
	*pbUseCachedFunction = eventArgs.UseCachedFunction;
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::JITCachedFunctionSearchFinished(FunctionID functionID, COR_PRF_JIT_CACHE result)
{
	Chronos::Agent::DotNet::JITCachedFunctionSearchFinishedEventArgs eventArgs(functionID, result);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::JITCachedFunctionSearchFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::JITFunctionPitched(FunctionID functionID)
{
	Chronos::Agent::DotNet::JITFunctionPitchedEventArgs eventArgs(functionID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::JITFunctionPitched, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::JITInlining(FunctionID callerID, FunctionID calleeID, BOOL *pfShouldInline)
{
	Chronos::Agent::DotNet::JITInliningEventArgs eventArgs(callerID, calleeID, *pfShouldInline);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::JITInlining, &eventArgs);
	*pfShouldInline = eventArgs.ShouldInline;
	return S_OK;
}

/// <summary>
/// The Runtime calls ThreadCreated to notify the code profiler that a thread has been created. The threadId is valid immediately.
/// </summary>
STDMETHODIMP ProfilerEntryPoint::ThreadCreated(ThreadID threadID)
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = new Chronos::Agent::DotNet::FunctionExceptionTracer(GlobalEvents, GlobalFunctions);
		_exceptionTracers->AttachItem(tracer);
	}
	Chronos::Agent::DotNet::ThreadCreatedEventArgs eventArgs(threadID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ThreadCreated, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ThreadDestroyed(ThreadID threadID)
{
	if (_exceptionTracers != null)
	{
		_exceptionTracers->RemoveItem();
	}
	Chronos::Agent::DotNet::ThreadDestroyedEventArgs eventArgs(threadID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ThreadDestroyed, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ThreadAssignedToOSThread(ThreadID managedThreadID, DWORD osThreadID) 
{
	Chronos::Agent::DotNet::ThreadAssignedToOSThreadEventArgs eventArgs(managedThreadID, osThreadID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ThreadAssignedToOSThread, &eventArgs);
	/*Chronos::DotNet::CThreadInfo* threadInfo = _threadManager->GetUnit(managedThreadID);
	threadInfo->OsThreadId = osThreadID;
	_threadManager->UpdateUnit(threadInfo);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ThreadNameChanged(ThreadID threadID, ULONG cchName, WCHAR name[])
{
	__string wrappedName(name);
	Chronos::Agent::DotNet::ThreadNameChangedEventArgs eventArgs(threadID, wrappedName);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ThreadNameChanged, &eventArgs);
	/*Chronos::DotNet::CThreadInfo* threadInfo = _threadManager->GetUnit(threadID);
	threadInfo->Name.assign(name);
	_threadManager->UpdateUnit(threadInfo);*/
	return S_OK;
}

// REMOTING EVENTS (Client-side)
STDMETHODIMP ProfilerEntryPoint::RemotingClientInvocationStarted()
{
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RemotingClientSendingMessage(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RemotingClientReceivingReply(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RemotingClientInvocationFinished()
{
	return S_OK;
}

// REMOTING EVENTS (Server-side)
STDMETHODIMP ProfilerEntryPoint::RemotingServerReceivingMessage(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RemotingServerInvocationStarted()
{
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RemotingServerInvocationReturned()
{
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RemotingServerSendingReply(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

// CONTEXT EVENTS
STDMETHODIMP ProfilerEntryPoint::UnmanagedToManagedTransition(FunctionID functionID, COR_PRF_TRANSITION_REASON reason)
{
	Chronos::Agent::DotNet::UnmanagedToManagedTransitionEventArgs eventArgs(functionID, reason);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::UnmanagedToManagedTransition, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ManagedToUnmanagedTransition(FunctionID functionID, COR_PRF_TRANSITION_REASON reason)
{
	Chronos::Agent::DotNet::ManagedToUnmanagedTransitionEventArgs eventArgs(functionID, reason);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ManagedToUnmanagedTransition, &eventArgs);
	return S_OK;
}

// SUSPENSION EVENTS
STDMETHODIMP ProfilerEntryPoint::RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
{
	Chronos::Agent::DotNet::RuntimeSuspendStartedEventArgs eventArgs(suspendReason);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RuntimeSuspendStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RuntimeSuspendFinished()
{
	Chronos::Agent::DotNet::RuntimeSuspendFinishedEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RuntimeSuspendFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RuntimeSuspendAborted()
{
	Chronos::Agent::DotNet::RuntimeSuspendAbortedEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RuntimeSuspendAborted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RuntimeResumeStarted()
{
	Chronos::Agent::DotNet::RuntimeResumeStartedEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RuntimeResumeStarted, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RuntimeResumeFinished()
{
	Chronos::Agent::DotNet::RuntimeResumeFinishedEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RuntimeResumeFinished, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RuntimeThreadSuspended(ThreadID threadID)
{
	Chronos::Agent::DotNet::RuntimeThreadSuspendedEventArgs eventArgs(threadID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RuntimeThreadSuspended, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RuntimeThreadResumed(ThreadID threadID)
{
	Chronos::Agent::DotNet::RuntimeThreadResumedEventArgs eventArgs(threadID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RuntimeThreadResumed, &eventArgs);
	return S_OK;
}

// GC EVENTS
STDMETHODIMP ProfilerEntryPoint::MovedReferences(ULONG cmovedObjectIDRanges, ObjectID oldObjectIDRangeStart[], ObjectID newObjectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
	/*Chronos::Agent::DotNet::MovedReferencesEventArgs eventArgs(cmovedObjectIDRanges, oldObjectIDRangeStart, newObjectIDRangeStart, cObjectIDRangeLength);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::MovedReferences, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ObjectAllocated(ObjectID objectID, ClassID classID)
{
	/*Chronos::Agent::DotNet::ObjectAllocatedEventArgs eventArgs(objectID, classID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ObjectAllocated, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ObjectsAllocatedByClass(ULONG classCount, ClassID classIDs[], ULONG objects[])
{
	/*Chronos::Agent::DotNet::ObjectsAllocatedByClassEventArgs eventArgs(classCount, classIDs, objects);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ObjectsAllocatedByClass, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ObjectReferences(ObjectID objectID, ClassID classID, ULONG objectRefs, ObjectID objectRefIDs[])
{
	/*Chronos::Agent::DotNet::ObjectReferencesEventArgs eventArgs(objectID, classID, objectRefs, objectRefIDs);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ObjectReferences, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RootReferences(ULONG rootRefs, ObjectID rootRefIDs[])
{
	/*Chronos::Agent::DotNet::RootReferencesEventArgs eventArgs(rootRefs, rootRefIDs);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RootReferences, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::GarbageCollectionStarted(int cGenerations, BOOL generationCollected[], COR_PRF_GC_REASON reason)
{
	/*Chronos::Agent::DotNet::GarbageCollectionStartedEventArgs eventArgs(cGenerations, generationCollected, reason);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::GarbageCollectionStarted, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::SurvivingReferences(ULONG cSurvivingObjectIDRanges, ObjectID objectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
	/*Chronos::Agent::DotNet::SurvivingReferencesEventArgs eventArgs(cSurvivingObjectIDRanges, objectIDRangeStart, cObjectIDRangeLength);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::SurvivingReferences, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::GarbageCollectionFinished()
{
	/*Chronos::Agent::DotNet::GarbageCollectionFinishedEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::GarbageCollectionFinished, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::FinalizeableObjectQueued(DWORD finalizerFlags, ObjectID objectID)
{
	/*Chronos::Agent::DotNet::FinalizeableObjectQueuedEventArgs eventArgs(finalizerFlags, objectID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FinalizeableObjectQueued, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::RootReferences2(ULONG cRootRefs, ObjectID rootRefIDs[], COR_PRF_GC_ROOT_KIND rootKinds[], COR_PRF_GC_ROOT_FLAGS rootFlags[], UINT_PTR rootIDs[])
{
	/*Chronos::Agent::DotNet::RootReferences2EventArgs eventArgs(cRootRefs, rootRefIDs, rootKinds, rootFlags, rootIDs);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::RootReferences2, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::HandleCreated(GCHandleID handleID, ObjectID initialObjectID)
{
	/*Chronos::Agent::DotNet::HandleCreatedEventArgs eventArgs(handleID, initialObjectID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::HandleCreated, &eventArgs);*/
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::HandleDestroyed(GCHandleID handleID)
{
	/*Chronos::Agent::DotNet::HandleDestroyedEventArgs eventArgs(handleID);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::HandleDestroyed, &eventArgs);*/
	return S_OK;
}

// EXCEPTION EVENTS (Exception creation)
STDMETHODIMP ProfilerEntryPoint::ExceptionThrown(ObjectID objectId)
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionThrown(objectId);
		}
	}
	Chronos::Agent::DotNet::ExceptionThrownEventArgs eventArgs(objectId);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionThrown, &eventArgs);
	return S_OK;
}

// EXCEPTION EVENTS (Search phase)
STDMETHODIMP ProfilerEntryPoint::ExceptionSearchFunctionEnter(FunctionID functionId)
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionSearchFunctionEnter(functionId);
		}
	}
	Chronos::Agent::DotNet::ExceptionSearchFunctionEnterEventArgs eventArgs(functionId);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionSearchFunctionEnter, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionSearchFunctionLeave()
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionSearchFunctionLeave();
		}
	}
	Chronos::Agent::DotNet::ExceptionSearchFunctionLeaveEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionSearchFunctionLeave, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionSearchFilterEnter(FunctionID functionId)
{
	Chronos::Agent::DotNet::ExceptionSearchFilterEnterEventArgs eventArgs(functionId);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionSearchFilterEnter, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionSearchFilterLeave()
{
	Chronos::Agent::DotNet::ExceptionSearchFilterLeaveEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionSearchFilterLeave, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionSearchCatcherFound(FunctionID functionId)
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionSearchCatcherFound(functionId);
		}
	}
	Chronos::Agent::DotNet::ExceptionSearchCatcherFoundEventArgs eventArgs(functionId);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionSearchCatcherFound, &eventArgs);
	return S_OK;
}

/// <summary>
/// Called when a catch block for an exception is executed inside the common language runtime (CLR) itself. This method is obsolete in the .NET Framework version 2.0.
/// </summary>
STDMETHODIMP ProfilerEntryPoint::ExceptionCLRCatcherFound()
{
	return S_OK;
}

/// <summary>
/// Called when a catch block for an exception is executed inside the common language runtime (CLR) itself. This method is obsolete in the .NET Framework version 2.0.
/// </summary>
STDMETHODIMP ProfilerEntryPoint::ExceptionCLRCatcherExecute()
{
	return S_OK;
}

/// <summary>
/// Not implemented. A profiler that needs unmanaged exception information must obtain this information through other means.
/// </summary>
STDMETHODIMP ProfilerEntryPoint::ExceptionOSHandlerEnter(FunctionID functionId)
{
	return S_OK;
}

/// <summary>
/// Not implemented. A profiler that needs unmanaged exception information must obtain this information through other means.
/// </summary>
STDMETHODIMP ProfilerEntryPoint::ExceptionOSHandlerLeave(FunctionID functionId)
{
	return S_OK;
}

// EXCEPTION EVENTS (Unwind phase)
STDMETHODIMP ProfilerEntryPoint::ExceptionUnwindFunctionEnter(FunctionID functionId)
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionUnwindFunctionEnter(functionId);
		}
	}
	Chronos::Agent::DotNet::ExceptionUnwindFunctionEnterEventArgs eventArgs(functionId);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionUnwindFunctionEnter, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionUnwindFunctionLeave()
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionUnwindFunctionLeave();
		}
	}
	Chronos::Agent::DotNet::ExceptionUnwindFunctionLeaveEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionUnwindFunctionLeave, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionUnwindFinallyEnter(FunctionID functionId)
{
	Chronos::Agent::DotNet::ExceptionUnwindFinallyEnterEventArgs eventArgs(functionId);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionUnwindFinallyEnter, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionUnwindFinallyLeave()
{
	Chronos::Agent::DotNet::ExceptionUnwindFinallyLeaveEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionUnwindFinallyLeave, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionCatcherEnter(FunctionID functionId, ObjectID objectId)
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionCatcherEnter(functionId, objectId);
		}
	}
	Chronos::Agent::DotNet::ExceptionCatcherEnterEventArgs eventArgs(functionId, objectId);
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionCatcherEnter, &eventArgs);
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::ExceptionCatcherLeave()
{
	if (_exceptionTracers != null)
	{
		Chronos::Agent::DotNet::FunctionExceptionTracer* tracer = _exceptionTracers->CurrentItem;
		if (tracer != null)
		{
			tracer->ExceptionCatcherLeave();
		}
	}
	Chronos::Agent::DotNet::ExceptionCatcherLeaveEventArgs eventArgs;
	GlobalEvents->RaiseEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::ExceptionCatcherLeave, &eventArgs);
	return S_OK;
}

// COM CLASSIC VTable
STDMETHODIMP ProfilerEntryPoint::COMClassicVTableCreated(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable, ULONG cSlots)
{
	return S_OK;
}

STDMETHODIMP ProfilerEntryPoint::COMClassicVTableDestroyed(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable)
{
	return S_OK;
}

// ATTACH EVENTS
STDMETHODIMP ProfilerEntryPoint::InitializeForAttach(IUnknown* pCorProfilerInfoUnk, void* pvClientData, UINT cbClientData)
{
	return S_OK;
	//return _profilerController.Attach(pCorProfilerInfoUnk);
}

STDMETHODIMP ProfilerEntryPoint::ProfilerAttachComplete()
{
	return S_OK;
	//return _profilerController.Attached();
}

STDMETHODIMP ProfilerEntryPoint::ProfilerDetachSucceeded()
{
	return S_OK;
	//return _profilerController.Deattach();
}