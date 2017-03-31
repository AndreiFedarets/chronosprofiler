#include "stdafx.h"
#include "ProfilerEntryPoint.h"

EXTERN_C void EnterNaked2(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
EXTERN_C void LeaveNaked2(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
EXTERN_C void TailcallNaked2(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func);


EXTERN_C void __stdcall FunctionEnterGlobal(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argInfo)
{
	CProfilerEntryPointBase::ProfilerCallbacksGlobal->FunctionEnter(functionID, clientData, frameInfo, argInfo);
}

EXTERN_C void __stdcall FunctionLeaveGlobal(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange)
{
	CProfilerEntryPointBase::ProfilerCallbacksGlobal->FunctionLeave(functionID,clientData,frameInfo,retvalRange);
}

EXTERN_C void __stdcall FunctionTailcallGlobal(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo)
{
	CProfilerEntryPointBase::ProfilerCallbacksGlobal->FunctionTailcall(functionID, clientData, frameInfo);
}

HRESULT CProfilerEntryPoint::InitializeFunctionCallbacks(__bool useFastCallbacks, ICorProfilerInfo2* corProfilerInfo2)
{
	HRESULT result;
	if (useFastCallbacks)
	{
		result = _corProfilerInfo2->SetEnterLeaveFunctionHooks2(EnterNaked2, LeaveNaked2, TailcallNaked2);
	}
	else
	{
		result = _corProfilerInfo2->SetEnterLeaveFunctionHooks2(EnterNaked2, LeaveNaked2, TailcallNaked2);
	}
	return result;
}