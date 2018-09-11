#pragma once
#include <Chronos/Chronos.Agent.h>
#include <Chronos.DotNet/Chronos.DotNet.Agent.h>
#include <Chronos.DotNet.BasicProfiler/Chronos.DotNet.BasicProfiler.Agent.h>

#ifdef CHRONOS_DOTNET_EXCEPTION_MONITOR_EXPORT_API
#define CHRONOS_DOTNET_EXCEPTION_MONITOR_API __declspec(dllexport) 
#else
#define CHRONOS_DOTNET_EXCEPTION_MONITOR_API __declspec(dllimport) 
#endif

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace ExceptionMonitor
			{
// ==================================================================================================================================================
				struct ManagedExceptionInfo : public Chronos::Agent::UnitBase
				{
					public:
						ManagedExceptionInfo();
						__string* GetMessage();
						ClassID GetExceptionClassId();
						__vector<FunctionID> GetStack();
						void InitializeSpecial(__string* message, ClassID exceptionClassId);
					private:
						__vector<FunctionID> _stack;
						ClassID _exceptionClassId;
				};
				
// ==================================================================================================================================================
				class UnitType
				{
					public:
						enum
						{
							ManagedException = 15,
						};
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_EXCEPTION_MONITOR_API UnitMarshaler
				{
					public:
						static void MarshalManagedException(ManagedExceptionInfo* managedExceptionInfo, IStreamWriter* stream);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_EXCEPTION_MONITOR_API ManagedExceptionCollection : public DotNetUnitCollectionBase<ManagedExceptionInfo>
				{
					public:
						ManagedExceptionCollection();
						const static __guid ServiceToken;
					private:
						HRESULT InitializeUnitSpecial(ManagedExceptionInfo* unit);
						HRESULT GetExceptionMessageField(ObjectID exceptionObjectId, ClassID* exceptionClass, mdFieldDef* fieldToken, IMetaDataImport2** metaDataImport);
						mdFieldDef _exceptionMessageFieldToken;
						IMetaDataImport2* _metaDataImport;
						ClassID _exceptionClassId;
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_EXCEPTION_MONITOR_API ProfilingTypeAdapter : public IProfilingTypeAdapter
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
						void FlushManagedExceptions(IStreamWriter* stream);

						void OnExceptionThrown(void* eventArgs);
						void OnExceptionCatcherEnter(void* eventArgs);

					private:
						ManagedExceptionCollection* _managedExceptions;
						ProfilingTimer* _profilingTimer;
						ProfilingEventsSubscription<ProfilingTypeAdapter>* _subscription;
						Chronos::Agent::DotNet::Reflection::RuntimeMetadataProvider* _metadataProvider;
						GatewayClient* _gatewayClient;
						__byte _dataMarker;
				};
				
// ==================================================================================================================================================
			}
		}
	}
}