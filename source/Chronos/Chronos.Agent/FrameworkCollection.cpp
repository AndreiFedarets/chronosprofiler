#include "StdAfx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		FrameworkCollection::FrameworkCollection()
		{
			_frameworks = new __vector<Framework*>();
			_frameworksSettings = null;
			_profilingTargetSettings = null;
		}

		FrameworkCollection::~FrameworkCollection()
		{
			for (__vector<Framework*>::iterator i = _frameworks->begin(); i != _frameworks->end(); ++i)
			{
				Framework* framework = *i;
				__FREEOBJ(framework);
			}
			_frameworks->clear();
			__FREEOBJ(_frameworks);
		}

		HRESULT FrameworkCollection::Initialize(FrameworkSettingsCollection* frameworksSettings, ProfilingTargetSettings* profilingTargetSettings,  ServiceContainer* container)
		{
			_frameworksSettings = frameworksSettings;
			_profilingTargetSettings = profilingTargetSettings;
			_container = container;
			return S_OK;
		}

		HRESULT FrameworkCollection::LoadExtensions()
		{
			//Load extensions
			std::vector<FrameworkSettings*> items = _frameworksSettings->GetItems();
			for (std::vector<FrameworkSettings*>::iterator item = items.begin(); item != items.end(); item++)
			{
				FrameworkSettings* frameworkSettings = *item;
				Framework* framework = new Framework(frameworkSettings, _profilingTargetSettings);
				HRESULT result =  framework->LoadAdapter();
				if (FAILED(result))
				{
					__FREEOBJ(framework);
					return result;
				}
				_frameworks->push_back(framework);
			}
			return S_OK;
		}

		HRESULT FrameworkCollection::BeginInitialize()
		{
			HRESULT result = S_OK;
			for (std::vector<Framework*>::iterator i = _frameworks->begin(); i != _frameworks->end(); ++i)
			{
				Framework* framework = *i;
				if (FAILED(framework->BeginInitialize()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT FrameworkCollection::ExportServices()
		{
			HRESULT result = S_OK;
			for (std::vector<Framework*>::iterator i = _frameworks->begin(); i != _frameworks->end(); ++i)
			{
				Framework* framework = *i;
				if (FAILED(framework->ExportServices(_container)))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT FrameworkCollection::ImportServices()
		{
			HRESULT result = S_OK;
			for (std::vector<Framework*>::iterator i = _frameworks->begin(); i != _frameworks->end(); ++i)
			{
				Framework* framework = *i;
				if (FAILED(framework->ImportServices(_container)))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT FrameworkCollection::EndInitialize()
		{
			HRESULT result = S_OK;
			for (std::vector<Framework*>::iterator i = _frameworks->begin(); i != _frameworks->end(); ++i)
			{
				Framework* framework = *i;
				if (FAILED(framework->EndInitialize()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT FrameworkCollection::SubscribeEvents()
		{
			HRESULT result = S_OK;
			std::stack<Framework*> frameworks;
			for (std::vector<Framework*>::iterator i = _frameworks->begin(); i != _frameworks->end(); ++i)
			{
				Framework* framework = *i;
				frameworks.push(framework);
			}
			while (!frameworks.empty())
			{
				Framework* framework = frameworks.top();
				frameworks.pop();
				if (FAILED(framework->SubscribeEvents()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT FrameworkCollection::FlushData()
		{
			HRESULT result = S_OK;
			for (std::vector<Framework*>::iterator i = _frameworks->begin(); i != _frameworks->end(); ++i)
			{
				Framework* framework = *i;
				if (FAILED(framework->FlushData()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}
	}
}