#include "ProfilingHooks.h"

CProfilingHooks::CProfilingHooks(void)
{
	// APPLICATION DOMAIN EVENTS
	_appDomainCreationStarted = null;
	_appDomainCreationFinished = null;
	_appDomainShutdownStarted = null;
	_appDomainShutdownFinished = null;
	// ASSEMBLY EVENTS
	_assemblyLoadStarted = null;
	_assemblyLoadFinished = null;
	_assemblyUnloadStarted = null;
	_assemblyUnloadFinished = null;
 	// MODULE EVENTS
	_moduleLoadStarted = null;
	_moduleLoadFinished = null;
	_moduleUnloadStarted = null;
	_moduleUnloadFinished = null;
	_moduleAttachedToAssembly = null;
	// CLASS EVENTS
	_classLoadStarted = null;
	_classLoadFinished = null;
	_classUnloadStarted = null;
	_classUnloadFinished = null;
	// FUNCTION EVENTS
	_functionLoadStarted = null;
	_functionUnloadStarted = null;
	_functionEnter = null;
	_functionLeave = null;
	_functionTailcall = null;
	_functionQuit = null;
	// THREAD EVENTS
	_threadCreated = null;
	_threadDestroyed = null;
	_threadAssignedToOSThread = null;
	_threadNameChanged = null;
	// EXCEPTION EVENTS (Exception creation)
	_exceptionThrown = null;
	// EXCEPTION EVENTS (Search phase)
	_exceptionSearchFunctionEnter = null;
	_exceptionSearchFunctionLeave = null;
	_exceptionSearchFilterEnter = null;
	_exceptionSearchFilterLeave = null;
	_exceptionSearchCatcherFound = null;
	_exceptionCLRCatcherFound = null;
	_exceptionCLRCatcherExecute = null;
	// EXCEPTION EVENTS (Unwind phase)
	_exceptionUnwindFunctionEnter = null;
	_exceptionUnwindFunctionLeave = null;
	_exceptionUnwindFinallyEnter = null;
	_exceptionUnwindFinallyLeave = null;
	_exceptionCatcherEnter = null;
	_exceptionCatcherLeave = null;
}

CProfilingHooks::~CProfilingHooks(void)
{
}
// STARTUP/SHUTDOWN EVENTS ==================================================================================================
ShutdownCallback CProfilingHooks::SubscribeShutdown(ShutdownCallback callback)
{
	SETUP_PROFILING_HOOK(ShutdownCallback, _shutdown, callback);
}

void CProfilingHooks::RaiseShutdown()
{
	RAISE_PROFILING_HOOK_0(_shutdown);
}

__bool CProfilingHooks::HookShutdown()
{
	HOOK_PROFILING_HOOK(_shutdown);
}
// APPLICATION DOMAIN EVENTS ================================================================================================
AppDomainCreationStartedCallback CProfilingHooks::SubscribeAppDomainCreationStarted(AppDomainCreationStartedCallback callback)
{
	SETUP_PROFILING_HOOK(AppDomainCreationStartedCallback, _appDomainCreationStarted, callback);
}
	
void CProfilingHooks::RaiseAppDomainCreationStarted(CAppDomainInfo* appDomainInfo)
{
	RAISE_PROFILING_HOOK_1(_appDomainCreationStarted, appDomainInfo);
}

__bool CProfilingHooks::HookAppDomainCreationStarted()
{
	HOOK_PROFILING_HOOK(_appDomainCreationStarted);
}

AppDomainCreationFinishedCallback CProfilingHooks::SubscribeAppDomainCreationFinished(AppDomainCreationFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(AppDomainCreationFinishedCallback, _appDomainCreationFinished, callback);
}

void CProfilingHooks::RaiseAppDomainCreationFinished(CAppDomainInfo* appDomainInfo)
{
	RAISE_PROFILING_HOOK_1(_appDomainCreationFinished, appDomainInfo);
}

__bool CProfilingHooks::HookAppDomainCreationFinished()
{
	HOOK_PROFILING_HOOK(_appDomainCreationFinished);
}

AppDomainShutdownStartedCallback CProfilingHooks::SubscribeAppDomainShutdownStarted(AppDomainShutdownStartedCallback callback)
{
	SETUP_PROFILING_HOOK(AppDomainShutdownStartedCallback, _appDomainShutdownStarted, callback);
}

