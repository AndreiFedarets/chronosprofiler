#pragma once
#include <fstream>
#include "ConfigurationSettings.h"
#include "DaemonClient.h"
#include "EnvironmentVariables.h"
#include "HostClient.h"
#include "StringFormatter.h"
#include "CallstackProcessorPool.h"
#include "CallstackStorage.h"
#include "ProfilerState.h"
#include "Timer.h"

#define PROFILER_CONFIGURATION_TOKEN_VARIABLE L"CHRONOS_PROFILER_CONFIGURATION_TOKEN"
#define COR_ENABLE_PROFILING_VARIABLE L"COR_ENABLE_PROFILING"

class CProfilerController
{
public:
	CProfilerController(void);
	~CProfilerController(void);
	HRESULT Initialize();
	__bool IsAssemblyExcluded(std::wstring assemblyName);
	CBaseStream* CreateUnitStream(__uint unitType);
	CBaseStream* CreateThreadStream(__uint threadId);
	CCallstackStorage* GetCallstackStorage();
	__uint GenerateCallstackId();
	
	DWORD TargetEvents;
	__bool LogFunctionCalls;
	__bool LogExceptions;
	__bool LogAppDomains;
	__bool LogAssemblies;
	__bool LogModules;
	__bool LogClasses;
	__bool LogThreads;
	__uint CallPageSize;
	__bool IsEnabled;
	__bool UseFastHooks;
	__bool ProfileSql;
	__uint ThreadStreamsCount;
private:
	HRESULT LoadFilterItems();
private:
	CDaemonClient* _daemonClient;
	CConfigurationSettings* _settings;
	__uint _startTime;
	CCallstackProcessorPool* _processors;
	CCallstackStorage* _storage;
	__uint _callstackId;
	CMonitor _callstackMonitor;

};

