#pragma once
#include "Units.h"
#define SETUP_PROFILING_HOOK(TYPE, FIELD, CALLBACK) { TYPE previous = FIELD; FIELD = CALLBACK; return previous; }
#define HOOK_PROFILING_HOOK(FIELD) { return FIELD != null; }
#define RAISE_PROFILING_HOOK_0(FIELD) { if (FIELD != null) { FIELD(); } }
#define RAISE_PROFILING_HOOK_1(FIELD, PARAM1) { if (FIELD != null) { FIELD(PARAM1); } }
#define RAISE_PROFILING_HOOK_2(FIELD, PARAM1, PARAM2) { if (FIELD != null) { FIELD(PARAM1, PARAM2); } }
// STARTUP/SHUTDOWN EVENTS ==================================================================================================
typedef void (*ShutdownCallback)();
// APPLICATION DOMAIN EVENTS ================================================================================================
typedef void (*AppDomainCreationStartedCallback)(CAppDomainInfo*);
typedef void (*AppDomainCreationFinishedCallback)(CAppDomainInfo*);
typedef void (*AppDomainShutdownStartedCallback)(CAppDomainInfo*);
typedef void (*AppDomainShutdownFinishedCallback)(CAppDomainInfo*);
// ASSEMBLY EVENTS ==========================================================================================================
typedef void (*AssemblyLoadStartedCallback)(CAssemblyInfo*);
typedef void (*AssemblyLoadFinishedCallback)(CAssemblyInfo*);
typedef void (*AssemblyUnloadStartedCallback)(CAssemblyInfo*);
typedef void (*AssemblyUnloadFinishedCallback)(CAssemblyInfo*);
// MODULE EVENTS  ===========================================================================================================
typedef void (*ModuleLoadStartedCallback)(CModuleInfo*);
typedef void (*ModuleLoadFinishedCallback)(CModuleInfo*);
typedef void (*ModuleUnloadStartedCallback)(CModuleInfo*);
typedef void (*ModuleUnloadFinishedCallback)(CModuleInfo*);
typedef void (*ModuleAttachedToAssemblyCallback)(CModuleInfo*, CAssemblyInfo*);
// CLASS EVENTS  ============================================================================================================
typedef void (*ClassLoadStartedCallback)(CClassInfo*);
typedef void (*ClassLoadFinishedCallback)(CClassInfo*);
typedef void (*ClassUnloadStartedCallback)(CClassInfo*);
typedef void (*ClassUnloadFinishedCallback)(CClassInfo*);
// FUNCTION EVENTS  =========================================================================================================
typedef void (*FunctionLoadStartedCallback)(CFunctionInfo*, bool* hookFunction);
typedef void (*FunctionUnloadStartedCallback)(CFunctionInfo*);
typedef void (*FunctionEnterCallback)(CFunctionInfo*);
typedef void (*FunctionLeaveCallback)(CFunctionInfo*);
typedef void (*FunctionTailcallCallback)(CFunctionInfo*);
typedef void (*FunctionQuitCallback)(CFunctionInfo*);
// THREAD EVENTS  ===========================================================================================================
typedef void (*ThreadCreatedCallstack)(CThreadInfo*);
typedef void (*ThreadDestroyedCallstack)(CThreadInfo*);
typedef void (*ThreadAssignedToOSThreadCallstack)(CThreadInfo*);
typedef void (*ThreadNameChangedCallstack)(CThreadInfo*);
// EXCEPTION EVENTS (Exception creation) ====================================================================================
typedef void (*ExceptionThrownCallback)(CExceptionInfo*);
// EXCEPTION EVENTS (Search phase) ==========================================================================================
typedef void (*ExceptionSearchFunctionEnterCallback)(CFunctionInfo*);
typedef void (*ExceptionSearchFunctionLeaveCallback)(CFunctionInfo*);
typedef void (*ExceptionSearchFilterEnterCallback)(CFunctionInfo*);
typedef void (*ExceptionSearchFilterLeaveCallback)(CFunctionInfo*);
typedef void (*ExceptionSearchCatcherFoundCallback)(CFunctionInfo*);
typedef void (*ExceptionCLRCatcherFoundCallback)(CFunctionInfo*);
typedef void (*ExceptionCLRCatcherExecuteCallback)(CFunctionInfo*);
// EXCEPTION EVENTS (Unwind phase) ==========================================================================================	
typedef void (*ExceptionUnwindFunctionEnterCallback)(CFunctionInfo*);
typedef void (*ExceptionUnwindFunctionLeaveCallback)(CFunctionInfo*);
typedef void (*ExceptionUnwindFinallyEnterCallback)(CFunctionInfo*);
typedef void (*ExceptionUnwindFinallyLeaveCallback)(CFunctionInfo*);
typedef void (*ExceptionCatcherEnterCallback)(CFunctionInfo*);
typedef void (*ExceptionCatcherLeaveCallback)(CFunctionInfo*);
// 

