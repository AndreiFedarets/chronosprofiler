// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "Chronos.Common.Agent.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

EXTERN_C __declspec(dllexport) void CreateChronosFramework(Chronos::Agent::IFrameworkAdapter** adapter)
{
	*adapter = new Chronos::Agent::Common::FrameworkAdapter();
}