// Chronos.DotNet.SqlProfiler.Agent.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "Chronos.DotNet.SqlProfiler.Agent.h"

EXTERN_C __declspec(dllexport) void CreateChronosProfilingType(Chronos::Agent::IProfilingTypeAdapter** adapter)
{
	*adapter = new Chronos::Agent::DotNet::SqlProfiler::ProfilingTypeAdapter();
}


EXTERN_C __declspec(dllexport) void BeginSqlQuery(wchar_t* sqlQuery)
{
	Chronos::Agent::DotNet::SqlProfiler::ProfilingTypeAdapter::BeginExecuteQuery(new __string(sqlQuery));
}


EXTERN_C __declspec(dllexport) void EndSqlQuery()
{
	Chronos::Agent::DotNet::SqlProfiler::ProfilingTypeAdapter::EndExecuteQuery();
}