class CProfilingHooks
{
public:
	CProfilingHooks(void);
	~CProfilingHooks(void);
// STARTUP/SHUTDOWN EVENTS ==================================================================================================
	ShutdownCallback SubscribeShutdown(ShutdownCallback callback);
	void RaiseShutdown();
	__bool HookShutdown();
// APPLICATION DOMAIN EVENTS ================================================================================================
	AppDomainCreationStartedCallback SubscribeAppDomainCreationStarted(AppDomainCreationStartedCallback callback);
	void RaiseAppDomainCreationStarted(CAppDomainInfo* appDomainInfo);
	__bool HookAppDomainCreationStarted();

	AppDomainCreationFinishedCallback SubscribeAppDomainCreationFinished(AppDomainCreationFinishedCallback callback);
	void RaiseAppDomainCreationFinished(CAppDomainInfo* appDomainInfo);
	__bool HookAppDomainCreationFinished();

	AppDomainShutdownStartedCallback SubscribeAppDomainShutdownStarted(AppDomainShutdownStartedCallback callback);
	void RaiseAppDomainShutdownStarted(CAppDomainInfo* appDomainInfo);
	__bool HookAppDomainShutdownStarted();

	AppDomainShutdownFinishedCallback SubscribeAppDomainShutdownFinished(AppDomainShutdownFinishedCallback callback);
	void RaiseAppDomainShutdownFinished(CAppDomainInfo* appDomainInfo);
	__bool HookAppDomainShutdownFinished();
// ASSEMBLY EVENTS ==========================================================================================================
	AssemblyLoadStartedCallback SubscribeAssemblyLoadStarted(AssemblyLoadStartedCallback callback);
	void RaiseAssemblyLoadStarted(CAssemblyInfo* assemblyInfo);
	__bool HookAssemblyLoadStarted();
	
	AssemblyLoadFinishedCallback SubscribeAssemblyLoadFinished(AssemblyLoadFinishedCallback callback);
	void RaiseAssemblyLoadFinished(CAssemblyInfo* assemblyInfo);
	__bool HookAssemblyLoadFinished();

	AssemblyUnloadStartedCallback SubscribeAssemblyUnloadStarted(AssemblyUnloadStartedCallback callback);
	void RaiseAssemblyUnloadStarted(CAssemblyInfo* assemblyInfo);
	__bool HookAssemblyUnloadStarted();

	AssemblyUnloadFinishedCallback SubscribeAssemblyUnloadFinished(AssemblyUnloadFinishedCallback callback);
	void RaiseAssemblyUnloadFinished(CAssemblyInfo* assemblyInfo);
	__bool HookAssemblyUnloadFinished();
// MODULE EVENTS  ===========================================================================================================
	ModuleLoadStartedCallback SubscribeModuleLoadStarted(ModuleLoadStartedCallback callback);
	void RaiseModuleLoadStarted(CModuleInfo* moduleInfo);
	__bool HookModuleLoadStarted();
	
	ModuleLoadFinishedCallback SubscribeModuleLoadFinished(ModuleLoadFinishedCallback callback);
	void RaiseModuleLoadFinished(CModuleInfo* moduleInfo);
	__bool HookModuleLoadFinished();

