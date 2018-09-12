#include "stdafx.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		ProfilingTypeCollection::ProfilingTypeCollection()
		{
			_profilingTypes = new __vector<ProfilingType*>();
			_settings = null;
		}
		
		ProfilingTypeCollection::~ProfilingTypeCollection(void)
		{
			for (__vector<ProfilingType*>::iterator i = _profilingTypes->begin(); i != _profilingTypes->end(); ++i)
			{
				ProfilingType* profilingType = *i;
				__FREEOBJ(profilingType);
			}
			_profilingTypes->clear();
			__FREEOBJ(_profilingTypes);
		}

		HRESULT ProfilingTypeCollection::Initialize(ProfilingTypeSettingsCollection* settings, ServiceContainer* container)
		{
			_settings = settings;
			_container = container;
			return S_OK;
		}

		HRESULT ProfilingTypeCollection::LoadExtensions()
		{
			//Load extensions
			std::vector<ProfilingTypeSettings*> items = _settings->GetItems();
			for (std::vector<ProfilingTypeSettings*>::iterator item = items.begin(); item != items.end(); item++)
			{
				ProfilingTypeSettings* settings = *item;
				ProfilingType* profilingType = new ProfilingType(settings);
				HRESULT result =  profilingType->LoadAdapter();
				if (FAILED(result))
				{
					__FREEOBJ(profilingType);
					return result;
				}
				_profilingTypes->push_back(profilingType);
			}
			return S_OK;
		}

		HRESULT ProfilingTypeCollection::BeginInitialize()
		{
			HRESULT result = S_OK;
			for (std::vector<ProfilingType*>::iterator i = _profilingTypes->begin(); i != _profilingTypes->end(); ++i)
			{
				ProfilingType* profilingType = *i;
				if (FAILED(profilingType->BeginInitialize()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT ProfilingTypeCollection::ExportServices()
		{
			HRESULT result = S_OK;
			for (std::vector<ProfilingType*>::iterator i = _profilingTypes->begin(); i != _profilingTypes->end(); ++i)
			{
				ProfilingType* profilingType = *i;
				if (FAILED(profilingType->ExportServices(_container)))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT ProfilingTypeCollection::ImportServices()
		{
			HRESULT result = S_OK;
			for (std::vector<ProfilingType*>::iterator i = _profilingTypes->begin(); i != _profilingTypes->end(); ++i)
			{
				ProfilingType* profilingType = *i;
				if (FAILED(profilingType->ImportServices(_container)))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT ProfilingTypeCollection::EndInitialize()
		{
			HRESULT result = S_OK;
			for (std::vector<ProfilingType*>::iterator i = _profilingTypes->begin(); i != _profilingTypes->end(); ++i)
			{
				ProfilingType* profilingType = *i;
				if (FAILED(profilingType->EndInitialize()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT ProfilingTypeCollection::SubscribeEvents()
		{
			HRESULT result = S_OK;
			std::stack<ProfilingType*> profilingTypes;
			for (std::vector<ProfilingType*>::iterator i = _profilingTypes->begin(); i != _profilingTypes->end(); ++i)
			{
				ProfilingType* profilingType = *i;
				profilingTypes.push(profilingType);
			}
			while (!profilingTypes.empty())
			{
				ProfilingType* profilingType = profilingTypes.top();
				profilingTypes.pop();
				if (FAILED(profilingType->SubscribeEvents()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}

		HRESULT ProfilingTypeCollection::FlushData()
		{
			HRESULT result = S_OK;
			for (std::vector<ProfilingType*>::iterator i = _profilingTypes->begin(); i != _profilingTypes->end(); ++i)
			{
				ProfilingType* profilingType = *i;
				if (FAILED(profilingType->FlushData()))
				{
					result = E_FAIL;
				}
			}
			return result;
		}
	}
}