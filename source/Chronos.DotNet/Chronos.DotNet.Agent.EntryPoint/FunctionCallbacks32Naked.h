#pragma once
#include <cor.h>
#include <corprof.h>

////=================================================================================================
//void FunctionEnterNaked(FunctionID functionId);
//void FunctionLeaveNaked(FunctionID functionId);
//void FunctionTailcallNaked(FunctionID functionId);
////=================================================================================================
//void FunctionEnter2Naked(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
//void FunctionLeave2Naked(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
//void FunctionTailcall2Naked(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func);
////=================================================================================================
void FunctionEnter3Naked(FunctionIDOrClientID functionIDOrClientID);
void FunctionLeave3Naked(FunctionIDOrClientID functionIDOrClientID);
void FunctionTailcall3Naked(FunctionIDOrClientID functionIDOrClientID);
////=================================================================================================
//void FunctionEnter3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//void FunctionLeave3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//void FunctionTailcall3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
////=================================================================================================