	ModuleUnloadStartedCallback SubscribeModuleUnloadStarted(ModuleUnloadStartedCallback callback);
	void RaiseModuleUnloadStarted(CModuleInfo* moduleInfo);
	__bool HookModuleUnloadStarted();

	ModuleUnloadFinishedCallback SubscribeModuleUnloadFinished(ModuleUnloadFinishedCallback callback);
	void RaiseModuleUnloadFinished(CModuleInfo* moduleInfo);
	__bool HookModuleUnloadFinished();

	ModuleAttachedToAssemblyCallback SubscribeModuleAttachedToAssembly(ModuleAttachedToAssemblyCallback callback);
	void RaiseModuleAttachedToAssembly(CModuleInfo* moduleInfo, CAssemblyInfo* assemblyInfo);
	__bool HookModuleAttachedToAssembly();
// CLASS EVENTS  ============================================================================================================
	ClassLoadStartedCallback SubscribeClassLoadStarted(ClassLoadStartedCallback callback);
	void RaiseClassLoadStarted(CClassInfo* classInfo);
	__bool HookClassLoadStarted();
	
	ClassLoadFinishedCallback SubscribeModuleLoadFinished(ClassLoadFinishedCallback callback);
	void RaiseClassLoadFinished(CClassInfo* classInfo);
	__bool HookClassLoadFinished();

	ClassUnloadStartedCallback SubscribeModuleUnloadStarted(ClassUnloadStartedCallback callback);
	void RaiseClassUnloadStarted(CClassInfo* classInfo);
	__bool HookClassUnloadStarted();

	ClassUnloadFinishedCallback SubscribeModuleUnloadFinished(ClassUnloadFinishedCallback callback);
	void RaiseClassUnloadFinished(CClassInfo* classInfo);
	__bool HookClassUnloadFinished();
// FUNCTION EVENTS  =========================================================================================================
	FunctionLoadStartedCallback SubscribeFunctionLoadStarted(FunctionLoadStartedCallback callback);
	void RaiseFunctionLoadStarted(CFunctionInfo* functionInfo, bool* hookFunction);
	__bool HookFunctionLoadStarted();

	FunctionUnloadStartedCallback SubscribeFunctionUnloadStarted(FunctionUnloadStartedCallback callback);
	void RaiseFunctionUnloadStarted(CFunctionInfo* functionInfo);
	__bool HookFunctionUnloadStarted();
	
	FunctionEnterCallback SubscribeFunctionEnter(FunctionEnterCallback callback);
	void RaiseFunctionEnter(CFunctionInfo* functionInfo);
	__bool HookFunctionEnter();

	FunctionLeaveCallback SubscribeFunctionLeave(FunctionLeaveCallback callback);
	void RaiseFunctionLeave(CFunctionInfo* functionInfo);
	__bool HookFunctionLeave();

	FunctionTailcallCallback SubscribeFunctionTailcall(FunctionTailcallCallback callback);
	void RaiseFunctionTailcall(CFunctionInfo* functionInfo);
	__bool HookFunctionTailcall();
	
	FunctionQuitCallback SubscribeFunctionQuit(FunctionQuitCallback callback);
	void RaiseFunctionQuit(CFunctionInfo* functionInfo);
	__bool HookFunctionQuit();
// THREAD EVENTS  ===========================================================================================================
	ThreadCreatedCallstack SubscribeThreadCreated(ThreadCreatedCallstack callback);
	void RaiseThreadCreated(CThreadInfo* threadInfo);
	__bool HookThreadCreated();

	ThreadDestroyedCallstack SubscribeThreadDestroyed(ThreadDestroyedCallstack callback);
	void RaiseThreadDestroyed(CThreadInfo* threadInfo);
	__bool HookThreadDestroyed();

	ThreadAssignedToOSThreadCallstack SubscribeThreadAssignedToOSThread(ThreadAssignedToOSThreadCallstack callback);
	void RaiseThreadAssignedToOSThread(CThreadInfo* threadInfo);
	__bool HookThreadAssignedToOSThread();