void CProfilingHooks::RaiseAppDomainShutdownStarted(CAppDomainInfo* appDomainInfo)
{
	RAISE_PROFILING_HOOK_1(_appDomainShutdownStarted, appDomainInfo);
}

__bool CProfilingHooks::HookAppDomainShutdownStarted()
{
	HOOK_PROFILING_HOOK(_appDomainShutdownStarted);
}

AppDomainShutdownFinishedCallback CProfilingHooks::SubscribeAppDomainShutdownFinished(AppDomainShutdownFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(AppDomainShutdownFinishedCallback, _appDomainShutdownFinished, callback);
}

void CProfilingHooks::RaiseAppDomainShutdownFinished(CAppDomainInfo* appDomainInfo)
{
	RAISE_PROFILING_HOOK_1(_appDomainShutdownFinished, appDomainInfo);
}

__bool CProfilingHooks::HookAppDomainShutdownFinished()
{
	HOOK_PROFILING_HOOK(_appDomainShutdownFinished);
}
// ASSEMBLY EVENTS ==========================================================================================================
AssemblyLoadStartedCallback CProfilingHooks::SubscribeAssemblyLoadStarted(AssemblyLoadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(AssemblyLoadStartedCallback, _assemblyLoadStarted, callback);
}

void CProfilingHooks::RaiseAssemblyLoadStarted(CAssemblyInfo* assemblyInfo)
{
	RAISE_PROFILING_HOOK_1(_assemblyLoadStarted, assemblyInfo);
}

__bool CProfilingHooks::HookAssemblyLoadStarted()
{
	HOOK_PROFILING_HOOK(_assemblyLoadStarted);
}

AssemblyLoadFinishedCallback CProfilingHooks::SubscribeAssemblyLoadFinished(AssemblyLoadFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(AssemblyLoadFinishedCallback, _assemblyLoadFinished, callback);
}

void CProfilingHooks::RaiseAssemblyLoadFinished(CAssemblyInfo* assemblyInfo)
{
	RAISE_PROFILING_HOOK_1(_assemblyLoadFinished, assemblyInfo);
}

__bool CProfilingHooks::HookAssemblyLoadFinished()
{
	HOOK_PROFILING_HOOK(_assemblyLoadFinished);
}

AssemblyUnloadStartedCallback CProfilingHooks::SubscribeAssemblyUnloadStarted(AssemblyUnloadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(AssemblyUnloadStartedCallback, _assemblyUnloadStarted, callback);
}

void CProfilingHooks::RaiseAssemblyUnloadStarted(CAssemblyInfo* assemblyInfo)
{
	RAISE_PROFILING_HOOK_1(_assemblyUnloadStarted, assemblyInfo);
}

__bool CProfilingHooks::HookAssemblyUnloadStarted()
{
	HOOK_PROFILING_HOOK(_assemblyUnloadStarted);
}

AssemblyUnloadFinishedCallback CProfilingHooks::SubscribeAssemblyUnloadFinished(AssemblyUnloadFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(AssemblyUnloadFinishedCallback, _assemblyUnloadFinished, callback);
}

void CProfilingHooks::RaiseAssemblyUnloadFinished(CAssemblyInfo* assemblyInfo)
{
	RAISE_PROFILING_HOOK_1(_assemblyUnloadFinished, assemblyInfo);
}

__bool CProfilingHooks::HookAssemblyUnloadFinished()
{
	HOOK_PROFILING_HOOK(_assemblyUnloadFinished);
}
// MODULE EVENTS  ===========================================================================================================
ModuleLoadStartedCallback CProfilingHooks::SubscribeModuleLoadStarted(ModuleLoadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(ModuleLoadStartedCallback, _moduleLoadStarted, callback);
}

void CProfilingHooks::RaiseModuleLoadStarted(CModuleInfo* moduleInfo)
{
	RAISE_PROFILING_HOOK_1(_moduleLoadStarted, moduleInfo);
}

__bool CProfilingHooks::HookModuleLoadStarted()
{
	HOOK_PROFILING_HOOK(_moduleLoadStarted);
}

ModuleLoadFinishedCallback CProfilingHooks::SubscribeModuleLoadFinished(ModuleLoadFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(ModuleLoadFinishedCallback, _moduleLoadFinished, callback);
}

void CProfilingHooks::RaiseModuleLoadFinished(CModuleInfo* moduleInfo)
{
	RAISE_PROFILING_HOOK_1(_moduleLoadFinished, moduleInfo);
}

