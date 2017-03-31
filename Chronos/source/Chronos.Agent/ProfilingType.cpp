#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		ProfilingType::ProfilingType(ProfilingTypeSettings* settings)
		{
			_adapter = null;
			_settings = settings;
		}

		ProfilingType::~ProfilingType()
		{
			__FREEOBJ(_adapter);
		}

		HRESULT ProfilingType::LoadAdapter()
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
			FARPROC proc = GetProcAddress(module, "CreateChronosProfilingType");
			if (proc == null)
			{
				FreeLibrary(module);
				return E_FAIL;
			}
			CREATE_CHRONOS_PROFILING_TYPE activator = CREATE_CHRONOS_PROFILING_TYPE(proc);
			activator(&_adapter);
			return S_OK;
		}

		HRESULT ProfilingType::BeginInitialize()
		{
			if (_adapter != null)
			{
				return _adapter->BeginInitialize(_settings);
			}
			return S_OK;
		}

		HRESULT ProfilingType::ExportServices(ServiceContainer* container)
		{
			if (_adapter != null)
			{
				return _adapter->ExportServices(container);
			}
			return S_OK;
		}

		HRESULT ProfilingType::ImportServices(ServiceContainer* container)
		{
			if (_adapter != null)
			{
				return _adapter->ImportServices(container);
			}
			return S_OK;
		}

		HRESULT ProfilingType::EndInitialize()
		{
			if (_adapter != null)
			{
				return _adapter->EndInitialize();
			}
			return S_OK;
		}

		HRESULT ProfilingType::SubscribeEvents()
		{
			if (_adapter != null)
			{
				return _adapter->SubscribeEvents();
			}
			return S_OK;
		}

		HRESULT ProfilingType::FlushData()
		{
			if (_adapter != null)
			{
				return _adapter->FlushData();
			}
			return S_OK;
		}
	}
}