	ThreadNameChangedCallstack SubscribeThreadNameChanged(ThreadNameChangedCallstack callback);
	void RaiseThreadNameChanged(CThreadInfo* threadInfo);
	__bool HookThreadNameChanged();
// EXCEPTION EVENTS (Exception creation) ====================================================================================
	ExceptionThrownCallback SubscribeExceptionThrown(ExceptionThrownCallback callback);
	void RaiseExceptionThrown(CExceptionInfo* exceptionInfo);
	__bool HookExceptionThrown();
// EXCEPTION EVENTS (Search phase) ==========================================================================================
	ExceptionSearchFunctionEnterCallback SubscribeExceptionSearchFunctionEnter(ExceptionSearchFunctionEnterCallback callback);
	void RaiseExceptionSearchFunctionEnter(CFunctionInfo* functionInfo);
	__bool HookExceptionSearchFunctionEnter();
	
	ExceptionSearchFunctionLeaveCallback SubscribeExceptionSearchFunctionLeave(ExceptionSearchFunctionLeaveCallback callback);
	void RaiseExceptionSearchFunctionLeave(CFunctionInfo* functionInfo);
	__bool HookExceptionSearchFunctionLeave();
	
	ExceptionSearchFilterEnterCallback SubscribeExceptionSearchFilterEnter(ExceptionSearchFilterEnterCallback callback);
	void RaiseExceptionSearchFilterEnter(CFunctionInfo* functionInfo);
	__bool HookExceptionSearchFilterEnter();
	
	ExceptionSearchFilterLeaveCallback SubscribeExceptionSearchFilterLeave(ExceptionSearchFilterLeaveCallback callback);
	void RaiseExceptionSearchFilterLeave(CFunctionInfo* functionInfo);
	__bool HookExceptionSearchFilterLeave();
	
	ExceptionSearchCatcherFoundCallback SubscribeExceptionSearchCatcherFound(ExceptionSearchCatcherFoundCallback callback);
	void RaiseExceptionSearchCatcherFound(CFunctionInfo* functionInfo);
	__bool HookExceptionSearchCatcherFound();

	ExceptionCLRCatcherFoundCallback SubscribeExceptionCLRCatcherFound(ExceptionCLRCatcherFoundCallback callback);
	void RaiseExceptionCLRCatcherFound(CFunctionInfo* functionInfo);
	__bool HookExceptionCLRCatcherFound();
	
	ExceptionCLRCatcherExecuteCallback SubscribeExceptionCLRCatcherExecute(ExceptionCLRCatcherExecuteCallback callback);
	void RaiseExceptionCLRCatcherExecute(CFunctionInfo* functionInfo);
	__bool HookExceptionCLRCatcherExecute();
// EXCEPTION EVENTS (Unwind phase) ==========================================================================================
	ExceptionUnwindFunctionEnterCallback SubscribeExceptionUnwindFunctionEnter(ExceptionUnwindFunctionEnterCallback callback);
	void RaiseExceptionUnwindFunctionEnter(CFunctionInfo* functionInfo);
	__bool HookExceptionUnwindFunctionEnter();
	
	ExceptionUnwindFunctionLeaveCallback SubscribeExceptionUnwindFunctionLeave(ExceptionUnwindFunctionLeaveCallback callback);
	void RaiseExceptionUnwindFunctionLeave(CFunctionInfo* functionInfo);
	__bool HookExceptionUnwindFunctionLeave();
	
	ExceptionUnwindFinallyEnterCallback SubscribeExceptionUnwindFinallyEnter(ExceptionUnwindFinallyEnterCallback callback);
	void RaiseExceptionUnwindFinallyEnter(CFunctionInfo* functionInfo);
	__bool HookExceptionUnwindFinallyEnter();
	
	ExceptionUnwindFinallyLeaveCallback SubscribeExceptionUnwindFinallyLeave(ExceptionUnwindFinallyLeaveCallback callback);
	void RaiseExceptionUnwindFinallyLeave(CFunctionInfo* functionInfo);
	__bool HookExceptionUnwindFinallyLeave();
	
