#pragma once
#include <cor.h>
#include <corprof.h>

//EXTERN_C void FunctionEnterNaked(FunctionID functionId);
//EXTERN_C void FunctionLeaveNaked(FunctionID functionId);
//EXTERN_C void FunctionTailcallNaked(FunctionID functionId);
//
EXTERN_C void FunctionEnter2Naked(FunctionID funcId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
EXTERN_C void FunctionLeave2Naked(FunctionID funcId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
EXTERN_C void FunctionTailcall2Naked(FunctionID funcId, UINT_PTR clientData, COR_PRF_FRAME_INFO func);
//
//EXTERN_C void FunctionEnter3Naked(FunctionIDOrClientID functionIDOrClientID);
//EXTERN_C void FunctionLeave3Naked(FunctionIDOrClientID functionIDOrClientID);
//EXTERN_C void FunctionTailcall3Naked(FunctionIDOrClientID functionIDOrClientID);
//
//EXTERN_C void FunctionEnter3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//EXTERN_C void FunctionLeave3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//EXTERN_C void FunctionTailcall3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);