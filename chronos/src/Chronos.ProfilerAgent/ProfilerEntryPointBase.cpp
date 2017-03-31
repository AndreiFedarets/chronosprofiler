#include "ProfilerEntryPointBase.h"

LONG WINAPI UnhandledExceptionHandler(EXCEPTION_POINTERS* exceptionInfo)
{
	//GenerateDump(CCurrentProcess::GetProcessId(), L"C:\\dumps\\crash.dmp", exceptionInfo);
	CProfilerEntryPointBase::ProfilerCallbacksGlobal->Shutdown();
	return EXCEPTION_CONTINUE_SEARCH; //: EXCEPTION_EXECUTE_HANDLER;
}

void CProfilerEntryPointBase::SetInstance(CProfilerEntryPointBase* instance)
{
	ProfilerCallbacksGlobal = instance;
}

CProfilerEntryPointBase::CProfilerEntryPointBase(void)
{
	CProfilerEntryPointBase::SetInstance(this);
	_hits = 0;
}

CProfilerEntryPointBase::~CProfilerEntryPointBase(void)
{
	Dispose();
}

int GlobalExceptionFilter(unsigned int code, struct _EXCEPTION_POINTERS *ep, wchar_t* source)
{
	_ASSERT(false);
	//MessageBox(NULL, CConvert::ToString(code).c_str(), source, MB_OK);
	return 0;
}

UINT_PTR _stdcall CProfilerEntryPointBase::FunctionMapper(FunctionID functionID, BOOL *hookFunction)
{
	return CProfilerEntryPointBase::ProfilerCallbacksGlobal->FunctionMap(functionID, hookFunction);
}

