#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			FunctionJitEvent::FunctionJitEvent(__string assemblyName, __string className, __string functionName, __uint argumentsCount, ICallback* callback)
			{
				_subscription = null;
				_metadataProvider = null;
				_assemblyName = new __string(assemblyName);
				_className = new __string(className);
				_functionName = new __string(functionName);
				_argumentsCount = argumentsCount;
				_callback = callback;
				_assemblies = new FunctionJitEvent::AssemblyCollection();
			}

			FunctionJitEvent::~FunctionJitEvent()
			{
				__FREEOBJ(_callback);
				__FREEOBJ(_assemblies);
				__FREEOBJ(_assemblyName);
				__FREEOBJ(_className);
				__FREEOBJ(_functionName);
			}
			
			void FunctionJitEvent::Initialize(RuntimeProfilingEvents* profilingEvents, Reflection::RuntimeMetadataProvider* metadataProvider)
			{
				_subscription = new ProfilingEventsSubscription<FunctionJitEvent>(this, profilingEvents);
				_metadataProvider = metadataProvider;
			}
			
			void FunctionJitEvent::Subscribe()
			{
				_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyLoadStarted, &FunctionJitEvent::OnAssemblyLoadStarted);
				_subscription->SubscribeEvent(RuntimeProfilingEvents::AssemblyUnloadStarted, &FunctionJitEvent::OnAssemblyUnloadStarted);
				_subscription->SubscribeEvent(RuntimeProfilingEvents::ModuleAttachedToAssembly, &FunctionJitEvent::OnModuleAttachedToAssembly);
				_subscription->SubscribeEvent(RuntimeProfilingEvents::JITCompilationStarted, &FunctionJitEvent::OnJITCompilationStarted);
				_subscription->SubscribeEvent(RuntimeProfilingEvents::JITCachedFunctionSearchStarted, &FunctionJitEvent::OnJITCachedFunctionSearchStarted);
			}

			void FunctionJitEvent::OnJITCachedFunctionSearchStarted(void* eventArgs)
			{
				JITCachedFunctionSearchStartedEventArgs* temp = static_cast<JITCachedFunctionSearchStartedEventArgs*>(eventArgs);
				Reflection::MethodMetadata* methodMetadata;
				__RETURN_VOID_IF_FAILED(_metadataProvider->GetMethod(temp->FunctionId, &methodMetadata));
				ModuleCollection* modules = _assemblies->Get(methodMetadata->GetAssemblyId());
				if (modules != null)
				{
					FunctionCollection* functions = modules->Get(methodMetadata->GetModuleId());
					if (functions != null && functions->Contains(methodMetadata->GetMethodToken()))
					{
						temp->UseCachedFunction = FALSE;
					}

				}
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCachedFunctionSearchStarted, eventArgs);
			}

			void FunctionJitEvent::OnAssemblyLoadStarted(void* eventArgs)
			{
				AssemblyLoadFinishedEventArgs* temp = static_cast<AssemblyLoadFinishedEventArgs*>(eventArgs);
				Reflection::AssemblyMetadata* assemblyMetadata = null;
				_metadataProvider->GetAssembly(temp->AssemblyId, &assemblyMetadata);
				if (_assemblyName->compare(*assemblyMetadata->GetName()) == 0)
				{
					_assemblies->Add(temp->AssemblyId);
				}
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyLoadStarted, eventArgs);
			}

			void FunctionJitEvent::OnAssemblyUnloadStarted(void* eventArgs)
			{
				AssemblyUnloadStartedEventArgs* temp = static_cast<AssemblyUnloadStartedEventArgs*>(eventArgs);
				_assemblies->Remove(temp->AssemblyId);
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyUnloadStarted, eventArgs);
			}

			void FunctionJitEvent::OnModuleAttachedToAssembly(void* eventArgs)
			{
				ModuleAttachedToAssemblyEventArgs* temp = static_cast<ModuleAttachedToAssemblyEventArgs*>(eventArgs);
				FunctionJitEvent::ModuleCollection* modules = _assemblies->Get(temp->AssemblyId);
				if (modules != null)
				{
					FunctionJitEvent::FunctionCollection* functions = modules->Add(temp->ModuleId);
					HRESULT result;
					ICorProfilerInfo2* profilerInfo = null;
					result = _metadataProvider->GetCorProfilerInfo2(&profilerInfo);

					IMetaDataImport2* metaDataImport;
					result = profilerInfo->GetModuleMetaData(temp->ModuleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &metaDataImport);
					mdTypeDef classToken = 0;
					result = metaDataImport->FindTypeDefByName(_className->c_str(), null, &classToken);

					HCORENUM phEnum = 0;
					const __byte methodsMaxCount = 16;
					ULONG methodsCount = 0;
					mdToken methods[methodsMaxCount];
					result = metaDataImport->EnumMembersWithName(&phEnum, classToken, _functionName->c_str(), methods, methodsMaxCount, &methodsCount);

					for (__uint j = 0; j < methodsCount; j++)
					{
						mdToken functionToken = methods[j];
						phEnum = 0;
						const __byte paramsMaxCount = 32;
						mdParamDef params[paramsMaxCount] { 0 };
						ULONG paramsCount = 0;
						result = metaDataImport->EnumParams(&phEnum, functionToken, params, paramsMaxCount, &paramsCount);
						__bool methodMatches = paramsCount == _argumentsCount;
						if (methodMatches)
						{
							functions->Add(functionToken);
							break;
						}
					}
				}
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::ModuleAttachedToAssembly, eventArgs);
			}

			void FunctionJitEvent::OnJITCompilationStarted(void* eventArgs)
			{
				JITCompilationStartedEventArgs* temp = static_cast<JITCompilationStartedEventArgs*>(eventArgs);
				Reflection::MethodMetadata* methodMetadata;
				__RETURN_VOID_IF_FAILED(_metadataProvider->GetMethod(temp->FunctionId, &methodMetadata));
				ModuleCollection* modules = _assemblies->Get(methodMetadata->GetAssemblyId());
				if (modules != null)
				{
					FunctionCollection* functions = modules->Get(methodMetadata->GetModuleId());
					if (functions != null && functions->Contains(methodMetadata->GetMethodToken()))
					{
						//all checks passed, call callback
						_callback->Call(eventArgs);
					}
				}
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCompilationStarted, eventArgs);
			}

