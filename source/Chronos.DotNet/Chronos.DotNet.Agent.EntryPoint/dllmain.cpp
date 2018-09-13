// dllmain.cpp : Implementation of DllMain.

#include "stdafx.h"
#include "resource.h"
#include "ChronosDotNetAgentEntryPoint_i.h"
#include "dllmain.h"
#include "Chronos.DotNet.Agent.EntryPoint.h"

CChronosDotNetAgentModule _AtlModule;
Chronos::Agent::DotNet::EntryPoint::AgentResolver _agentResolver;

// DLL Entry Point
extern "C" BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	hInstance;
	return _AtlModule.DllMain(dwReason, lpReserved); 
}

EXTERN_C __declspec(dllexport) void CreateChronosFramework(Chronos::Agent::IFrameworkAdapter** adapter)
{
	*adapter = new Chronos::Agent::DotNet::FrameworkAdapter();
}
