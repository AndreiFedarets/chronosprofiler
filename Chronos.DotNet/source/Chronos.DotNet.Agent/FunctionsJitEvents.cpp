#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			FunctionsJitEvents::FunctionsJitEvents(__string assemblyName, __string className, __vector<__string> functions, ICallback* callback)
			{
				_subscription = null;
				_assemblyName = new __string(assemblyName);
				_className = new __string(className);
				_callback = callback;
				_functions = new __vector<__string>();
				for (__vector<__string>::iterator i = functions.begin(); i != functions.end(); ++i)
				{
					__string function = *i;
					_functions->push_back(function);
				}

				_assemblies = new FunctionsJitEvents::AssemblyCollection();
			}

			FunctionsJitEvents::~FunctionsJitEvents()
			{
				__FREEOBJ(_assemblyName);
				__FREEOBJ(_className);
				__FREEOBJ(_callback);
				__FREEOBJ(_assemblies);
			}

			void FunctionsJitEvents::Initialize(RuntimeProfilingEvents* profilingEvents, Reflection::RuntimeMetadataProvider* metadataProvider)
			{
				_subscription = new ProfilingEventsSubscription<FunctionsJitEvents>(this, profilingEvents);
				_metadataProvider = metadataProvider;
			}
			
			void FunctionsJitEvents::Subscribe()
			{
				_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyLoadStarted, &FunctionsJitEvents::OnAssemblyLoadStarted);
				_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyUnloadStarted, &FunctionsJitEvents::OnAssemblyUnloadStarted);
				_subscription->SubscribeEvent(RuntimeProfilingEvents::ModuleAttachedToAssembly, &FunctionsJitEvents::OnModuleAttachedToAssembly);
				_subscription->SubscribeEvent(RuntimeProfilingEvents::JITCompilationStarted, &FunctionsJitEvents::OnJITCompilationStarted);
			}

			void FunctionsJitEvents::OnAssemblyLoadStarted(void* eventArgs)
			{
				AssemblyLoadFinishedEventArgs* temp = static_cast<AssemblyLoadFinishedEventArgs*>(eventArgs);
				Reflection::AssemblyMetadata* assemblyMetadata = null;
				_metadataProvider->GetAssembly(temp->AssemblyId, &assemblyMetadata);
				if (_assemblyName->compare(*assemblyMetadata->GetName()) == 0)
				{
					_assemblies->Add(temp->AssemblyId);
				}
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyLoadFinished, eventArgs);
			}

			void FunctionsJitEvents::OnAssemblyUnloadStarted(void* eventArgs)
			{
				AssemblyUnloadStartedEventArgs* temp = static_cast<AssemblyUnloadStartedEventArgs*>(eventArgs);
				_assemblies->Remove(temp->AssemblyId);
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyLoadFinished, eventArgs);
			}

			void FunctionsJitEvents::OnModuleAttachedToAssembly(void* eventArgs)
			{
				ModuleAttachedToAssemblyEventArgs* temp = static_cast<ModuleAttachedToAssemblyEventArgs*>(eventArgs);
				FunctionsJitEvents::ModuleCollection* modules = _assemblies->Get(temp->AssemblyId);
				if (modules != null)
				{
					FunctionsJitEvents::FunctionCollection* functions = modules->Add(temp->ModuleId);
					HRESULT result;
					ICorProfilerInfo2* profilerInfo = null;
					result = _metadataProvider->GetCorProfilerInfo2(&profilerInfo);

					IMetaDataImport2* metaDataImport;
					result = profilerInfo->GetModuleMetaData(temp->ModuleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &metaDataImport);
					mdTypeDef classToken = 0;
					result = metaDataImport->FindTypeDefByName(_className->c_str(), null, &classToken);

					for (__vector<__string>::iterator i = _functions->begin(); i != _functions->end(); ++i)
					{
						HCORENUM phEnum = 0;
						__string functionName = *i;
						const __byte methodsMaxCount = 16;
						ULONG methodsCount = 0;
						mdToken methods[methodsMaxCount];
						result = metaDataImport->EnumMembersWithName(&phEnum, classToken, functionName.c_str(), methods, methodsMaxCount, &methodsCount);

						for (__uint j = 0; j < methodsCount; j++)
						{
							mdToken functionToken = methods[j];
							phEnum = 0;
							const __byte paramsMaxCount = 16;
							ULONG paramsCount = 0;
							mdToken params[paramsMaxCount];
							result = metaDataImport->EnumParams(&phEnum, functionToken, params, paramsMaxCount, &paramsCount);
							
							for (__uint k = 0; k < methodsCount; j++)
							{
								//metaDataImport->GetParamProps(
							}
						}
						
					}
				}
			}

			void FunctionsJitEvents::OnJITCompilationStarted(void* eventArgs)
			{
				JITCompilationStartedEventArgs* temp = static_cast<JITCompilationStartedEventArgs*>(eventArgs);

				AssemblyID assemblyId = 0;
				ModuleID moduleId = 0;
				ClassID classId = 0;
				mdToken functionToken = 0;

				Reflection::MethodMetadata* methodMetadata;
				_metadataProvider->GetMethod(temp->FunctionId, &methodMetadata);

				FunctionsJitEvents::ModuleCollection* modules = _assemblies->Get(methodMetadata->GetAssemblyId());
				if (modules != null)
				{
					FunctionsJitEvents::FunctionCollection* functions = modules->Get(moduleId);
					if (functions != null)
					{
						if (functions->Contains(functionToken))
						{
							_callback->Call(eventArgs);
						}
					}
				}

				_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCompilationStarted, eventArgs);
			}


