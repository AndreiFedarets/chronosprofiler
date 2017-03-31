#include "StackTracer.h"


CStackTracer::CStackTracer(CFunctionManager* functionManager, ICorProfilerInfo2* corProfilerInfo2)
	: _functionManager(functionManager), _corProfilerInfo2(corProfilerInfo2)
{
}

CStackTracer::~CStackTracer(void)
{
}

///======================================================================================================================
HRESULT __stdcall OnStackSnapshotCallback(FunctionID funcId, UINT_PTR ip, COR_PRF_FRAME_INFO frameInfo, ULONG32 contextSize, BYTE context[], void *clientData)
{
	CTraceSnapshotContext* snapshotContext = (CTraceSnapshotContext*)clientData;
	if (funcId != 0)
	{
		CFunctionInfo* functionInfo = snapshotContext->FunctionManager->Get(funcId);
		if (functionInfo != null)
		{
			snapshotContext->Stack->push(functionInfo);
		}
	}
	return S_OK;
}
///======================================================================================================================
#pragma warning(push)
#pragma warning(disable:4482)
void CStackTracer::DumpStack(std::queue<CFunctionInfo*>* stack)
{
	CTraceSnapshotContext snapshotContext(_functionManager, stack);
	ULONG32 flags = (ULONG32)_COR_PRF_SNAPSHOT_INFO::COR_PRF_SNAPSHOT_DEFAULT;
	_corProfilerInfo2->DoStackSnapshot(null, (StackSnapshotCallback*)OnStackSnapshotCallback, flags, &snapshotContext, null, 0);
}
#pragma warning(pop)