// ProfilerCallback.h : Declaration of the ProfilerCallback

#pragma once
#include "resource.h"       // main symbols
#include "ChronosDotNetAgentEntryPoint_i.h"
#include "Chronos.DotNet.Agent.EntryPoint.h"

using namespace ATL;


// ProfilerCallback

class ATL_NO_VTABLE ProfilerCallback :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<ProfilerCallback, &CLSID_ProfilerCallback>,
	public ICorProfilerCallback3
{
public:
		DECLARE_REGISTRY_RESOURCEID(IDR_PROFILERCALLBACK)
		BEGIN_COM_MAP(ProfilerCallback)
			COM_INTERFACE_ENTRY(IUnknown)
			COM_INTERFACE_ENTRY(ICorProfilerCallback)
			COM_INTERFACE_ENTRY(ICorProfilerCallback2)
			COM_INTERFACE_ENTRY(ICorProfilerCallback3)
		END_COM_MAP()
		DECLARE_PROTECT_FINAL_CONSTRUCT()

		HRESULT FinalConstruct() { return S_OK; }
		void FinalRelease() { }
	
		ProfilerCallback(void);
		~ProfilerCallback(void);

		HRESULT SetupFunctionCallbacks();
		HRESULT InitializeInternal(IUnknown* corProfilerInfoUnk);

		// STARTUP EVENTS
		STDMETHOD(Initialize)(IUnknown* corProfilerInfoUnk);
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
		// ATTACH EVENTS
		STDMETHOD(InitializeForAttach)(IUnknown* corProfilerInfoUnk, void* pvClientData, UINT cbClientData);
		STDMETHOD(ProfilerAttachComplete)();
		STDMETHOD(ProfilerDetachSucceeded)();

	private:
		ICorProfilerInfo2* _corProfilerInfo;
		Chronos::Agent::Application* _application;
		Chronos::Agent::DotNet::Reflection::RuntimeMetadataProvider* _metadataProvider;
		Chronos::Agent::DotNet::EntryPoint::ThreadScopeDictionary<Chronos::Agent::DotNet::EntryPoint::FunctionExceptionTracer*>* _exceptionTracers;
};

OBJECT_ENTRY_AUTO(__uuidof(ProfilerCallback), ProfilerCallback)
