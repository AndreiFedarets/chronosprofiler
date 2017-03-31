#include "StdAfx.h"
#include "ProfilerController.h"

CProfilerController::CProfilerController(void)
	: _settings(new CConfigurationSettings()), _callstackId(0), ProfileSql(false)
{
	_startTime = CTimer::CurrentTime;
    _processors = null;
    _storage = new CCallstackStorage();
}

CProfilerController::~CProfilerController(void)
{
	__FREEOBJ(_processors);
	__FREEOBJ(_settings);
	__FREEOBJ(_daemonClient);
}

CCallstackStorage* CProfilerController::GetCallstackStorage()
{
	return _storage;
}

CBaseStream* CProfilerController::CreateUnitStream(__uint unitType)
{
	return _daemonClient->QueryUnitStream(unitType);
}

CBaseStream* CProfilerController::CreateThreadStream(__uint threadId)
{
	return _daemonClient->QueryThreadStream();
}

#pragma warning(push) 
#pragma warning(disable:4482)
HRESULT CProfilerController::Initialize()
{
	//Get session token from environment variables
	std::wstring sessionToken = CEnvironmentVariables::Get(PROFILER_CONFIGURATION_TOKEN_VARIABLE);
	//Create host client
	CHostClient client;
	//Load session setting
	if (!client.GetConfigurationSettings(&sessionToken, _settings))
	{
		return E_FAIL;
	}
	IsEnabled = _settings->InitialState == CProfilerState::Profiling;
	//Check that current is target process
	std::wstring currentProcessName = CCurrentProcess::GetName();
	std::wstring targetProcessName = _settings->TargetProcessName;
	UseFastHooks = _settings->UseFastHooks;
	if (!CStringFormatter::Equals(&targetProcessName, &currentProcessName))
	{
		if (_settings->ProfileChildProcess)
		{
			//TODO: get parent process name
			/*std::wstring parentProcessName;
			if (!CStringFormatter::Equals(&targetProcessName, &parentProcessName))
			{
				return E_FAIL;
			}*/
		}
		else
		{
			return E_FAIL;
		}
	}
	std::wstring processTargetArguments = _settings->ProcessTargetArguments;
	if (!processTargetArguments.empty())
	{
		std::wstring processCurrentArguments = CCurrentProcess::GetArguments();
		if (!CStringFormatter::Contains(&processCurrentArguments, &processTargetArguments))
		{
			return E_FAIL;
		}
	}
	//Review events
	TargetEvents = _settings->EventsMask;

	LogAppDomains = (TargetEvents & (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_APPDOMAIN_LOADS) == (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_APPDOMAIN_LOADS;
	TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_APPDOMAIN_LOADS;
	LogAssemblies = (TargetEvents & (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_ASSEMBLY_LOADS) == (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_ASSEMBLY_LOADS;
	TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_ASSEMBLY_LOADS;
	LogThreads = (TargetEvents & (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_THREADS) == (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_THREADS;
	TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_THREADS;
	LogModules = (TargetEvents & (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_MODULE_LOADS) == (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_MODULE_LOADS;
	TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_MODULE_LOADS;
	LogClasses = (TargetEvents & (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_CLASS_LOADS) == (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_CLASS_LOADS;
	TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_CLASS_LOADS;
	LogFunctionCalls = (TargetEvents & (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_ENTERLEAVE) == (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_ENTERLEAVE;
	TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_ENTERLEAVE;

	if (LogFunctionCalls)
	{
		TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_EXCEPTIONS;
		TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_CLR_EXCEPTIONS;
		TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_FUNCTION_UNLOADS;

		//TargetEvents = TargetEvents | COR_PRF_MONITOR::COR_PRF_MONITOR_CODE_TRANSITIONS;
	}

	if (_settings->ProfileSql)
	{
		TargetEvents = TargetEvents | (DWORD)COR_PRF_MONITOR::COR_PRF_ENABLE_FUNCTION_ARGS;
		UseFastHooks = false;
		ProfileSql = true;
	}

	LogExceptions = (TargetEvents & (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_EXCEPTIONS) == (DWORD)COR_PRF_MONITOR::COR_PRF_MONITOR_EXCEPTIONS;

	IsEnabled = IsEnabled && LogFunctionCalls;

	//Query daemon token from server
	CAgentSettings* agentSettings = new CAgentSettings();
	if (!client.StartProfilingSession(&sessionToken, agentSettings))
	{
		return E_FAIL;
	}
	_daemonClient = new CDaemonClient(agentSettings->SessionToken);
	CallPageSize = agentSettings->CallPageSize;
	ThreadStreamsCount = agentSettings->ThreadStreamsCount;
	//Start callstack senders
	_processors = new CCallstackProcessorPool(ThreadStreamsCount, _storage, _daemonClient);
	//If profiling of child processes is not enabled - disable profiling to prevent child processes profiling
	if (!_settings->ProfileChildProcess)
	{
		CEnvironmentVariables::Remove(COR_ENABLE_PROFILING_VARIABLE);
	}
	return S_OK;
}
#pragma warning(pop)

__bool CProfilerController::IsAssemblyExcluded(std::wstring assemblyName)
{
	std::vector<std::wstring>* filterItems = _settings->FilterItems;
	for (size_t i = 0; i < filterItems->size(); i++)
	{
		std::wstring filterItem = filterItems->at(i);
		if (CStringFormatter::Equals(&filterItem, &assemblyName))
		{
			return _settings->IsExclusionType;
		}
	}
	return !_settings->IsExclusionType;
}

__uint CProfilerController::GenerateCallstackId()
{
	CLock lock(&_callstackMonitor);
	__uint callstackId = _callstackId;
	_callstackId++;
	return callstackId;
}