__bool CProfilingHooks::HookModuleLoadFinished()
{
	HOOK_PROFILING_HOOK(_moduleLoadFinished);
}

ModuleUnloadStartedCallback CProfilingHooks::SubscribeModuleUnloadStarted(ModuleUnloadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(ModuleUnloadStartedCallback, _moduleUnloadStarted, callback);
}

void CProfilingHooks::RaiseModuleUnloadStarted(CModuleInfo* moduleInfo)
{
	RAISE_PROFILING_HOOK_1(_moduleUnloadStarted, moduleInfo);
}

__bool CProfilingHooks::HookModuleUnloadStarted()
{
	HOOK_PROFILING_HOOK(_moduleUnloadStarted);
}

ModuleUnloadFinishedCallback CProfilingHooks::SubscribeModuleUnloadFinished(ModuleUnloadFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(ModuleUnloadFinishedCallback, _moduleUnloadFinished, callback);
}

void CProfilingHooks::RaiseModuleUnloadFinished(CModuleInfo* moduleInfo)
{
	RAISE_PROFILING_HOOK_1(_moduleUnloadFinished, moduleInfo);
}

__bool CProfilingHooks::HookModuleUnloadFinished()
{
	HOOK_PROFILING_HOOK(_moduleUnloadFinished);
}

ModuleAttachedToAssemblyCallback CProfilingHooks::SubscribeModuleAttachedToAssembly(ModuleAttachedToAssemblyCallback callback)
{
	SETUP_PROFILING_HOOK(ModuleAttachedToAssemblyCallback, _moduleAttachedToAssembly, callback);
}

void CProfilingHooks::RaiseModuleAttachedToAssembly(CModuleInfo* moduleInfo, CAssemblyInfo* assemblyInfo)
{
	RAISE_PROFILING_HOOK_2(_moduleAttachedToAssembly, moduleInfo, assemblyInfo);
}

__bool CProfilingHooks::HookModuleAttachedToAssembly()
{
	HOOK_PROFILING_HOOK(_moduleAttachedToAssembly);
}
// CLASS EVENTS  ============================================================================================================
ClassLoadStartedCallback CProfilingHooks::SubscribeClassLoadStarted(ClassLoadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(ClassLoadStartedCallback, _classLoadStarted, callback);
}

void CProfilingHooks::RaiseClassLoadStarted(CClassInfo* classInfo)
{
	RAISE_PROFILING_HOOK_1(_classLoadStarted, classInfo);
}

__bool CProfilingHooks::HookClassLoadStarted()
{
	HOOK_PROFILING_HOOK(_classLoadStarted);
}

ClassLoadFinishedCallback CProfilingHooks::SubscribeModuleLoadFinished(ClassLoadFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(ClassLoadFinishedCallback, _classLoadFinished, callback);
}

void CProfilingHooks::RaiseClassLoadFinished(CClassInfo* classInfo)
{
	RAISE_PROFILING_HOOK_1(_classLoadFinished, classInfo);
}

__bool CProfilingHooks::HookClassLoadFinished()
{
	HOOK_PROFILING_HOOK(_classLoadFinished);
}

ClassUnloadStartedCallback CProfilingHooks::SubscribeModuleUnloadStarted(ClassUnloadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(ClassUnloadStartedCallback, _classUnloadStarted, callback);
}

void CProfilingHooks::RaiseClassUnloadStarted(CClassInfo* classInfo)
{
	RAISE_PROFILING_HOOK_1(_classUnloadStarted, classInfo);
}

__bool CProfilingHooks::HookClassUnloadStarted()
{
	HOOK_PROFILING_HOOK(_classUnloadStarted);
}

ClassUnloadFinishedCallback CProfilingHooks::SubscribeModuleUnloadFinished(ClassUnloadFinishedCallback callback)
{
	SETUP_PROFILING_HOOK(ClassUnloadFinishedCallback, _classUnloadFinished, callback);
}

void CProfilingHooks::RaiseClassUnloadFinished(CClassInfo* classInfo)
{
	RAISE_PROFILING_HOOK_1(_classUnloadFinished, classInfo);
}

__bool CProfilingHooks::HookClassUnloadFinished()
{
	HOOK_PROFILING_HOOK(_classUnloadFinished);
}
// FUNCTION EVENTS  =========================================================================================================
FunctionLoadStartedCallback CProfilingHooks::SubscribeFunctionLoadStarted(FunctionLoadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(FunctionLoadStartedCallback, _functionLoadStarted, callback);
}

