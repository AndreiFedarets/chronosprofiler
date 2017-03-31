#pragma once
#include "Chronos.DotNet.Agent.h"

#ifdef CHRONOS_DOTNET_BASICPROFILER_EXPORT_API
#define CHRONOS_DOTNET_BASICPROFILER_API __declspec(dllexport) 
#else
#define CHRONOS_DOTNET_BASICPROFILER_API __declspec(dllimport) 
#endif

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace BasicProfiler
			{
// ==================================================================================================================================================
				template<typename T>
				struct BasicUnitBase : public Chronos::Agent::UnitBase
				{
					public:
						BasicUnitBase<T>() : _metadata(null)
						{
						}
						void InitializeMetadata(Reflection::RuntimeMetadataProvider* metadataProvider)
						{
							_metadataProvider = metadataProvider;
						}
						void PrepareClose()
						{
							if (_name == null)
							{
								_name = new __string(*(GetMetadata()->GetName()));
							}
						}
						T* GetMetadata()
						{
							if (_metadata == null)
							{
								_metadata = GetMetadataInternal();
							}
							return _metadata;
						}
						__string* GetName()
						{
							if (GetIsAlive() && _name == null)
							{
								_name = new __string(*(GetMetadata()->GetName()));
							}
							return _name;
						}
						virtual void OnLoaded() = 0;
					protected:
						virtual T* GetMetadataInternal() = 0;
						T* _metadata;
						Reflection::RuntimeMetadataProvider* _metadataProvider;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_BASICPROFILER_API AppDomainInfo : public BasicUnitBase<Reflection::AppDomainMetadata>
				{
					public:
						AppDomainInfo();
						void OnLoaded();
					protected:
						Reflection::AppDomainMetadata* GetMetadataInternal();
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_BASICPROFILER_API AssemblyInfo : public BasicUnitBase<Reflection::AssemblyMetadata>
				{
					public:
						AssemblyInfo();
						void PrepareClose();
						AppDomainID GetAppDomainId();
						void OnLoaded();
					protected:
						Reflection::AssemblyMetadata* GetMetadataInternal();
					private:
						AppDomainID _appDomainId;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_BASICPROFILER_API ModuleInfo : public BasicUnitBase<Reflection::ModuleMetadata>
				{
					public:
						ModuleInfo();
						void PrepareClose();
						AssemblyID GetAssemblyId();
						void OnLoaded();
					protected:
						Reflection::ModuleMetadata* GetMetadataInternal();
					private:
						AssemblyID _assemblyId;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_BASICPROFILER_API ClassInfo : public BasicUnitBase<Reflection::TypeMetadata>
				{
					public:
						ClassInfo();
						void PrepareClose();
						ModuleID GetModuleId();
						mdTypeDef GetTypeToken();
						void OnLoaded();
					protected:
						Reflection::TypeMetadata* GetMetadataInternal();
					private:
						ModuleID _moduleId;
						mdTypeDef _typeToken;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_BASICPROFILER_API FunctionInfo : public BasicUnitBase<Reflection::MethodMetadata>
				{
					public:
						FunctionInfo();
						void PrepareClose();
						mdTypeDef GetTypeToken();
						ModuleID GetModuleId();
						AssemblyID GetAssemblyId();
						ClassID GetClassId();
						void OnLoaded();
					protected:
						Reflection::MethodMetadata* GetMetadataInternal();
					private:
						mdTypeDef _typeToken;
						AssemblyID _assemblyId;
						ModuleID _moduleId;
						ClassID _classId;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_BASICPROFILER_API ThreadInfo : public BasicUnitBase<Reflection::ThreadMetadata>
				{
					public:
						ThreadInfo();
						void PrepareClose();
						HANDLE GetThreadHandle();
						__uint GetOsThreadId();
						void OnLoaded();
					protected:
						Reflection::ThreadMetadata* GetMetadataInternal();
					private:
						HANDLE _handle;
						__uint _osThreadId;
				};

// ==================================================================================================================================================
				class UnitType
				{
					public:
						enum
						{
							Process = 1,
							AppDomain = 2,
							Assembly = 3,
							Module = 4,
							Class = 5,
							Function = 6,
							Thread = 7,
						};
				};
				
// ==================================================================================================================================================
				template<typename T>
				class BasicUnitCollectionBase : public DotNetUnitCollectionBase<T>
				{
					public:
						BasicUnitCollectionBase<T>()
						{
						}

						~BasicUnitCollectionBase<T>()
						{
						}
					
						void PrepareCloseUnit(__uptr id)
						{
							UnitContainer<T>* container = GetUnitContainer(id, true);
							T* unit = &(container->Unit);
							unit->PrepareClose();
						}
				};
		
// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API AppDomainCollection : public BasicUnitCollectionBase<AppDomainInfo>
				{
					public:
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(AppDomainInfo* unit);
				};
		
// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API AssemblyCollection : public BasicUnitCollectionBase<AssemblyInfo>
				{
					public:
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(AssemblyInfo* unit);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API ModuleCollection : public BasicUnitCollectionBase<ModuleInfo>
				{
					public:
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(ModuleInfo* unit);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API ClassCollection : public BasicUnitCollectionBase<ClassInfo>
				{
					public:
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(ClassInfo* unit);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API FunctionCollection : public BasicUnitCollectionBase<FunctionInfo>
				{
					public:
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(FunctionInfo* unit);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API ThreadCollection : public BasicUnitCollectionBase<ThreadInfo>
				{
					public:
						HRESULT GetCurrentThreadInfo(ThreadInfo** unit);
						const static __guid ServiceToken;
					protected:
						HRESULT InitializeUnitSpecial(ThreadInfo* unit);
					private:
						const static __string* MainThreadName;
						const static __string* FinalizationThreadName;
						const static __string* WorkerThreadName;
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API UnitMarshaler
				{
					public:
						static void MarshalAppDomain(AppDomainInfo* appDomainInfo, IStreamWriter* stream);
						static void MarshalAssembly(AssemblyInfo* assemblyInfo, IStreamWriter* stream);
						static void MarshalModule(ModuleInfo* moduleInfo, IStreamWriter* stream);
						static void MarshalClass(ClassInfo* classInfo, IStreamWriter* stream);
						static void MarshalFunction(FunctionInfo* functionInfo, IStreamWriter* stream);
						static void MarshalThread(ThreadInfo* threadInfo, IStreamWriter* stream);
				};

// ==================================================================================================================================================
				class CHRONOS_DOTNET_BASICPROFILER_API ProfilingTypeAdapter : public IProfilingTypeAdapter
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
						void FlushAppDomains(IStreamWriter* stream);
						void FlushAssemblies(IStreamWriter* stream);
						void FlushModules(IStreamWriter* stream);
						void FlushClasses(IStreamWriter* stream);
						void FlushFunctions(IStreamWriter* stream);
						void FlushThreads(IStreamWriter* stream);

						void OnAppDomainCreationStarted(void* eventArgs);
						void OnAppDomainCreationFinished(void* eventArgs);
						void OnAppDomainShutdownStarted(void* eventArgs);
						void OnAppDomainShutdownFinished(void* eventArgs);

						void OnAssemblyLoadStarted(void* eventArgs);
						void OnAssemblyLoadFinished(void* eventArgs);
						void OnAssemblyUnloadStarted(void* eventArgs);
						void OnAssemblyUnloadFinished(void* eventArgs);
						
						void OnModuleLoadStarted(void* eventArgs);
						void OnModuleLoadFinished(void* eventArgs);
						void OnModuleUnloadStarted(void* eventArgs);
						void OnModuleUnloadFinished(void* eventArgs);
						void OnModuleAttachedToAssembly(void* eventArgs);
						
						void OnClassLoadStarted(void* eventArgs);
						void OnClassLoadFinished(void* eventArgs);
						void OnClassUnloadStarted(void* eventArgs);
						void OnClassUnloadFinished(void* eventArgs);
						
						void OnFunctionLoadStarted(void* eventArgs);
						void OnFunctionUnloadStarted(void* eventArgs);
						
						void OnThreadCreated(void* eventArgs);
						void OnThreadDestroyed(void* eventArgs);
						void OnThreadNameChanged(void* eventArgs);
						
					private:
						AppDomainCollection* _appDomains;
						AssemblyCollection* _assemblies;
						ModuleCollection* _modules;
						ClassCollection* _classes;
						FunctionCollection* _functions;
						ThreadCollection* _threads;

						ProfilingEventsSubscription<ProfilingTypeAdapter>* _subscription;
						ProfilingTimer* _profilingTimer;
						Reflection::RuntimeMetadataProvider* _metadataProvider;
						GatewayClient* _gatewayClient;
						__byte _dataMarker;
				
				};
// ==================================================================================================================================================
			}
		}
	}
}