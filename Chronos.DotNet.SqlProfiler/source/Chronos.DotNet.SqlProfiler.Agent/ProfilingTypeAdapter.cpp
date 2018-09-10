#include "stdafx.h"
#include "Chronos.DotNet.SqlProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace SqlProfiler
			{
				// ProfilingTypeAdapter =======================================================================================
				ProfilingTypeAdapter::ProfilingTypeAdapter(void)
				{
					__debugbreak();
					_jitEvents = null;
					_msSqlQueries = null;
				}

				ProfilingTypeAdapter::~ProfilingTypeAdapter(void)
				{
					__FREEOBJ(_msSqlQueries);
				}

				HRESULT ProfilingTypeAdapter::BeginInitialize(ProfilingTypeSettings* settings)
				{
					__RETURN_IF_FAILED(settings->GetDataMarker(&_dataMarker));
					_msSqlQueries = new MsSqlQueryCollection();
					//initialize jit hook
					ICallback* jitCallback = new ThisCallback<ProfilingTypeAdapter>(this, &ProfilingTypeAdapter::OnJITCompilationStarted);
					__vector<__string> functions;
					functions.push_back(L"ExecuteReader");
					_jitEvents = new FunctionsJitEvents(L"System.Data", L"System.Data.SqlClient.SqlCommand", functions, jitCallback);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ExportServices(ServiceContainer* container)
				{
					__REGISTER_SERVICE(container, MsSqlQueryCollection, _msSqlQueries);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ImportServices(ServiceContainer* container)
				{
					RuntimeProfilingEvents* profilingEvents;
					__RESOLVE_SERVICE(container, GatewayClient, _gatewayClient);
					__RESOLVE_SERVICE(container, RuntimeProfilingEvents, profilingEvents);
					__RESOLVE_SERVICE(container, Reflection::RuntimeMetadataProvider, _metadataProvider);
					__RESOLVE_SERVICE(container, ProfilingTimer, _profilingTimer);
					_jitEvents->Initialize(profilingEvents, _metadataProvider);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::EndInitialize()
				{
					_msSqlQueries->Initialize(_profilingTimer, _metadataProvider);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::SubscribeEvents()
				{
					//Subscribe SqlCommand functions JIT event
					//ICallback* callback = new ThisCallback<ProfilingTypeAdapter>(this, &ProfilingTypeAdapter::OnSqlCommandFunctionJitting);
					//_functions->SubscribeFunctionJitting(, L"ExecuteReader", callback);
					_jitEvents->Subscribe();
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::FlushData()
				{
					//Gateway has limit on package size (3mb), so we split data in 2 packages to minimize its size:

					//Package#1. AppDomains, Assemblies, Modules, Classes, Threads
					GatewayPackage* package = GatewayPackage::CreateDynamic(_dataMarker);

					FlushMsSqlQueries(package);
					_gatewayClient->Send(package, false);

					return S_OK;
				}

				void ProfilingTypeAdapter::FlushMsSqlQueries(IStreamWriter* stream)
				{
					std::list<MsSqlQueryInfo*> updates;
					_msSqlQueries->GetUpdates(&updates);
					Marshaler::MarshalInt(UnitType::MsSqlSquery, stream);
					Marshaler::MarshalSize(updates.size(), stream);
					for (std::list<MsSqlQueryInfo*>::iterator i = updates.begin(); i != updates.end(); ++i)
					{
						MsSqlQueryInfo* unit = *i;
						UnitMarshaler::MarshalMsSqlQuery(unit, stream);
					}
				}

				void ProfilingTypeAdapter::OnJITCompilationStarted(void* eventArgs)
				{
					JITCompilationStartedEventArgs* temp = static_cast<JITCompilationStartedEventArgs*>(eventArgs);
					//ICorProfilerInfo2* profilerInfo = null;
					//_managedProvider->QueryInterface(__uuidof(ICorProfilerInfo2), (void**)&profilerInfo);
				}
			}
		}
	}
}