void CProfilingHooks::RaiseFunctionLoadStarted(CFunctionInfo* functionInfo, bool* hookFunction)
{
	RAISE_PROFILING_HOOK_2(_functionLoadStarted, functionInfo, hookFunction);
}

__bool CProfilingHooks::HookFunctionLoadStarted()
{
	HOOK_PROFILING_HOOK(_functionLoadStarted);
}

FunctionUnloadStartedCallback CProfilingHooks::SubscribeFunctionUnloadStarted(FunctionUnloadStartedCallback callback)
{
	SETUP_PROFILING_HOOK(FunctionUnloadStartedCallback, _functionUnloadStarted, callback);
}

void CProfilingHooks::RaiseFunctionUnloadStarted(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_functionUnloadStarted, functionInfo);
}

__bool CProfilingHooks::HookFunctionUnloadStarted()
{
	HOOK_PROFILING_HOOK(_functionUnloadStarted);
}

FunctionEnterCallback CProfilingHooks::SubscribeFunctionEnter(FunctionEnterCallback callback)
{
	SETUP_PROFILING_HOOK(FunctionEnterCallback, _functionEnter, callback);
}

void CProfilingHooks::RaiseFunctionEnter(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_functionEnter, functionInfo);
}

__bool CProfilingHooks::HookFunctionEnter()
{
	HOOK_PROFILING_HOOK(_functionEnter);
}

FunctionLeaveCallback CProfilingHooks::SubscribeFunctionLeave(FunctionLeaveCallback callback)
{
	SETUP_PROFILING_HOOK(FunctionLeaveCallback, _functionLeave, callback);
}

void CProfilingHooks::RaiseFunctionLeave(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_functionLeave, functionInfo);
}

__bool CProfilingHooks::HookFunctionLeave()
{
	HOOK_PROFILING_HOOK(_functionLeave);
}

FunctionTailcallCallback CProfilingHooks::SubscribeFunctionTailcall(FunctionTailcallCallback callback)
{
	SETUP_PROFILING_HOOK(FunctionTailcallCallback, _functionTailcall, callback);
}

void CProfilingHooks::RaiseFunctionTailcall(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_functionTailcall, functionInfo);
}

__bool CProfilingHooks::HookFunctionTailcall()
{
	HOOK_PROFILING_HOOK(_functionTailcall);
}

FunctionQuitCallback CProfilingHooks::SubscribeFunctionQuit(FunctionQuitCallback callback)
{
	SETUP_PROFILING_HOOK(FunctionQuitCallback, _functionQuit, callback);
}

void CProfilingHooks::RaiseFunctionQuit(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_functionQuit, functionInfo);
}

__bool CProfilingHooks::HookFunctionQuit()
{
	HOOK_PROFILING_HOOK(_functionQuit);
}
// THREAD EVENTS  ===========================================================================================================
ThreadCreatedCallstack CProfilingHooks::SubscribeThreadCreated(ThreadCreatedCallstack callback)
{
	SETUP_PROFILING_HOOK(ThreadCreatedCallstack, _threadCreated, callback);
}

void CProfilingHooks::RaiseThreadCreated(CThreadInfo* threadInfo)
{
	RAISE_PROFILING_HOOK_1(_threadCreated, threadInfo);
}

__bool CProfilingHooks::HookThreadCreated()
{
	HOOK_PROFILING_HOOK(_threadCreated);
}

ThreadDestroyedCallstack CProfilingHooks::SubscribeThreadDestroyed(ThreadDestroyedCallstack callback)
{
	SETUP_PROFILING_HOOK(ThreadDestroyedCallstack, _threadDestroyed, callback);
}

void CProfilingHooks::RaiseThreadDestroyed(CThreadInfo* threadInfo)
{
	RAISE_PROFILING_HOOK_1(_threadDestroyed, threadInfo);
}

__bool CProfilingHooks::HookThreadDestroyed()
{
	HOOK_PROFILING_HOOK(_threadDestroyed);
}

ThreadAssignedToOSThreadCallstack CProfilingHooks::SubscribeThreadAssignedToOSThread(ThreadAssignedToOSThreadCallstack callback)
{
	SETUP_PROFILING_HOOK(ThreadAssignedToOSThreadCallstack, _threadAssignedToOSThread, callback);
}

void CProfilingHooks::RaiseThreadAssignedToOSThread(CThreadInfo* threadInfo)
{
	RAISE_PROFILING_HOOK_1(_threadAssignedToOSThread, threadInfo);
}