	ExceptionCatcherEnterCallback SubscribeExceptionCatcherEnter(ExceptionCatcherEnterCallback callback);
	void RaiseExceptionCatcherEnter(CFunctionInfo* functionInfo);
	__bool HookExceptionCatcherEnter();
	
	ExceptionCatcherLeaveCallback SubscribeExceptionCatcherLeave(ExceptionCatcherLeaveCallback callback);
	void RaiseExceptionCatcherLeave(CFunctionInfo* functionInfo);
	__bool HookExceptionCatcherLeave();

private:
// STARTUP/SHUTDOWN EVENTS ==================================================================================================
	ShutdownCallback _shutdown;
// APPLICATION DOMAIN EVENTS ================================================================================================
	AppDomainCreationStartedCallback _appDomainCreationStarted;
	AppDomainCreationFinishedCallback _appDomainCreationFinished;
	AppDomainShutdownStartedCallback _appDomainShutdownStarted;
	AppDomainShutdownFinishedCallback _appDomainShutdownFinished;
// ASSEMBLY EVENTS ==========================================================================================================
	AssemblyLoadStartedCallback _assemblyLoadStarted;
	AssemblyLoadFinishedCallback _assemblyLoadFinished;
	AssemblyUnloadStartedCallback _assemblyUnloadStarted;
	AssemblyUnloadFinishedCallback _assemblyUnloadFinished;
// MODULE EVENTS  ===========================================================================================================
	ModuleLoadStartedCallback _moduleLoadStarted;
	ModuleLoadFinishedCallback _moduleLoadFinished;
	ModuleUnloadStartedCallback _moduleUnloadStarted;
	ModuleUnloadFinishedCallback _moduleUnloadFinished;
	ModuleAttachedToAssemblyCallback _moduleAttachedToAssembly;
// CLASS EVENTS  ============================================================================================================
	ClassLoadStartedCallback _classLoadStarted;
	ClassLoadStartedCallback _classLoadFinished;
	ClassLoadStartedCallback _classUnloadStarted;
	ClassLoadStartedCallback _classUnloadFinished;
// FUNCTION EVENTS  =========================================================================================================
	FunctionLoadStartedCallback _functionLoadStarted;
	FunctionUnloadStartedCallback _functionUnloadStarted;
	FunctionEnterCallback _functionEnter;
	FunctionLeaveCallback _functionLeave;
	FunctionTailcallCallback _functionTailcall;
	FunctionQuitCallback _functionQuit;
// THREAD EVENTS  ===========================================================================================================
	ThreadCreatedCallstack _threadCreated;
	ThreadDestroyedCallstack _threadDestroyed;
	ThreadAssignedToOSThreadCallstack _threadAssignedToOSThread;
	ThreadNameChangedCallstack _threadNameChanged;
// EXCEPTION EVENTS (Exception creation) ====================================================================================
	ExceptionThrownCallback _exceptionThrown;
// EXCEPTION EVENTS (Search phase) ==========================================================================================
	ExceptionSearchFunctionEnterCallback _exceptionSearchFunctionEnter;
	ExceptionSearchFunctionLeaveCallback _exceptionSearchFunctionLeave;
	ExceptionSearchFilterEnterCallback _exceptionSearchFilterEnter;
	ExceptionSearchFilterLeaveCallback _exceptionSearchFilterLeave;
	ExceptionSearchCatcherFoundCallback _exceptionSearchCatcherFound;
	ExceptionCLRCatcherFoundCallback _exceptionCLRCatcherFound;
	ExceptionCLRCatcherExecuteCallback _exceptionCLRCatcherExecute;
// EXCEPTION EVENTS (Unwind phase) ==========================================================================================	
	ExceptionUnwindFunctionEnterCallback _exceptionUnwindFunctionEnter;
	ExceptionUnwindFunctionLeaveCallback _exceptionUnwindFunctionLeave;
	ExceptionUnwindFinallyEnterCallback _exceptionUnwindFinallyEnter;
	ExceptionUnwindFinallyLeaveCallback _exceptionUnwindFinallyLeave;
	ExceptionCatcherEnterCallback _exceptionCatcherEnter;
	ExceptionCatcherLeaveCallback _exceptionCatcherLeave;
};

