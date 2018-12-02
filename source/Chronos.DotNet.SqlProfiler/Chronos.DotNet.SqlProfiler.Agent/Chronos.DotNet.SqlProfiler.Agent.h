#pragma once
#include "Chronos.DotNet/Chronos.DotNet.Agent.h"
#include "Chronos.Common.EventsTree/Chronos.Common.EventsTree.Agent.h"

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
				class EventType
				{
					public:
						static const __byte SqlQuery = 0x10;
				};
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
				struct CHRONOS_DOTNET_SQLPROFILER_API MsSqlQueryInfo : public Chronos::Agent::UnitBase
				{
					public:
						MsSqlQueryInfo();
						void InitializeName(__string* queryText);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_SQLPROFILER_API MsSqlQueryCollection : public DotNetUnitCollectionBase<MsSqlQueryInfo>
				{
					public:
						MsSqlQueryCollection();
						MsSqlQueryInfo* CreateUnit();
						MsSqlQueryInfo* FindQuery(__string* queryText, __bool create);
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(MsSqlQueryInfo* unit);
						volatile __uint _lastUid;
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
						static void BeginExecuteQuery(__string* queryText);
						static void EndExecuteQuery();
				
					private:
						//void FlushMsSqlQueries(IStreamWriter* stream);
						void OnJITCompilationStarted(void* functionObject);
						void OnBeginExecuteQuery(__string* queryText);
						void OnEndExecuteQuery();

					private:
						__bool _eventTreeAvailable;
						MsSqlQueryCollection* _msSqlQueries;
						FunctionJitEvent* _jitEvents;
						ProfilingTimer* _profilingTimer;
						Reflection::RuntimeMetadataProvider* _metadataProvider;
						GatewayClient* _gatewayClient;
						__byte _dataMarker;
						static ProfilingTypeAdapter* _current;
				};
			}
		}
	}
}