__bool CProfilingHooks::HookThreadAssignedToOSThread()
{
	HOOK_PROFILING_HOOK(_threadAssignedToOSThread);
}

ThreadNameChangedCallstack CProfilingHooks::SubscribeThreadNameChanged(ThreadNameChangedCallstack callback)
{
	SETUP_PROFILING_HOOK(ThreadNameChangedCallstack, _threadNameChanged, callback);
}

void CProfilingHooks::RaiseThreadNameChanged(CThreadInfo* threadInfo)
{
	RAISE_PROFILING_HOOK_1(_threadNameChanged, threadInfo);
}

__bool CProfilingHooks::HookThreadNameChanged()
{
	HOOK_PROFILING_HOOK(_threadNameChanged);
}
// EXCEPTION EVENTS (Exception creation) ====================================================================================
ExceptionThrownCallback CProfilingHooks::SubscribeExceptionThrown(ExceptionThrownCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionThrownCallback, _exceptionThrown, callback);
}

void CProfilingHooks::RaiseExceptionThrown(CExceptionInfo* exceptionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionThrown, exceptionInfo);
}

__bool CProfilingHooks::HookExceptionThrown()
{
	HOOK_PROFILING_HOOK(_exceptionThrown);
}
// EXCEPTION EVENTS (Search phase) ==========================================================================================
ExceptionSearchFunctionEnterCallback CProfilingHooks::SubscribeExceptionSearchFunctionEnter(ExceptionSearchFunctionEnterCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionSearchFunctionEnterCallback, _exceptionSearchFunctionEnter, callback);
}

void CProfilingHooks::RaiseExceptionSearchFunctionEnter(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionSearchFunctionEnter, functionInfo);
}

__bool CProfilingHooks::HookExceptionSearchFunctionEnter()
{
	HOOK_PROFILING_HOOK(_exceptionSearchFunctionEnter);
}

ExceptionSearchFunctionLeaveCallback CProfilingHooks::SubscribeExceptionSearchFunctionLeave(ExceptionSearchFunctionLeaveCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionSearchFunctionLeaveCallback, _exceptionSearchFunctionLeave, callback);
}

void CProfilingHooks::RaiseExceptionSearchFunctionLeave(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionSearchFunctionLeave, functionInfo);
}

__bool CProfilingHooks::HookExceptionSearchFunctionLeave()
{
	HOOK_PROFILING_HOOK(_exceptionSearchFunctionLeave);
}

ExceptionSearchFilterEnterCallback CProfilingHooks::SubscribeExceptionSearchFilterEnter(ExceptionSearchFilterEnterCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionSearchFilterEnterCallback, _exceptionSearchFilterEnter, callback);
}

void CProfilingHooks::RaiseExceptionSearchFilterEnter(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionSearchFilterEnter, functionInfo);
}

__bool CProfilingHooks::HookExceptionSearchFilterEnter()
{
	HOOK_PROFILING_HOOK(_exceptionSearchFilterEnter);
}

ExceptionSearchFilterLeaveCallback CProfilingHooks::SubscribeExceptionSearchFilterLeave(ExceptionSearchFilterLeaveCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionSearchFilterLeaveCallback, _exceptionSearchFilterLeave, callback);
}

void CProfilingHooks::RaiseExceptionSearchFilterLeave(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionSearchFilterLeave, functionInfo);
}

__bool CProfilingHooks::HookExceptionSearchFilterLeave()
{
	HOOK_PROFILING_HOOK(_exceptionSearchFilterLeave);
}

ExceptionSearchCatcherFoundCallback CProfilingHooks::SubscribeExceptionSearchCatcherFound(ExceptionSearchCatcherFoundCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionSearchCatcherFoundCallback, _exceptionSearchCatcherFound, callback);
}

void CProfilingHooks::RaiseExceptionSearchCatcherFound(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionSearchCatcherFound, functionInfo);
}

__bool CProfilingHooks::HookExceptionSearchCatcherFound()
{
	HOOK_PROFILING_HOOK(_exceptionSearchCatcherFound);
}

ExceptionCLRCatcherFoundCallback CProfilingHooks::SubscribeExceptionCLRCatcherFound(ExceptionCLRCatcherFoundCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionCLRCatcherFoundCallback, _exceptionCLRCatcherFound, callback);
}

