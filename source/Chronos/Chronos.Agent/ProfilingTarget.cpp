#include "StdAfx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		ProfilingTarget::ProfilingTarget(ProfilingTargetSettings* settings)
		{
			_adapter = null;
			_settings = settings;
		}

		ProfilingTarget::~ProfilingTarget()
		{
			__FREEOBJ(_adapter);
		}

		HRESULT ProfilingTarget::LoadAdapter()
		{
			__string agentDll;
			if (!_settings->GetAgentDll(&agentDll))
			{
				return E_FAIL;
			}
			if (agentDll.empty())
			{
				return S_OK;
			}
			HMODULE module = LoadLibrary(agentDll.c_str());
			if (module == 0)
			{
				return E_FAIL;
			}
			FARPROC proc = GetProcAddress(module, "CreateChronosProfilingTarget");
			if (proc == null)
			{
				FreeLibrary(module);
				return E_FAIL;
			}
			CREATE_CHRONOS_PROFILING_TARGET activator = CREATE_CHRONOS_PROFILING_TARGET(proc);
			activator(&_adapter);
			return S_OK;
		}

		HRESULT ProfilingTarget::BeginInitialize()
		{
			if (_adapter != null)
			{
				return _adapter->BeginInitialize(_settings);
			}
			return S_OK;
		}

		HRESULT ProfilingTarget::ExportServices(ServiceContainer* container)
		{
			if (_adapter != null)
			{
				return _adapter->ExportServices(container);
			}
			return S_OK;
		}

		HRESULT ProfilingTarget::ImportServices(ServiceContainer* container)
		{
			if (_adapter != null)
			{
				return _adapter->ImportServices(container);
			}
			return S_OK;
		}

		HRESULT ProfilingTarget::EndInitialize()
		{
			if (_adapter != null)
			{
				return _adapter->EndInitialize();
			}
			return S_OK;
		}

	}
}