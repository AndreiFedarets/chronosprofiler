#pragma once
#include "Chronos.DotNet/Chronos.DotNet.Agent.h"

#ifdef CHRONOS_DOTNET_SQLPROFILER_EXPORT_API
#define CHRONOS_DOTNET_SQLPROFILER_API __declspec(dllexport) 
#else
#define CHRONOS_DOTNET_SQLPROFILER_API __declspec(dllimport) 
#endif

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace SqlProfiler
			{
// ==================================================================================================================================================
				class UnitType
				{
					public:
						enum
						{
							MsSqlSquery = 10,
						};
				};

// ==================================================================================================================================================
				struct MsSqlQueryInfo : public Chronos::Agent::UnitBase
				{
					public:
						MsSqlQueryInfo();
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_SQLPROFILER_API MsSqlQueryCollection : public DotNetUnitCollectionBase<MsSqlQueryInfo>
				{
					public:
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(MsSqlQueryInfo* unit);
				};
				
// ==================================================================================================================================================
				class CHRONOS_DOTNET_SQLPROFILER_API UnitMarshaler
				{
					public:
						static void MarshalMsSqlQuery(MsSqlQueryInfo* sqlQueryInfo, IStreamWriter* stream);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_SQLPROFILER_API ProfilingTypeAdapter : public IProfilingTypeAdapter
				{
					public:
						ProfilingTypeAdapter(void);
						~ProfilingTypeAdapter(void);
						HRESULT BeginInitialize(ProfilingTypeSettings* settings);
						HRESULT ExportServices(ServiceContainer* container);
						HRESULT ImportServices(ServiceContainer* container);
						HRESULT EndInitialize();
						HRESULT SubscribeEvents();
						HRESULT FlushData();
				
					private:
						void FlushMsSqlQueries(IStreamWriter* stream);
						void OnJITCompilationStarted(void* functionObject);

					private:
						MsSqlQueryCollection* _msSqlQueries;
						FunctionsJitEvents* _jitEvents;
						ProfilingTimer* _profilingTimer;
						Reflection::RuntimeMetadataProvider* _metadataProvider;
						GatewayClient* _gatewayClient;
						__byte _dataMarker;
				};
			}
		}
	}
}