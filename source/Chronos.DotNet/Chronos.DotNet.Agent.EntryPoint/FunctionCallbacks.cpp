#include "stdafx.h"
#include <Chronos.DotNet\Chronos.DotNet.Agent.h>
#include "FunctionCallbacks.h"

extern Chronos::Agent::DotNet::RuntimeProfilingEvents* GlobalEvents;

//=================================================================================================
void __stdcall FunctionEnterGlobal(FunctionID functionId)
{
	Chronos::Agent::DotNet::FunctionEnterEventArgs eventArgs(functionId, functionId);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionEnter, &eventArgs);
}

void __stdcall FunctionLeaveGlobal(FunctionID functionId)
{
	Chronos::Agent::DotNet::FunctionLeaveEventArgs eventArgs(functionId, functionId);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionLeave, &eventArgs);
}

void __stdcall FunctionTailcallGlobal(FunctionID functionId)
{
	Chronos::Agent::DotNet::FunctionTailcallEventArgs eventArgs(functionId, functionId);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionTailcall, &eventArgs);
}
//=================================================================================================
void __stdcall FunctionEnter2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{
	Chronos::Agent::DotNet::FunctionEnterEventArgs eventArgs(functionId, clientData);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionEnter, &eventArgs);
}

void __stdcall FunctionLeave2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange)
{
	Chronos::Agent::DotNet::FunctionLeaveEventArgs eventArgs(functionId, clientData);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionLeave, &eventArgs);
}

void __stdcall FunctionTailcall2Global(FunctionID functionId, UINT_PTR clientData, COR_PRF_FRAME_INFO func)
{
	Chronos::Agent::DotNet::FunctionTailcallEventArgs eventArgs(functionId, clientData);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionTailcall, &eventArgs);
}
//=================================================================================================
void __stdcall FunctionEnter3Global(FunctionIDOrClientID functionIDOrClientID)
{
	Chronos::Agent::DotNet::FunctionEnterEventArgs eventArgs(functionIDOrClientID.functionID, functionIDOrClientID.clientID);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionEnter, &eventArgs);
}

void __stdcall FunctionLeave3Global(FunctionIDOrClientID functionIDOrClientID)
{
	Chronos::Agent::DotNet::FunctionLeaveEventArgs eventArgs(functionIDOrClientID.functionID, functionIDOrClientID.clientID);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionLeave, &eventArgs);
}

void __stdcall FunctionTailcall3Global(FunctionIDOrClientID functionIDOrClientID)
{
	Chronos::Agent::DotNet::FunctionTailcallEventArgs eventArgs(functionIDOrClientID.functionID, functionIDOrClientID.clientID);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionTailcall, &eventArgs);
}
//=========================================================================
void __stdcall FunctionEnter3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
	Chronos::Agent::DotNet::FunctionEnterEventArgs eventArgs(functionIDOrClientID.functionID, functionIDOrClientID.clientID);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionEnter, &eventArgs);
}

void __stdcall FunctionLeave3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
	Chronos::Agent::DotNet::FunctionLeaveEventArgs eventArgs(functionIDOrClientID.functionID, functionIDOrClientID.clientID);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionLeave, &eventArgs);
}

void __stdcall FunctionTailcall3WithInfoGlobal(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
	Chronos::Agent::DotNet::FunctionTailcallEventArgs eventArgs(functionIDOrClientID.functionID, functionIDOrClientID.clientID);
	GlobalEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionTailcall, &eventArgs);
}
//=========================================================================