// STARTUP/SHUTDOWN EVENTS
STDMETHODIMP CProfilerEntryPointBase::Initialize(IUnknown *corProfilerInfoUnk)
{
	CTimer::Initialize();
	_disposed = false;
	_profilerController = new CProfilerController();
	///Load and initialize client
	HRESULT result = _profilerController->Initialize();
	if (FAILED(result))
	{
		return S_OK;
	}
	
	///Setup unhandler exception filter to force logs saving
	SetUnhandledExceptionFilter(UnhandledExceptionHandler);

	/////Query ICorProfilerInfo2 interface
	result = corProfilerInfoUnk->QueryInterface(IID_ICustomCorProfilerInfo, (void**)&_corProfilerInfo2);
	if (FAILED(result))
	{
		return S_OK; 
	}
    CManagedProvider::Initialize(_corProfilerInfo2);
	/////Initialize managers
	_processShadow = new CProcessShadow(_corProfilerInfo2, _profilerController);

	/////Initialize requests server
	_requestsServer = new CRequestsServer(_profilerController, _processShadow);
	result = _requestsServer->Initialize();
	if (FAILED(result))
	{
		return S_OK;
	}

	/////Initialize pool of loggers
	_threadLoggerPool = new CThreadLoggerPool(_corProfilerInfo2, _profilerController, _processShadow->FunctionManager);
#ifdef FUNCTION_EVENT_DIRECT_CALL
	ThreadLoggerPool = _threadLoggerPool;
#endif
	///////Assing callbacks for Function Enter/Leave/Tailcall/Map (works with x86 / x64)
	if (_profilerController->LogFunctionCalls)
	{
		result = InitializeFunctionCallbacks(_profilerController->UseFastHooks, _corProfilerInfo2);
		if (FAILED(result))
		{
			return S_OK;
		}
	}
	result = _corProfilerInfo2->SetFunctionIDMapper(FunctionMapper);
	/////Apply events mask
	DWORD eventMask = _profilerController->TargetEvents;
	result = _corProfilerInfo2->SetEventMask(eventMask); 
	if (FAILED(result))
	{
		return S_OK; 
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::Shutdown()
{
	//MessageBox(null, CConvert::ToString(_hits).c_str(), null, MB_OK);
	Dispose();
	return S_OK;
}

CProfilerEntryPointBase* CProfilerEntryPointBase::ProfilerCallbacksGlobal;
#ifdef FUNCTION_EVENT_DIRECT_CALL
CThreadLoggerPool* CProfilerEntryPointBase::ThreadLoggerPool;
#endif


// APPLICATION DOMAIN EVENTS
STDMETHODIMP CProfilerEntryPointBase::AppDomainCreationStarted(AppDomainID appDomainID)
{
	CAppDomainManager* appDomainManager = _processShadow->AppDomainManager;
	CAppDomainInfo* appDomainInfo = appDomainManager->Create(appDomainID);
	if (_profilerController->LogAppDomains)
	{
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		appDomainInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::AppDomainCreation, appDomainInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::AppDomainCreationFinished(AppDomainID appDomainID, HRESULT status)
{
	CAppDomainManager* appDomainManager = _processShadow->AppDomainManager;
	CAppDomainInfo* appDomainInfo = appDomainManager->Get(appDomainID);
	appDomainManager->Initialize(appDomainInfo);
	appDomainInfo->LoadResult = status;
	appDomainManager->Update(appDomainInfo);
	if (_profilerController->LogAppDomains)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(appDomainInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::AppDomainCreation, appDomainInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::AppDomainShutdownStarted(AppDomainID appDomainID)
{
	if (_profilerController->LogAppDomains)
	{
		CAppDomainManager* appDomainManager = _processShadow->AppDomainManager;
		CAppDomainInfo* appDomainInfo = appDomainManager->Get(appDomainID);
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		appDomainInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::AppDomainShutdown, appDomainInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::AppDomainShutdownFinished(AppDomainID appDomainID, HRESULT status)
{
	CAppDomainManager* appDomainManager = _processShadow->AppDomainManager;
	CAppDomainInfo* appDomainInfo = appDomainManager->Get(appDomainID);
	if (_profilerController->LogAppDomains)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(appDomainInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::AppDomainShutdown, appDomainInfo);
	}
	appDomainManager->Close(appDomainInfo);
	return S_OK;
}

// ASSEMBLY EVENTS
STDMETHODIMP CProfilerEntryPointBase::AssemblyLoadStarted(AssemblyID assemblyID)
{
	CAssemblyManager* assemblyManager = _processShadow->AssemblyManager;
	CAssemblyInfo* assemblyInfo = assemblyManager->Create(assemblyID);
	if (_profilerController->LogAssemblies)
	{
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		assemblyInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::AssemblyLoad, assemblyInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::AssemblyLoadFinished(AssemblyID assemblyID, HRESULT status)
{
	CAssemblyManager* assemblyManager = _processShadow->AssemblyManager;
	CAssemblyInfo* assemblyInfo = assemblyManager->Get(assemblyID);
	assemblyManager->Initialize(assemblyInfo);
	assemblyInfo->LoadResult = status;
	assemblyInfo->IsExcluded = _profilerController->IsAssemblyExcluded(assemblyInfo->Name);
	assemblyManager->Update(assemblyInfo);
	if (_profilerController->LogAssemblies)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(assemblyInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::AssemblyLoad, assemblyInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::AssemblyUnloadStarted(AssemblyID assemblyID)
{
	if (_profilerController->LogAssemblies)
	{
		CAssemblyManager* assemblyManager = _processShadow->AssemblyManager;
		CAssemblyInfo* assemblyInfo = assemblyManager->Get(assemblyID);
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		assemblyInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::AssemblyUnload, assemblyInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::AssemblyUnloadFinished(AssemblyID assemblyID, HRESULT status)
{
	CAssemblyManager* assemblyManager = _processShadow->AssemblyManager;
	CAssemblyInfo* assemblyInfo = assemblyManager->Get(assemblyID);
	if (_profilerController->LogAssemblies)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(assemblyInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::AssemblyUnload, assemblyInfo);
	}
	assemblyManager->Close(assemblyInfo);
	return S_OK;
}

// MODULE EVENTS
STDMETHODIMP CProfilerEntryPointBase::ModuleLoadStarted(ModuleID moduleID)
{
	CModuleManager* moduleManager = _processShadow->ModuleManager;
	CModuleInfo* moduleInfo = moduleManager->Create(moduleID);
	if (_profilerController->LogModules)
	{
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		moduleInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::ModuleLoad, moduleInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ModuleLoadFinished(ModuleID moduleID, HRESULT status)
{
	CModuleManager* moduleManager = _processShadow->ModuleManager;
	CModuleInfo* moduleInfo = moduleManager->Get(moduleID);
	moduleManager->Initialize(moduleInfo);
	moduleInfo->LoadResult = status;
	moduleManager->Update(moduleInfo);
	if (_profilerController->LogModules)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(moduleInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::ModuleLoad, moduleInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ModuleUnloadStarted(ModuleID moduleID)
{
	if (_profilerController->LogModules)
	{
		CModuleManager* moduleManager = _processShadow->ModuleManager;
		CModuleInfo* moduleInfo = moduleManager->Get(moduleID);
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		moduleInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::ModuleUnload, moduleInfo);
	}
	return S_OK;
}
	  
STDMETHODIMP CProfilerEntryPointBase::ModuleUnloadFinished(ModuleID moduleID, HRESULT status)
{
	CModuleManager* moduleManager = _processShadow->ModuleManager;
	CModuleInfo* moduleInfo = moduleManager->Get(moduleID);
	if (_profilerController->LogModules)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(moduleInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::ModuleUnload, moduleInfo);
	}
	moduleManager->Close(moduleInfo);
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ModuleAttachedToAssembly(ModuleID moduleID, AssemblyID assemblyID)
{
	CModuleManager* moduleManager = _processShadow->ModuleManager;
	CModuleInfo* moduleInfo = moduleManager->Get(moduleID);
	CAssemblyManager* assemblyManager = _processShadow->AssemblyManager;
	CAssemblyInfo* assemblyInfo = assemblyManager->Get(assemblyID);
	moduleInfo->AssemblyManagedId = assemblyInfo->ManagedId;
	moduleManager->Update(moduleInfo);
	return S_OK;
}

// CLASS EVENTS
STDMETHODIMP CProfilerEntryPointBase::ClassLoadStarted(ClassID classID)
{
	CClassManager* classManager = _processShadow->ClassManager;
	CClassInfo* classInfo = classManager->Create(classID);
	if (_profilerController->LogClasses)
	{
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		classInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::ClassLoad, classInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ClassLoadFinished(ClassID classID, HRESULT status)
{
	CClassManager* classManager = _processShadow->ClassManager;
	CClassInfo* classInfo = classManager->Get(classID);
	classManager->Initialize(classInfo);
	classInfo->LoadResult = status;
	classManager->Update(classInfo);
	if (_profilerController->LogClasses)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(classInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::ClassLoad, classInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ClassUnloadStarted(ClassID classID)
{
	if (_profilerController->LogClasses)
	{
		CClassManager* classManager = _processShadow->ClassManager;
		CClassInfo* classInfo = classManager->Get(classID);
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		classInfo->OwnerThreadManagedId = logger->ThreadInfo->ManagedId;
		logger->Callstack->Call(EventTypes::ClassUnload, classInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ClassUnloadFinished(ClassID classID, HRESULT status)
{
	CClassManager* classManager = _processShadow->ClassManager;
	CClassInfo* classInfo = classManager->Get(classID);
	if (_profilerController->LogClasses)
	{
		UINT_PTR ownerThreadManagedId = static_cast<UINT_PTR>(classInfo->OwnerThreadManagedId);
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(ownerThreadManagedId);
		logger->Callstack->Ret(EventTypes::ClassUnload, classInfo);
	}
	classManager->Close(classInfo);
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::FunctionUnloadStarted(FunctionID functionID)
{
	CFunctionManager* functionManager = _processShadow->FunctionManager;
	CFunctionInfo* functionInfo = functionManager->Get(functionID);
	functionManager->Close(functionInfo);
	return S_OK;
}

// JIT EVENTS
STDMETHODIMP CProfilerEntryPointBase::JITCompilationStarted(FunctionID functionID, BOOL fIsSafeToBlock)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::JITCompilationFinished(FunctionID functionID, HRESULT status, BOOL fIsSafeToBlock)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::JITCachedFunctionSearchStarted(FunctionID functionID, BOOL *pbUseCachedFunction)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::JITCachedFunctionSearchFinished(FunctionID functionID, COR_PRF_JIT_CACHE result)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::JITFunctionPitched(FunctionID functionID)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::JITInlining(FunctionID callerID, FunctionID calleeID, BOOL *pfShouldInline)
{
	return S_OK;
}

/// <summary>
/// The Runtime calls ThreadCreated to notify the code profiler that a thread has been created. The threadId is valid immediately.
/// </summary>
STDMETHODIMP CProfilerEntryPointBase::ThreadCreated(ThreadID threadID)
{
	CThreadManager* threadManager = _processShadow->ThreadManager;
	CThreadInfo* threadInfo = threadManager->Create(threadID);
	threadManager->Initialize(threadInfo);
	CThreadLogger* logger = _threadLoggerPool->CreateThreadLogger(threadInfo);
	if (_profilerController->LogThreads)
	{
		logger->Callstack->CallRet(EventTypes::ThreadCreate, threadInfo);
	}
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ThreadDestroyed(ThreadID threadID)
{
	CThreadManager* threadManager = _processShadow->ThreadManager;
	CThreadInfo* threadInfo = threadManager->Get(threadID);
	if (_profilerController->LogThreads)
	{
		CThreadLogger* logger = _threadLoggerPool->GetThreadLogger(threadID);
		logger->Callstack->CallRet(EventTypes::ThreadDestroy, threadInfo);
	}
	threadManager->Close(threadInfo);
	_threadLoggerPool->DestroyThreadLogger(threadID);
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ThreadAssignedToOSThread(ThreadID managedThreadID, DWORD osThreadID) 
{
	CThreadManager* threadManager = _processShadow->ThreadManager;
	CThreadInfo* threadInfo = threadManager->Get(managedThreadID);
	threadInfo->OsThreadId = osThreadID;
	threadManager->Update(threadInfo);
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ThreadNameChanged(ThreadID threadID, ULONG cchName, WCHAR name[])
{
	/*ThreadManager* threadManager = _processShadow->Threads;
	ThreadInfo* threadInfo = threadManager->Get(threadID);
	threadInfo->Name = name;
	threadManager->Update(threadInfo);*/
	return S_OK;
}

// REMOTING EVENTS (Client-side)
STDMETHODIMP CProfilerEntryPointBase::RemotingClientInvocationStarted()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RemotingClientSendingMessage(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RemotingClientReceivingReply(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RemotingClientInvocationFinished()
{
	return S_OK;
}

// REMOTING EVENTS (Server-side)
STDMETHODIMP CProfilerEntryPointBase::RemotingServerReceivingMessage(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RemotingServerInvocationStarted()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RemotingServerInvocationReturned()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RemotingServerSendingReply(GUID *pCookie, BOOL fIsAsync)
{
	return S_OK;
}

// CONTEXT EVENTS
STDMETHODIMP CProfilerEntryPointBase::UnmanagedToManagedTransition(FunctionID functionID, COR_PRF_TRANSITION_REASON reason)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ManagedToUnmanagedTransition(FunctionID functionID, COR_PRF_TRANSITION_REASON reason)
{
	return S_OK;
}

// SUSPENSION EVENTS
STDMETHODIMP CProfilerEntryPointBase::RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RuntimeSuspendFinished()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RuntimeSuspendAborted()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RuntimeResumeStarted()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RuntimeResumeFinished()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RuntimeThreadSuspended(ThreadID threadID)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RuntimeThreadResumed(ThreadID threadID)
{
	return S_OK;
}

// GC EVENTS
STDMETHODIMP CProfilerEntryPointBase::MovedReferences(ULONG cmovedObjectIDRanges, ObjectID oldObjectIDRangeStart[], ObjectID newObjectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ObjectAllocated(ObjectID objectID, ClassID classID)
{
	/*ULONG size = 0;
	_corProfilerInfo2->GetObjectSize(objectID, &size);
	CThreadEventsLogger* logger = _threadsPool->GetCurrentThreadLogger();
	logger->ObjectAllocated(classID, size);*/
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ObjectsAllocatedByClass(ULONG classCount, ClassID classIDs[], ULONG objects[])
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ObjectReferences(ObjectID objectID, ClassID classID, ULONG objectRefs, ObjectID objectRefIDs[])
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RootReferences(ULONG rootRefs, ObjectID rootRefIDs[])
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::GarbageCollectionStarted(int cGenerations, BOOL generationCollected[], COR_PRF_GC_REASON reason)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::SurvivingReferences(ULONG cSurvivingObjectIDRanges, ObjectID objectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::GarbageCollectionFinished()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::FinalizeableObjectQueued(DWORD finalizerFlags, ObjectID objectID)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::RootReferences2(ULONG cRootRefs, ObjectID rootRefIDs[], COR_PRF_GC_ROOT_KIND rootKinds[], COR_PRF_GC_ROOT_FLAGS rootFlags[], UINT_PTR rootIDs[])
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::HandleCreated(GCHandleID handleID, ObjectID initialObjectID)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::HandleDestroyed(GCHandleID handleID)
{
	return S_OK;
}

// EXCEPTION EVENTS (Exception creation)
STDMETHODIMP CProfilerEntryPointBase::ExceptionThrown(ObjectID objectID)
{
	CExceptionManager* exceptionManager = _processShadow->ExceptionManager;
	CExceptionInfo* exceptionInfo = exceptionManager->Create(objectID);
	exceptionManager->Initialize(exceptionInfo);
	
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->ExceptionStack->ExceptionThrown(exceptionInfo);
	return S_OK;
}

// EXCEPTION EVENTS (Search phase)
STDMETHODIMP CProfilerEntryPointBase::ExceptionSearchFunctionEnter(FunctionID functionID)
{
	CFunctionManager* functionManager = _processShadow->FunctionManager;
	CFunctionInfo* functionInfo = functionManager->Get(functionID);
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->ExceptionStack->ExceptionSearchFunctionEnter(functionInfo);
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionSearchFunctionLeave()
{
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->ExceptionStack->ExceptionSearchFunctionLeave();
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionSearchFilterEnter(FunctionID functionID)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionSearchFilterLeave()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionSearchCatcherFound(FunctionID functionID)
{
	CFunctionManager* functionManager = _processShadow->FunctionManager;
	CFunctionInfo* functionInfo = functionManager->Get(functionID);
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->ExceptionStack->ExceptionSearchCatcherFound(functionInfo);
	return S_OK;
}

/// <summary>
/// Called when a catch block for an exception is executed inside the common language runtime (CLR) itself. This method is obsolete in the .NET Framework version 2.0.
/// </summary>
STDMETHODIMP CProfilerEntryPointBase::ExceptionCLRCatcherFound()
{
	return S_OK;
}

/// <summary>
/// Called when a catch block for an exception is executed inside the common language runtime (CLR) itself. This method is obsolete in the .NET Framework version 2.0.
/// </summary>
STDMETHODIMP CProfilerEntryPointBase::ExceptionCLRCatcherExecute()
{
	return S_OK;
}

/// <summary>
/// Not implemented. A profiler that needs unmanaged exception information must obtain this information through other means.
/// </summary>
STDMETHODIMP CProfilerEntryPointBase::ExceptionOSHandlerEnter(FunctionID functionID)
{
	return S_OK;
}

/// <summary>
/// Not implemented. A profiler that needs unmanaged exception information must obtain this information through other means.
/// </summary>
STDMETHODIMP CProfilerEntryPointBase::ExceptionOSHandlerLeave(FunctionID functionID)
{
	return S_OK;
}

// EXCEPTION EVENTS (Unwind phase)
STDMETHODIMP CProfilerEntryPointBase::ExceptionUnwindFunctionEnter(FunctionID functionID)
{
	CFunctionManager* functionManager = _processShadow->FunctionManager;
	CFunctionInfo* functionInfo = functionManager->Get(functionID);
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->ExceptionStack->ExceptionUnwindFunctionEnter(functionInfo);
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionUnwindFunctionLeave()
{
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->ExceptionStack->ExceptionUnwindFunctionLeave();
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionUnwindFinallyEnter(FunctionID functionID)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionUnwindFinallyLeave()
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionCatcherEnter(FunctionID functionID, ObjectID objectID)
{
	CFunctionManager* functionManager = _processShadow->FunctionManager;
	CFunctionInfo* functionInfo = functionManager->Get(functionID);
	CExceptionManager* exceptionManager = _processShadow->ExceptionManager;
	CExceptionInfo* exceptionInfo = exceptionManager->Get(objectID);
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->ExceptionStack->ExceptionCatcherEnter(functionInfo, exceptionInfo);
	exceptionManager->Close(exceptionInfo);
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::ExceptionCatcherLeave()
{
	CExceptionManager* exceptionManager = _processShadow->ExceptionManager;
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	CExceptionInfo* exceptionInfo = logger->ExceptionStack->GetCurrentException();
	logger->ExceptionStack->ExceptionCatcherLeave();
	UINT_PTR exceptionManagedId = static_cast<UINT_PTR>(exceptionInfo->ManagedId);
	if (exceptionInfo != null && exceptionManager->Contains(exceptionManagedId))
	{
		exceptionManager->Close(exceptionInfo);
	}
	return S_OK;
}

// COM CLASSIC VTable
STDMETHODIMP CProfilerEntryPointBase::COMClassicVTableCreated(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable, ULONG cSlots)
{
	return S_OK;
}

STDMETHODIMP CProfilerEntryPointBase::COMClassicVTableDestroyed(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable)
{
	return S_OK;
}

void CProfilerEntryPointBase::FunctionEnter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{
	//InterlockedIncrement(&_hits);
	CFunctionInfo* functionInfo = (CFunctionInfo*)clientData;
	if (_profilerController->ProfileSql && functionInfo->IsSqlEntryPoint)
	{
		UINT_PTR commandAddress = 0;
		CSqlRequestManager* manager = _processShadow->SqlRequestManager;
		CManagedProvider::GetFirstArgument(argumentInfo, &commandAddress);
		CSqlRequestInfo* sqlRequest = manager->Create(commandAddress);
		manager->Initialize(sqlRequest);
		if (_profilerController->IsEnabled)
		{
			CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
			logger->Callstack->Call(EventTypes::SqlQuery, sqlRequest);
		}
	}
	else
	{
#ifdef THREAD_LOGGER_TLS
		CThreadLoggerPool::CurrentLogger->Callstack->Call(EventTypes::FunctionCall, functionInfo);
#else
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		logger->Callstack->Call(EventTypes::FunctionCall, functionInfo);
#endif
	}
}

void CProfilerEntryPointBase::FunctionLeave(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange)
{
	CFunctionInfo* functionInfo = (CFunctionInfo*)clientData;
	if (_profilerController->ProfileSql && functionInfo->IsSqlEntryPoint)
	{
		if (_profilerController->IsEnabled)
		{
			CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
			__uint id = logger->Callstack->CurrentUnitId();
			CSqlRequestInfo* sqlRequest = _processShadow->SqlRequestManager->Get(id);
			logger->Callstack->Ret(EventTypes::SqlQuery, sqlRequest);
		}
	}
	else
	{
#ifdef THREAD_LOGGER_TLS
		CThreadLoggerPool::CurrentLogger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
#else
		CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
		logger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
#endif
	}
}

void CProfilerEntryPointBase::FunctionTailcall(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func)
{
	CFunctionInfo* functionInfo = (CFunctionInfo*)clientData;
#ifdef THREAD_LOGGER_TLS
	CThreadLoggerPool::CurrentLogger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
#else
	CThreadLogger* logger = _threadLoggerPool->GetCurrentThreadLogger();
	logger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
#endif
}

UINT_PTR CProfilerEntryPointBase::FunctionMap(FunctionID functionID, BOOL *hookFunction)
{
	CFunctionManager* functionManager = _processShadow->FunctionManager;
	CFunctionInfo* functionInfo = functionManager->Create(functionID);
	functionManager->Initialize(functionInfo);
	if (functionInfo->ClassManagedId == 0)
	{
		functionInfo->IsTarget = false;
	}
	else
	{
		CAssemblyManager* assemblyManager = _processShadow->AssemblyManager;
		UINT_PTR assemblyManagedId = static_cast<UINT_PTR>(functionInfo->AssemblyManagedId);
		CAssemblyInfo* assemblyInfo = assemblyManager->Get(assemblyManagedId);
		if (assemblyInfo != NULL)
		{
			functionInfo->IsTarget = !assemblyInfo->IsExcluded && _profilerController->LogFunctionCalls;
		}
	}
	*hookFunction = functionInfo->IsTarget;
	functionManager->Update(functionInfo);
	return (UINT_PTR)functionInfo;
}

void CProfilerEntryPointBase::Dispose()
{
	if (_disposed)
	{
		return;
	}
	_disposed = true;
	__FREEOBJ(_threadLoggerPool);
	__FREEOBJ(_processShadow);
	__FREEOBJ(_profilerController);
	__FREEOBJ(_requestsServer);
	_corProfilerInfo2->Release();
	_corProfilerInfo2 = NULL;
	ProfilerCallbacksGlobal= NULL;
}