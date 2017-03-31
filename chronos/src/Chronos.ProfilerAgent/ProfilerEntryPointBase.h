#pragma once
#include "ProcessShadow.h"
#include "ThreadLoggerPool.h"
#include "ProfilerController.h"
#include "RequestsServer.h"
#include "ManagedProvider.h"
#include "ProfilingHooks.h"
//#include "ProcDump.h"

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
		const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}
MIDL_DEFINE_GUID(IID, IID_ICustomCorProfilerInfo,0xCC0935CD,0xA518,0x487d,0xB0,0xBB,0xA9,0x32,0x14,0xE6,0x54,0x78);

class CProfilerEntryPointBase : public ICorProfilerCallback2
{
public:
	CProfilerEntryPointBase(void);
	~CProfilerEntryPointBase(void);

	STDMETHOD(Initialize)(IUnknown *corProfilerInfoUnk);
	STDMETHOD(Shutdown)();
	// APPLICATION DOMAIN EVENTS
   	STDMETHOD(AppDomainCreationStarted)(AppDomainID appDomainID);
	STDMETHOD(AppDomainCreationFinished)(AppDomainID appDomainID, HRESULT status);
	STDMETHOD(AppDomainShutdownStarted)(AppDomainID appDomainID);
	STDMETHOD(AppDomainShutdownFinished)(AppDomainID appDomainID, HRESULT status);
 	// ASSEMBLY EVENTS
   	STDMETHOD(AssemblyLoadStarted)(AssemblyID assemblyID);
	STDMETHOD(AssemblyLoadFinished)(AssemblyID assemblyID, HRESULT status);
	STDMETHOD(AssemblyUnloadStarted)(AssemblyID assemblyID);
	STDMETHOD(AssemblyUnloadFinished)(AssemblyID assemblyID, HRESULT status);
 	// MODULE EVENTS
   	STDMETHOD(ModuleLoadStarted)(ModuleID moduleID);
	STDMETHOD(ModuleLoadFinished)(ModuleID moduleID, HRESULT status);
	STDMETHOD(ModuleUnloadStarted)(ModuleID moduleID);
	STDMETHOD(ModuleUnloadFinished)(ModuleID moduleID, HRESULT status);
	STDMETHOD(ModuleAttachedToAssembly)(ModuleID moduleID, AssemblyID assemblyID);
	// CLASS EVENTS
	STDMETHOD(ClassLoadStarted)(ClassID classID);
	STDMETHOD(ClassLoadFinished)(ClassID classID, HRESULT status);
 	STDMETHOD(ClassUnloadStarted)(ClassID classID);
	STDMETHOD(ClassUnloadFinished)(ClassID classID, HRESULT status);
	STDMETHOD(FunctionUnloadStarted)(FunctionID functionID);
	// JIT EVENTS
	STDMETHOD(JITCompilationStarted)(FunctionID functionID, BOOL fIsSafeToBlock);
	STDMETHOD(JITCompilationFinished)(FunctionID functionID, HRESULT status, BOOL fIsSafeToBlock);
	STDMETHOD(JITCachedFunctionSearchStarted)(FunctionID functionID, BOOL *pbUseCachedFunction);
	STDMETHOD(JITCachedFunctionSearchFinished)(FunctionID functionID, COR_PRF_JIT_CACHE result);
	STDMETHOD(JITFunctionPitched)(FunctionID functionID);
	STDMETHOD(JITInlining)(FunctionID callerID, FunctionID calleeID, BOOL *pfShouldInline);
	// THREAD EVENTS
	STDMETHOD(ThreadCreated)(ThreadID threadID);
	STDMETHOD(ThreadDestroyed)(ThreadID threadID);
	STDMETHOD(ThreadAssignedToOSThread)(ThreadID managedThreadID, DWORD osThreadID);
	STDMETHOD(ThreadNameChanged)(ThreadID threadId, ULONG cchName, WCHAR name[]);
	// REMOTING EVENTS (Client-side)
	STDMETHOD(RemotingClientInvocationStarted)();
	STDMETHOD(RemotingClientSendingMessage)(GUID *pCookie, BOOL fIsAsync);
	STDMETHOD(RemotingClientReceivingReply)(GUID *pCookie, BOOL fIsAsync);
	STDMETHOD(RemotingClientInvocationFinished)();
	// REMOTING EVENTS (Server-side)
	STDMETHOD(RemotingServerReceivingMessage)(GUID *pCookie, BOOL fIsAsync);
	STDMETHOD(RemotingServerInvocationStarted)();
	STDMETHOD(RemotingServerInvocationReturned)();
	STDMETHOD(RemotingServerSendingReply)(GUID *pCookie, BOOL fIsAsync);
	// CONTEXT EVENTS
	STDMETHOD(UnmanagedToManagedTransition)(FunctionID functionID, COR_PRF_TRANSITION_REASON reason);
	STDMETHOD(ManagedToUnmanagedTransition)(FunctionID functionID, COR_PRF_TRANSITION_REASON reason);
	// SUSPENSION EVENTS
	STDMETHOD(RuntimeSuspendStarted)(COR_PRF_SUSPEND_REASON suspendReason);
	STDMETHOD(RuntimeSuspendFinished)();
	STDMETHOD(RuntimeSuspendAborted)();
	STDMETHOD(RuntimeResumeStarted)();
	STDMETHOD(RuntimeResumeFinished)();
	STDMETHOD(RuntimeThreadSuspended)(ThreadID threadid);
	STDMETHOD(RuntimeThreadResumed)(ThreadID threadid);
	// GC EVENTS
	STDMETHOD(MovedReferences)(ULONG cmovedObjectIDRanges, ObjectID oldObjectIDRangeStart[], ObjectID newObjectIDRangeStart[], ULONG cObjectIDRangeLength[]);
	STDMETHOD(ObjectAllocated)(ObjectID objectID, ClassID classID);
	STDMETHOD(ObjectsAllocatedByClass)(ULONG classCount, ClassID classIDs[], ULONG objects[]);
	STDMETHOD(ObjectReferences)(ObjectID objectID, ClassID classID, ULONG cObjectRefs, ObjectID objectRefIDs[]);
	STDMETHOD(RootReferences)(ULONG cRootRefs, ObjectID rootRefIDs[]);
	STDMETHOD(GarbageCollectionStarted)(int cGenerations, BOOL generationCollected[], COR_PRF_GC_REASON reason);
	STDMETHOD(SurvivingReferences)(ULONG cSurvivingObjectIDRanges, ObjectID objectIDRangeStart[], ULONG cObjectIDRangeLength[]);
	STDMETHOD(GarbageCollectionFinished)();
	STDMETHOD(FinalizeableObjectQueued)(DWORD finalizerFlags, ObjectID objectID);
	STDMETHOD(RootReferences2)(ULONG cRootRefs, ObjectID rootRefIds[], COR_PRF_GC_ROOT_KIND rootKinds[], COR_PRF_GC_ROOT_FLAGS rootFlags[], UINT_PTR rootIds[]);
	STDMETHOD(HandleCreated)(GCHandleID handleId, ObjectID initialObjectId);
	STDMETHOD(HandleDestroyed)(GCHandleID handleId);
	// EXCEPTION EVENTS (Exception creation)
	STDMETHOD(ExceptionThrown)(ObjectID thrownObjectID);
	// EXCEPTION EVENTS (Search phase)
	STDMETHOD(ExceptionSearchFunctionEnter)(FunctionID functionID);
	STDMETHOD(ExceptionSearchFunctionLeave)();
	STDMETHOD(ExceptionSearchFilterEnter)(FunctionID functionID);
	STDMETHOD(ExceptionSearchFilterLeave)();
	STDMETHOD(ExceptionSearchCatcherFound)(FunctionID functionID);
	STDMETHOD(ExceptionCLRCatcherFound)();
	STDMETHOD(ExceptionCLRCatcherExecute)();
	STDMETHOD(ExceptionOSHandlerEnter)(FunctionID functionID);
	STDMETHOD(ExceptionOSHandlerLeave)(FunctionID functionID);
	// EXCEPTION EVENTS (Unwind phase)
	STDMETHOD(ExceptionUnwindFunctionEnter)(FunctionID functionID);
	STDMETHOD(ExceptionUnwindFunctionLeave)();
	STDMETHOD(ExceptionUnwindFinallyEnter)(FunctionID functionID);
	STDMETHOD(ExceptionUnwindFinallyLeave)();
	STDMETHOD(ExceptionCatcherEnter)(FunctionID functionID, ObjectID objectID);
	STDMETHOD(ExceptionCatcherLeave)();
	// COM CLASSIC VTable
	STDMETHOD(COMClassicVTableCreated)(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable, ULONG cSlots);
	STDMETHOD(COMClassicVTableDestroyed)(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable);

 	
	virtual HRESULT InitializeFunctionCallbacks(__bool useFastCallbacks, ICorProfilerInfo2* corProfilerInfo2) = 0;
	void FunctionEnter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
	void FunctionLeave(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
	void FunctionTailcall(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func);


	static UINT_PTR _stdcall FunctionMapper(FunctionID functionID, BOOL* hookFunction);
	UINT_PTR FunctionMap(FunctionID functionID, BOOL* hookFunction);
	
	static void SetInstance(CProfilerEntryPointBase* instance);
	static CProfilerEntryPointBase* ProfilerCallbacksGlobal;
#ifdef FUNCTION_EVENT_DIRECT_CALL
	static CThreadLoggerPool* ThreadLoggerPool;
#endif


protected:
	void Dispose();

protected:
	CProfilingHooks ProfilingHooks;
	ICorProfilerInfo2* _corProfilerInfo2;
	CProcessShadow* _processShadow;
	CProfilerController* _profilerController;
	CThreadLoggerPool* _threadLoggerPool;
	CRequestsServer* _requestsServer;
	volatile bool _disposed;
	volatile __ulong _hits;
};
