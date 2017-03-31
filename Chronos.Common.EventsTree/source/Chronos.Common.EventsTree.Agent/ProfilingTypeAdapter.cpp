#include "stdafx.h"
#include "Chronos.Common.EventsTree.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
			namespace EventsTree
			{
				ProfilingTypeAdapter::ProfilingTypeAdapter(void)
				{
					_settings = null;
				}

				ProfilingTypeAdapter::~ProfilingTypeAdapter(void)
				{
					__FREEOBJ(_loggers);
					//EventsTreeLoggerCollection::DestroyInstance();
				}
				
				HRESULT ProfilingTypeAdapter::BeginInitialize(ProfilingTypeSettings* settings)
				{
					//Initialize local settings
					_settings = settings;
					_loggers = new EventsTreeLoggerCollection();
					__RETURN_IF_FAILED( settings->GetDataMarker(&_dataMarker));

					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ExportServices(ServiceContainer* container)
				{
					__REGISTER_SERVICE(container, IEventsTreeLoggerCollection, _loggers);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ImportServices(ServiceContainer* container)
				{
					//Query shared services
					__RESOLVE_SERVICE(container, GatewayClient, _gatewayClient);
					__RESOLVE_SERVICE(container, ProfilingTimer, _profilingTimer);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::EndInitialize()
				{
					//Initialize local services
					_loggers->Initialize(_gatewayClient, _profilingTimer, _dataMarker, 250 * 1024, 10000);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::SubscribeEvents()
				{
					return S_OK;
				}
			
				HRESULT ProfilingTypeAdapter::FlushData()
				{
					return S_OK;
				}
			}
		}
	}
}
