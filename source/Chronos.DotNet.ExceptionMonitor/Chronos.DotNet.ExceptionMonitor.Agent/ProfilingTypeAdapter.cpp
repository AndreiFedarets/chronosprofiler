#include "stdafx.h"
#include "Chronos.DotNet.ExceptionMonitor.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace ExceptionMonitor
			{
				void ProfilingTypeAdapter::OnExceptionThrown(void* eventArgs)
				{
					ExceptionThrownEventArgs* temp = static_cast<ExceptionThrownEventArgs*>(eventArgs);
					ManagedExceptionInfo* unit = _managedExceptions->CreateUnit(temp->ExceptionId);
					_managedExceptions->CloseUnit(temp->ExceptionId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ExceptionThrown, eventArgs);
				}

				void ProfilingTypeAdapter::OnExceptionCatcherEnter(void* eventArgs)
				{
					//ExceptionCatcherEnterEventArgs* temp = static_cast<ExceptionCatcherEnterEventArgs*>(eventArgs);
					//_managedExceptions->CloseUnit(temp->ExceptionId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ExceptionCatcherEnter, eventArgs);
				}

				ProfilingTypeAdapter::ProfilingTypeAdapter(void)
				{
					_managedExceptions = null;
				}

				ProfilingTypeAdapter::~ProfilingTypeAdapter(void)
				{
					__FREEOBJ(_managedExceptions);
				}

				HRESULT ProfilingTypeAdapter::BeginInitialize(ProfilingTypeSettings* settings)
				{
					//Initialize local settings
					__RETURN_IF_FAILED(settings->GetDataMarker(&_dataMarker));
					_managedExceptions = new ManagedExceptionCollection();
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ExportServices(ServiceContainer* container)
				{
					__REGISTER_SERVICE(container, ManagedExceptionCollection, _managedExceptions);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ImportServices(ServiceContainer* container)
				{
					//Query shared services
					RuntimeProfilingEvents* profilingEvents;
					__RESOLVE_SERVICE(container, GatewayClient, _gatewayClient);
					__RESOLVE_SERVICE(container, RuntimeProfilingEvents, profilingEvents);
					__RESOLVE_SERVICE(container, Reflection::RuntimeMetadataProvider, _metadataProvider);
					__RESOLVE_SERVICE(container, ProfilingTimer, _profilingTimer);
					_subscription = new ProfilingEventsSubscription<ProfilingTypeAdapter>(this, profilingEvents);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::EndInitialize()
				{
					_managedExceptions->Initialize(_profilingTimer, _metadataProvider);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::SubscribeEvents()
				{
					//Subscribe function events
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ExceptionThrown, &ProfilingTypeAdapter::OnExceptionThrown);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ExceptionCatcherEnter, &ProfilingTypeAdapter::OnExceptionCatcherEnter);
					return S_OK;
				}
			
				HRESULT ProfilingTypeAdapter::FlushData()
				{
					GatewayPackage* package = GatewayPackage::CreateDynamic(_dataMarker);
					FlushManagedExceptions(package);
					_gatewayClient->Send(package, false);
					return S_OK;
				}

				
				void ProfilingTypeAdapter::FlushManagedExceptions(IStreamWriter* stream)
				{
					std::list<ManagedExceptionInfo*> updates;
					_managedExceptions->GetUpdates(&updates);
					Marshaler::MarshalInt(UnitType::ManagedException, stream);
					Marshaler::MarshalSize(updates.size(), stream);
					for (std::list<ManagedExceptionInfo*>::iterator i = updates.begin(); i != updates.end(); ++i)
					{
						ManagedExceptionInfo* unit = *i;
						UnitMarshaler::MarshalManagedException(unit, stream);
					}
				}
			}
		}
	}
}