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
					//__debugbreak();
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
					__vector<__string> arguments;
					//_jitEvents = new FunctionJitEvent(L"System.Data", L"System.Data.SqlClient.SqlCommand", L"ExecuteReader", arguments, jitCallback);
					_jitEvents = new FunctionJitEvent(L"SqlConsole", L"SqlConsole.Program", L"Empty", arguments, jitCallback);
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
					_jitEvents->Subscribe();
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::FlushData()
				{
					//Gateway has limit on package size (3mb), so we split data in 2 packages to minimize its size:

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
					FunctionID functionId = temp->FunctionId;
					Reflection::MethodMetadata* methodMetadata = null;
					_metadataProvider->GetMethod(functionId, &methodMetadata);
					ModuleID moduleId = methodMetadata->GetModuleId();

					ICorProfilerInfo2* profilerInfo = null;
					_metadataProvider->GetCorProfilerInfo2(&profilerInfo);
					MethodInjector* injector = new MethodInjector(_metadataProvider);
					injector->Initialize(moduleId, L"Chronos.DotNet.SqlProfiler.Agent.dll", L"SQLHOOK", L"BeginSqlQuery", L"EndSqlQuery");
					injector->InjectById(temp->FunctionId);
				}
			}
		}
	}
}