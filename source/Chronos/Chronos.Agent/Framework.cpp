#include "StdAfx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		Framework::Framework(FrameworkSettings* frameworkSettings, ProfilingTargetSettings* profilingTargetSettings)
		{
			_adapter = null;
			_frameworkSettings = frameworkSettings;
			_profilingTargetSettings = profilingTargetSettings;
		}

		Framework::~Framework()
		{
			__FREEOBJ(_adapter);
		}
		
		HRESULT Framework::LoadAdapter()
		{
			__string agentDll;
			if (!_frameworkSettings->GetAgentDll(&agentDll))
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
			FARPROC proc = GetProcAddress(module, "CreateChronosFramework");
			if (proc == null)
			{
				FreeLibrary(module);
				return E_FAIL;
			}
			CREATE_CHRONOS_FRAMEWORK activator = CREATE_CHRONOS_FRAMEWORK(proc);
			activator(&_adapter);
			return S_OK;
		}

		HRESULT Framework::BeginInitialize()
		{
			if (_adapter != null)
			{
				return _adapter->BeginInitialize(_frameworkSettings, _profilingTargetSettings);
			}
			return S_OK;
		}
		
		HRESULT Framework::ExportServices(ServiceContainer* container)
		{
			if (_adapter != null)
			{
				return _adapter->ExportServices(container);
			}
			return S_OK;
		}

		HRESULT Framework::ImportServices(ServiceContainer* container)
		{
			if (_adapter != null)
			{
				return _adapter->ImportServices(container);
			}
			return S_OK;
		}

		HRESULT Framework::EndInitialize()
		{
			if (_adapter != null)
			{
				return _adapter->EndInitialize();
			}
			return S_OK;
		}

		HRESULT Framework::SubscribeEvents()
		{
			if (_adapter != null)
			{
				return _adapter->SubscribeEvents();
			}
			return S_OK;
		}

		HRESULT Framework::FlushData()
		{
			if (_adapter != null)
			{
				return _adapter->FlushData();
			}
			return S_OK;
		}
	}
}