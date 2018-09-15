#pragma once
#include <cor.h>
#include <corprof.h>

//=================================================================================================
EXTERN_C void __stdcall FunctionEnterGlobal(FunctionID functionId);
EXTERN_C void __stdcall FunctionLeaveGlobal(FunctionID functionId);
EXTERN_C void __stdcall FunctionTailcallGlobal(FunctionID functionId);
//=================================================================================================
EXTERN_C void __stdcall FunctionEnter2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
EXTERN_C void __stdcall FunctionLeave2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
EXTERN_C void __stdcall FunctionTailcall2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func);
//=================================================================================================
EXTERN_C void __stdcall FunctionEnter3Global(FunctionIDOrClientID functionIDOrClientID);
EXTERN_C void __stdcall FunctionLeave3Global(FunctionIDOrClientID functionIDOrClientID);
EXTERN_C void __stdcall FunctionTailcall3Global(FunctionIDOrClientID functionIDOrClientID);
//=================================================================================================
EXTERN_C void __stdcall FunctionEnter3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
EXTERN_C void __stdcall FunctionLeave3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
EXTERN_C void __stdcall FunctionTailcall3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//=================================================================================================