void CProfilingHooks::RaiseExceptionCLRCatcherFound(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionCLRCatcherFound, functionInfo);
}

__bool CProfilingHooks::HookExceptionCLRCatcherFound()
{
	HOOK_PROFILING_HOOK(_exceptionCLRCatcherFound);
}

ExceptionCLRCatcherExecuteCallback CProfilingHooks::SubscribeExceptionCLRCatcherExecute(ExceptionCLRCatcherExecuteCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionCLRCatcherExecuteCallback, _exceptionCLRCatcherExecute, callback);
}

void CProfilingHooks::RaiseExceptionCLRCatcherExecute(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionCLRCatcherExecute, functionInfo);
}

__bool CProfilingHooks::HookExceptionCLRCatcherExecute()
{
	HOOK_PROFILING_HOOK(_exceptionCLRCatcherExecute);
}
// EXCEPTION EVENTS (Unwind phase) ==========================================================================================
ExceptionUnwindFunctionEnterCallback CProfilingHooks::SubscribeExceptionUnwindFunctionEnter(ExceptionUnwindFunctionEnterCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionUnwindFunctionEnterCallback, _exceptionUnwindFunctionEnter, callback);
}

void CProfilingHooks::RaiseExceptionUnwindFunctionEnter(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionUnwindFunctionEnter, functionInfo);
}

__bool CProfilingHooks::HookExceptionUnwindFunctionEnter()
{
	HOOK_PROFILING_HOOK(_exceptionUnwindFunctionEnter);
}

ExceptionUnwindFunctionLeaveCallback CProfilingHooks::SubscribeExceptionUnwindFunctionLeave(ExceptionUnwindFunctionLeaveCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionUnwindFunctionLeaveCallback, _exceptionUnwindFunctionLeave, callback);
}

void CProfilingHooks::RaiseExceptionUnwindFunctionLeave(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionUnwindFunctionLeave, functionInfo);
}

__bool CProfilingHooks::HookExceptionUnwindFunctionLeave()
{
	HOOK_PROFILING_HOOK(_exceptionUnwindFunctionLeave);
}

ExceptionUnwindFinallyEnterCallback CProfilingHooks::SubscribeExceptionUnwindFinallyEnter(ExceptionUnwindFinallyEnterCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionUnwindFinallyEnterCallback, _exceptionUnwindFinallyEnter, callback);
}

void CProfilingHooks::RaiseExceptionUnwindFinallyEnter(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionUnwindFinallyEnter, functionInfo);
}

__bool CProfilingHooks::HookExceptionUnwindFinallyEnter()
{
	HOOK_PROFILING_HOOK(_exceptionUnwindFinallyEnter);
}

ExceptionUnwindFinallyLeaveCallback CProfilingHooks::SubscribeExceptionUnwindFinallyLeave(ExceptionUnwindFinallyLeaveCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionUnwindFinallyLeaveCallback, _exceptionUnwindFinallyLeave, callback);
}

void CProfilingHooks::RaiseExceptionUnwindFinallyLeave(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionUnwindFinallyLeave, functionInfo);
}

__bool CProfilingHooks::HookExceptionUnwindFinallyLeave()
{
	HOOK_PROFILING_HOOK(_exceptionUnwindFinallyLeave);
}

ExceptionCatcherEnterCallback CProfilingHooks::SubscribeExceptionCatcherEnter(ExceptionCatcherEnterCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionCatcherEnterCallback, _exceptionCatcherEnter, callback);
}

void CProfilingHooks::RaiseExceptionCatcherEnter(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionCatcherEnter, functionInfo);
}

__bool CProfilingHooks::HookExceptionCatcherEnter()
{
	HOOK_PROFILING_HOOK(_exceptionCatcherEnter);
}

ExceptionCatcherLeaveCallback CProfilingHooks::SubscribeExceptionCatcherLeave(ExceptionCatcherLeaveCallback callback)
{
	SETUP_PROFILING_HOOK(ExceptionCatcherLeaveCallback, _exceptionCatcherLeave, callback);
}

void CProfilingHooks::RaiseExceptionCatcherLeave(CFunctionInfo* functionInfo)
{
	RAISE_PROFILING_HOOK_1(_exceptionCatcherLeave, functionInfo);
}

__bool CProfilingHooks::HookExceptionCatcherLeave()
{
	HOOK_PROFILING_HOOK(_exceptionCatcherLeave);
}