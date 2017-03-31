// ProfilerEntryPoint.h : Declaration of the CProfilerEntryPoint

#pragma once
#include "resource.h"       // main symbols
#include "ChronosProfilerAgent32_i.h"
#include <ProfilerEntryPointBase.h>


#ifdef _WIN32_WCE
#error "Neutral-threaded COM objects are not supported on Windows CE."
#endif

using namespace ATL;

class ATL_NO_VTABLE CProfilerEntryPoint :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CProfilerEntryPointBase,
	public CComCoClass<CProfilerEntryPoint, &CLSID_ProfilerEntryPoint>,
	public IDispatchImpl<IProfilerEntryPoint, &IID_IProfilerEntryPoint, &LIBID_ChronosProfilerAgent32Lib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:

	DECLARE_REGISTRY_RESOURCEID(IDR_PROFILERENTRYPOINT)

	BEGIN_COM_MAP(CProfilerEntryPoint)
		COM_INTERFACE_ENTRY(IProfilerEntryPoint)
		COM_INTERFACE_ENTRY(IDispatch)
		COM_INTERFACE_ENTRY(ICorProfilerCallback)
		COM_INTERFACE_ENTRY(ICorProfilerCallback2)
	END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct() { return S_OK; }
	void FinalRelease() { }

	HRESULT InitializeFunctionCallbacks(__bool useFastCallbacks, ICorProfilerInfo2* corProfilerInfo2);
};

OBJECT_ENTRY_AUTO(__uuidof(ProfilerEntryPoint), CProfilerEntryPoint)