//---------------------------------------------------------------------------------------------------------------------------------------------------
			FunctionsJitEvents::AssemblyCollection::~AssemblyCollection()
			{
				for (std::map<AssemblyID, ModuleCollection*>::iterator i = _assemblies.begin(); i != _assemblies.end(); ++i)
				{
					ModuleCollection* modules = i->second;
					__FREEOBJ(modules);
				}
				_assemblies.clear();
			}

			FunctionsJitEvents::ModuleCollection* FunctionsJitEvents::AssemblyCollection::Add(AssemblyID assemblyId)
			{
				Lock lock(&_criticalSection);
				ModuleCollection* modules = new ModuleCollection();
				_assemblies.insert(std::pair<AssemblyID, ModuleCollection*>(assemblyId, modules));
				return modules;
			}

			FunctionsJitEvents::ModuleCollection* FunctionsJitEvents::AssemblyCollection::Get(AssemblyID assemblyId)
			{
				Lock lock(&_criticalSection);
				std::map<AssemblyID, FunctionsJitEvents::ModuleCollection*>::iterator i = _assemblies.find(assemblyId);
				if (i == _assemblies.end())
				{
					return null;
				}
				return i->second;
			}

			void FunctionsJitEvents::AssemblyCollection::Remove(AssemblyID assemblyId)
			{
				Lock lock(&_criticalSection);
				std::map<AssemblyID, FunctionsJitEvents::ModuleCollection*>::iterator i = _assemblies.find(assemblyId);
				if (i != _assemblies.end())
				{
					ModuleCollection* modules = i->second;
					__FREEOBJ(modules);
				}
				_assemblies.erase(assemblyId);
			}

//---------------------------------------------------------------------------------------------------------------------------------------------------
			FunctionsJitEvents::ModuleCollection::~ModuleCollection()
			{
				for (std::map<ModuleID, FunctionCollection*>::iterator i = _modules.begin(); i != _modules.end(); ++i)
				{
					FunctionCollection* functions = i->second;
					__FREEOBJ(functions);
				}
				_modules.clear();
			}

			FunctionsJitEvents::FunctionCollection* FunctionsJitEvents::ModuleCollection::Add(ModuleID moduleId)
			{
				Lock lock(&_criticalSection);
				FunctionCollection* functions = new FunctionCollection();
				_modules.insert(std::pair<ModuleID, FunctionCollection*>(moduleId, functions));
				return functions;
			}

			FunctionsJitEvents::FunctionCollection* FunctionsJitEvents::ModuleCollection::Get(ModuleID moduleId)
			{
				Lock lock(&_criticalSection);
				std::map<ModuleID, FunctionCollection*>::iterator i = _modules.find(moduleId);
				if (i == _modules.end())
				{
					return null;
				}
				return i->second;
			}

			void FunctionsJitEvents::ModuleCollection::Remove(ModuleID moduleId)
			{
				Lock lock(&_criticalSection);
				std::map<ModuleID, FunctionsJitEvents::FunctionCollection*>::iterator i = _modules.find(moduleId);
				if (i != _modules.end())
				{
					FunctionCollection* functions = i->second;
					__FREEOBJ(functions);
				}
				_modules.erase(moduleId);
			}
//---------------------------------------------------------------------------------------------------------------------------------------------------
			
			void FunctionsJitEvents::FunctionCollection::Add(mdToken functionToken)
			{
				Lock lock(&_criticalSection);
				_functions.insert(std::pair<mdToken, mdToken>(functionToken, functionToken));
			}
			
			__bool FunctionsJitEvents::FunctionCollection::Contains(mdToken functionToken)
			{
				Lock lock(&_criticalSection);
				std::map<mdToken, mdToken>::iterator i = _functions.find(functionToken);
				return i != _functions.end();
			}

			void FunctionsJitEvents::FunctionCollection::Remove(mdToken functionToken)
			{
				Lock lock(&_criticalSection);
				_functions.erase(functionToken);
			}
		}
	}
}