//---------------------------------------------------------------------------------------------------------------------------------------------------
			FunctionJitEvent::AssemblyCollection::~AssemblyCollection()
			{
				for (std::map<AssemblyID, ModuleCollection*>::iterator i = _assemblies.begin(); i != _assemblies.end(); ++i)
				{
					ModuleCollection* modules = i->second;
					__FREEOBJ(modules);
				}
				_assemblies.clear();
			}

			FunctionJitEvent::ModuleCollection* FunctionJitEvent::AssemblyCollection::Add(AssemblyID assemblyId)
			{
				Lock lock(&_criticalSection);
				ModuleCollection* modules = new ModuleCollection();
				_assemblies.insert(std::pair<AssemblyID, ModuleCollection*>(assemblyId, modules));
				return modules;
			}

			FunctionJitEvent::ModuleCollection* FunctionJitEvent::AssemblyCollection::Get(AssemblyID assemblyId)
			{
				Lock lock(&_criticalSection);
				std::map<AssemblyID, FunctionJitEvent::ModuleCollection*>::iterator i = _assemblies.find(assemblyId);
				if (i == _assemblies.end())
				{
					return null;
				}
				return i->second;
			}

			void FunctionJitEvent::AssemblyCollection::Remove(AssemblyID assemblyId)
			{
				Lock lock(&_criticalSection);
				std::map<AssemblyID, FunctionJitEvent::ModuleCollection*>::iterator i = _assemblies.find(assemblyId);
				if (i != _assemblies.end())
				{
					ModuleCollection* modules = i->second;
					__FREEOBJ(modules);
				}
				_assemblies.erase(assemblyId);
			}

//---------------------------------------------------------------------------------------------------------------------------------------------------
			FunctionJitEvent::ModuleCollection::~ModuleCollection()
			{
				for (std::map<ModuleID, FunctionCollection*>::iterator i = _modules.begin(); i != _modules.end(); ++i)
				{
					FunctionCollection* functions = i->second;
					__FREEOBJ(functions);
				}
				_modules.clear();
			}

			FunctionJitEvent::FunctionCollection* FunctionJitEvent::ModuleCollection::Add(ModuleID moduleId)
			{
				Lock lock(&_criticalSection);
				FunctionCollection* functions = new FunctionCollection();
				_modules.insert(std::pair<ModuleID, FunctionCollection*>(moduleId, functions));
				return functions;
			}

			FunctionJitEvent::FunctionCollection* FunctionJitEvent::ModuleCollection::Get(ModuleID moduleId)
			{
				Lock lock(&_criticalSection);
				std::map<ModuleID, FunctionCollection*>::iterator i = _modules.find(moduleId);
				if (i == _modules.end())
				{
					return null;
				}
				return i->second;
			}

			void FunctionJitEvent::ModuleCollection::Remove(ModuleID moduleId)
			{
				Lock lock(&_criticalSection);
				std::map<ModuleID, FunctionJitEvent::FunctionCollection*>::iterator i = _modules.find(moduleId);
				if (i != _modules.end())
				{
					FunctionCollection* functions = i->second;
					__FREEOBJ(functions);
				}
				_modules.erase(moduleId);
			}
//---------------------------------------------------------------------------------------------------------------------------------------------------
			
			void FunctionJitEvent::FunctionCollection::Add(mdToken functionToken)
			{
				Lock lock(&_criticalSection);
				_functions.insert(std::pair<mdToken, mdToken>(functionToken, functionToken));
			}
			
			__bool FunctionJitEvent::FunctionCollection::Contains(mdToken functionToken)
			{
				Lock lock(&_criticalSection);
				std::map<mdToken, mdToken>::iterator i = _functions.find(functionToken);
				return i != _functions.end();
			}

			void FunctionJitEvent::FunctionCollection::Remove(mdToken functionToken)
			{
				Lock lock(&_criticalSection);
				_functions.erase(functionToken);
			}
		}
	}
}