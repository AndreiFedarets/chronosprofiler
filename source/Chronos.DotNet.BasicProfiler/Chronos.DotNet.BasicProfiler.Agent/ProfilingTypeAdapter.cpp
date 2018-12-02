#include "stdafx.h"
#include "Chronos.DotNet.BasicProfiler.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace BasicProfiler
			{
				// APPDOMAIN HOOKS ============================================================================================
				void ProfilingTypeAdapter::OnAppDomainCreationStarted(void* eventArgs)
				{
					AppDomainCreationStartedEventArgs* temp = static_cast<AppDomainCreationStartedEventArgs*>(eventArgs);
					_appDomains->CreateUnit(temp->AppDomainId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AppDomainCreationStarted, eventArgs);
				}

				void ProfilingTypeAdapter::OnAppDomainCreationFinished(void* eventArgs)
				{
					AppDomainCreationFinishedEventArgs* temp = static_cast<AppDomainCreationFinishedEventArgs*>(eventArgs);
					AppDomainInfo* appDomainInfo = _appDomains->GetUnit(temp->AppDomainId);
					appDomainInfo->OnLoaded();
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AppDomainCreationFinished, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnAppDomainShutdownStarted(void* eventArgs)
				{
					AppDomainShutdownStartedEventArgs* temp = static_cast<AppDomainShutdownStartedEventArgs*>(eventArgs);
					_appDomains->PrepareCloseUnit(temp->AppDomainId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AppDomainShutdownStarted, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnAppDomainShutdownFinished(void* eventArgs)
				{
					AppDomainShutdownFinishedEventArgs* temp = static_cast<AppDomainShutdownFinishedEventArgs*>(eventArgs);
					_appDomains->CloseUnit(temp->AppDomainId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AppDomainShutdownFinished, eventArgs);
				}

				// ASSEMBLY HOOKS =============================================================================================
				void ProfilingTypeAdapter::OnAssemblyLoadStarted(void* eventArgs)
				{
					AssemblyLoadStartedEventArgs* temp = static_cast<AssemblyLoadStartedEventArgs*>(eventArgs);
					_assemblies->CreateUnit(temp->AssemblyId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyLoadStarted, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnAssemblyLoadFinished(void* eventArgs)
				{
					AssemblyLoadFinishedEventArgs* temp = static_cast<AssemblyLoadFinishedEventArgs*>(eventArgs);
					AssemblyInfo* assemblyInfo = _assemblies->GetUnit(temp->AssemblyId);
					assemblyInfo->OnLoaded();
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyLoadFinished, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnAssemblyUnloadStarted(void* eventArgs)
				{
					AssemblyUnloadStartedEventArgs* temp = static_cast<AssemblyUnloadStartedEventArgs*>(eventArgs);
					_assemblies->PrepareCloseUnit(temp->AssemblyId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyUnloadStarted, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnAssemblyUnloadFinished(void* eventArgs)
				{
					AssemblyUnloadFinishedEventArgs* temp = static_cast<AssemblyUnloadFinishedEventArgs*>(eventArgs);
					_assemblies->CloseUnit(temp->AssemblyId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyUnloadFinished, eventArgs);
				}

				// MODULE HOOKS ===============================================================================================
				void ProfilingTypeAdapter::OnModuleLoadStarted(void* eventArgs)
				{
					ModuleLoadStartedEventArgs* temp = static_cast<ModuleLoadStartedEventArgs*>(eventArgs);
					_modules->CreateUnit(temp->ModuleId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ModuleLoadStarted, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnModuleLoadFinished(void* eventArgs)
				{
					ModuleLoadFinishedEventArgs* temp = static_cast<ModuleLoadFinishedEventArgs*>(eventArgs);
					ModuleInfo* moduleInfo = _modules->GetUnit(temp->ModuleId);
					moduleInfo->OnLoaded();
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ModuleLoadFinished, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnModuleUnloadStarted(void* eventArgs)
				{
					ModuleUnloadStartedEventArgs* temp = static_cast<ModuleUnloadStartedEventArgs*>(eventArgs);
					_modules->PrepareCloseUnit(temp->ModuleId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ModuleUnloadStarted, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnModuleUnloadFinished(void* eventArgs)
				{
					ModuleUnloadFinishedEventArgs* temp = static_cast<ModuleUnloadFinishedEventArgs*>(eventArgs);
					_modules->CloseUnit(temp->ModuleId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ModuleUnloadFinished, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnModuleAttachedToAssembly(void* eventArgs)
				{
					ModuleAttachedToAssemblyEventArgs* temp = static_cast<ModuleAttachedToAssemblyEventArgs*>(eventArgs);
					_modules->UpdateUnit(temp->ModuleId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ModuleAttachedToAssembly, eventArgs);
				}

				// CLASS HOOKS ================================================================================================
				void ProfilingTypeAdapter::OnClassLoadStarted(void* eventArgs)
				{
					ClassLoadStartedEventArgs* temp = static_cast<ClassLoadStartedEventArgs*>(eventArgs);
					_classes->CreateUnit(temp->ClassId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ClassLoadStarted, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnClassLoadFinished(void* eventArgs)
				{
					ClassLoadFinishedEventArgs* temp = static_cast<ClassLoadFinishedEventArgs*>(eventArgs);
					ClassInfo* classInfo = _classes->GetUnit(temp->ClassId);
					classInfo->OnLoaded();
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ClassLoadFinished, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnClassUnloadStarted(void* eventArgs)
				{
					ClassUnloadStartedEventArgs* temp = static_cast<ClassUnloadStartedEventArgs*>(eventArgs);
					_classes->PrepareCloseUnit(temp->ClassId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ClassUnloadStarted, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnClassUnloadFinished(void* eventArgs)
				{
					ClassUnloadFinishedEventArgs* temp = static_cast<ClassUnloadFinishedEventArgs*>(eventArgs);
					_classes->CloseUnit(temp->ClassId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ClassUnloadFinished, eventArgs);
				}

				// FUNCTION HOOKS =============================================================================================
				void ProfilingTypeAdapter::OnFunctionLoadStarted(void* eventArgs)
				{
					FunctionLoadStartedEventArgs* temp = static_cast<FunctionLoadStartedEventArgs*>(eventArgs);
					FunctionInfo* functionInfo = _functions->CreateUnit(temp->FunctionId);
					functionInfo->OnLoaded();
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::FunctionLoadStarted, eventArgs);
					temp->ClientData = functionInfo;
				}

				void ProfilingTypeAdapter::OnFunctionUnloadStarted(void* eventArgs)
				{
					FunctionUnloadStartedEventArgs* temp = static_cast<FunctionUnloadStartedEventArgs*>(eventArgs);
					_functions->PrepareCloseUnit(temp->FunctionId);
					_functions->CloseUnit(temp->FunctionId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::FunctionUnloadStarted, eventArgs);
				}
		
				// THREAD HOOKS ===============================================================================================
				void ProfilingTypeAdapter::OnThreadCreated(void* eventArgs)
				{
					ThreadCreatedEventArgs* temp = static_cast<ThreadCreatedEventArgs*>(eventArgs);
					ThreadInfo* threadInfo = _threads->CreateUnit(temp->ThreadId);
					threadInfo->OnLoaded();
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ThreadCreated, eventArgs);
				}

				void ProfilingTypeAdapter::OnThreadDestroyed(void* eventArgs)
				{
					ThreadDestroyedEventArgs* temp = static_cast<ThreadDestroyedEventArgs*>(eventArgs);
					_threads->PrepareCloseUnit(temp->ThreadId);
					_threads->CloseUnit(temp->ThreadId);
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ThreadDestroyed, eventArgs);
				}
		
				void ProfilingTypeAdapter::OnThreadNameChanged(void* eventArgs)
				{
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::ThreadNameChanged, eventArgs);
				}

				// ProfilingTypeAdapter =======================================================================================
				ProfilingTypeAdapter::ProfilingTypeAdapter(void)
				{
					_appDomains = null;
					_assemblies = null;
					_modules = null;
					_classes = null;
					_functions = null;
					_threads = null;
					_subscription = null;
				}

				ProfilingTypeAdapter::~ProfilingTypeAdapter(void)
				{
					__FREEOBJ(_appDomains);
					__FREEOBJ(_assemblies);
					__FREEOBJ(_modules);
					__FREEOBJ(_classes);
					__FREEOBJ(_functions);
					__FREEOBJ(_threads);
					__FREEOBJ(_subscription);
				}

				HRESULT ProfilingTypeAdapter::BeginInitialize(ProfilingTypeSettings* settings)
				{
					__RETURN_IF_FAILED(settings->GetDataMarker(&_dataMarker));
					
					_appDomains = new AppDomainCollection();
					_assemblies = new AssemblyCollection();
					_modules = new ModuleCollection();
					_classes = new ClassCollection();
					_functions = new FunctionCollection();
					_threads = new ThreadCollection();

					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ExportServices(ServiceContainer* container)
				{
					__REGISTER_SERVICE(container, AppDomainCollection, _appDomains);
					__REGISTER_SERVICE(container, AssemblyCollection, _assemblies);
					__REGISTER_SERVICE(container, ModuleCollection, _modules);
					__REGISTER_SERVICE(container, ClassCollection, _classes);
					__REGISTER_SERVICE(container, FunctionCollection, _functions);
					__REGISTER_SERVICE(container, ThreadCollection, _threads);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ImportServices(ServiceContainer* container)
				{
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
					_appDomains->Initialize(_profilingTimer, _metadataProvider);
					_assemblies->Initialize(_profilingTimer, _metadataProvider);
					_modules->Initialize(_profilingTimer, _metadataProvider);
					_classes->Initialize(_profilingTimer, _metadataProvider);
					_functions->Initialize(_profilingTimer, _metadataProvider);
					_threads->Initialize(_profilingTimer, _metadataProvider);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::SubscribeEvents()
				{
					//Subscribe function events
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AppDomainCreationStarted, &ProfilingTypeAdapter::OnAppDomainCreationStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AppDomainCreationFinished, &ProfilingTypeAdapter::OnAppDomainCreationFinished);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AppDomainShutdownStarted, &ProfilingTypeAdapter::OnAppDomainShutdownStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AppDomainShutdownFinished, &ProfilingTypeAdapter::OnAppDomainShutdownFinished);
					
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyLoadStarted, &ProfilingTypeAdapter::OnAssemblyLoadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyLoadFinished, &ProfilingTypeAdapter::OnAssemblyLoadFinished);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyUnloadStarted, &ProfilingTypeAdapter::OnAssemblyUnloadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyUnloadFinished, &ProfilingTypeAdapter::OnAssemblyUnloadFinished);
					
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ModuleLoadStarted, &ProfilingTypeAdapter::OnModuleLoadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ModuleLoadFinished, &ProfilingTypeAdapter::OnModuleLoadFinished);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ModuleUnloadStarted, &ProfilingTypeAdapter::OnModuleUnloadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ModuleUnloadFinished, &ProfilingTypeAdapter::OnModuleUnloadFinished);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ModuleAttachedToAssembly, &ProfilingTypeAdapter::OnModuleAttachedToAssembly);
					
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ClassLoadStarted, &ProfilingTypeAdapter::OnClassLoadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ClassLoadFinished, &ProfilingTypeAdapter::OnClassLoadFinished);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ClassUnloadStarted, &ProfilingTypeAdapter::OnClassUnloadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ClassUnloadFinished, &ProfilingTypeAdapter::OnClassUnloadFinished);
					
					_subscription->SubscribeEvent(RuntimeProfilingEvents::FunctionLoadStarted, &ProfilingTypeAdapter::OnFunctionLoadStarted);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::FunctionUnloadStarted, &ProfilingTypeAdapter::OnFunctionUnloadStarted);
					
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ThreadCreated, &ProfilingTypeAdapter::OnThreadCreated);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ThreadDestroyed, &ProfilingTypeAdapter::OnThreadDestroyed);
					_subscription->SubscribeEvent(RuntimeProfilingEvents::ThreadNameChanged, &ProfilingTypeAdapter::OnThreadNameChanged);

					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::FlushData()
				{
					Chronos::Agent::UnitMarshaler::SendUnits(UnitType::Function, _functions, UnitMarshaler::MarshalFunction, _gatewayClient, _dataMarker);
					Chronos::Agent::UnitMarshaler::SendUnits(UnitType::Class, _classes, UnitMarshaler::MarshalClass, _gatewayClient, _dataMarker);
					Chronos::Agent::UnitMarshaler::SendUnits(UnitType::Module, _modules, UnitMarshaler::MarshalModule, _gatewayClient, _dataMarker);
					Chronos::Agent::UnitMarshaler::SendUnits(UnitType::Assembly, _assemblies, UnitMarshaler::MarshalAssembly, _gatewayClient, _dataMarker);
					Chronos::Agent::UnitMarshaler::SendUnits(UnitType::AppDomain, _appDomains, UnitMarshaler::MarshalAppDomain, _gatewayClient, _dataMarker);
					Chronos::Agent::UnitMarshaler::SendUnits(UnitType::Thread, _threads, UnitMarshaler::MarshalThread, _gatewayClient, _dataMarker);
					return S_OK;
				}
			}
		}
	}
}