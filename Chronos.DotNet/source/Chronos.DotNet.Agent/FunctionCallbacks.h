#pragma once
#include <cor.h>
#include <corprof.h>

//=================================================================================================
void __stdcall FunctionEnterGlobal(FunctionID functionId);
void __stdcall FunctionLeaveGlobal(FunctionID functionId);
void __stdcall FunctionTailcallGlobal(FunctionID functionId);
//=================================================================================================
void __stdcall FunctionEnter2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
void __stdcall FunctionLeave2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
void __stdcall FunctionTailcall2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func);
//=================================================================================================
void __stdcall FunctionEnter3Global(FunctionIDOrClientID functionIDOrClientID);
void __stdcall FunctionLeave3Global(FunctionIDOrClientID functionIDOrClientID);
void __stdcall FunctionTailcall3Global(FunctionIDOrClientID functionIDOrClientID);
//=================================================================================================
void __stdcall FunctionEnter3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
void __stdcall FunctionLeave3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
void __stdcall FunctionTailcall3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
//=================================================================================================