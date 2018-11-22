#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			FunctionJitEvent::FunctionJitEvent(__string assemblyName, __string className, __string functionName, __vector<__string> arguments, ICallback* callback)
			{
				_subscription = null;
				_metadataProvider = null;
				_assemblyName = new __string(assemblyName);
				_className = new __string(className);
				_functionName = new __string(functionName);
				_arguments = new __vector<__string>();
				for (__vector<__string>::iterator i = arguments.begin(); i != arguments.end(); ++i)
				{
					__string argument = *i;
					_arguments->push_back(argument);
				}
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
				__FREEOBJ(_arguments);
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
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyLoadFinished, eventArgs);
			}

			void FunctionJitEvent::OnAssemblyUnloadStarted(void* eventArgs)
			{
				AssemblyUnloadStartedEventArgs* temp = static_cast<AssemblyUnloadStartedEventArgs*>(eventArgs);
				_assemblies->Remove(temp->AssemblyId);
				_subscription->RaiseNextEvent(RuntimeProfilingEvents::AssemblyLoadFinished, eventArgs);
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
						__bool methodMatches = false;
						mdToken functionToken = methods[j];
						phEnum = 0;
						const __byte paramsMaxCount = 16;
						ULONG paramsCount = 0;
						mdToken params[paramsMaxCount];
						result = metaDataImport->EnumParams(&phEnum, functionToken, params, paramsMaxCount, &paramsCount);
						if (_arguments->size() == paramsCount)
						{
							methodMatches = true;
							for (__uint k = 0; k < paramsCount; k++)
							{
								mdParamDef paramToken = params[k];
								mdMethodDef methodToken = 0;
								ULONG pulSequence = 0;
								const __byte paramNameMaxLength = 255;
								__wchar paramName[paramNameMaxLength];
								memset(&paramName, 0, sizeof(__wchar) * paramNameMaxLength);
								ULONG paramNameLength = 0;
								DWORD attributes = 0;
								DWORD typeFlags = 0;
								UVCP_CONSTANT uvcp = 0;
								ULONG uvcpLenght = 0;
								result = metaDataImport->GetParamProps(paramToken, &methodToken, &pulSequence, (LPWSTR)&paramName, paramNameMaxLength, &paramNameLength, &attributes, &typeFlags, &uvcp, &uvcpLenght);
								__int argumentIndex = pulSequence - 1;
								__string argument = _arguments->at(argumentIndex);
								if (argument.compare(paramName) != 0)
								{
									methodMatches = false;
									break;
								}
							}
						}
						if (methodMatches)
						{
							functions->Add(functionToken);
							break;
						}
					}
				}
			}

			void FunctionJitEvent::OnJITCompilationStarted(void* eventArgs)
			{
				JITCompilationStartedEventArgs* temp = static_cast<JITCompilationStartedEventArgs*>(eventArgs);

				Reflection::MethodMetadata* methodMetadata;
				__RETURN_VOID_IF_FAILED( _metadataProvider->GetMethod(temp->FunctionId, &methodMetadata) );

				//check assembly
				AssemblyID assemblyId = methodMetadata->GetAssemblyId();
				Reflection::AssemblyMetadata* assemblyMetadata;
				__RETURN_VOID_IF_FAILED( _metadataProvider->GetAssembly(assemblyId, &assemblyMetadata) );
				if (_assemblyName->compare(*assemblyMetadata->GetName()) != 0)
				{
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCompilationStarted, eventArgs);
					return;
				}

				//check class
				ClassID classId = methodMetadata->GetTypeId();
				Reflection::TypeMetadata* typeMetadata;
				__RETURN_VOID_IF_FAILED(_metadataProvider->GetType(classId, &typeMetadata));
				if (_className->compare(*typeMetadata->GetName()) != 0)
				{
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCompilationStarted, eventArgs);
					return;
				}

				//check function
				if (_functionName->compare(*methodMetadata->GetName()) != 0)
				{
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCompilationStarted, eventArgs);
					return;
				}
				
				//check parameters
				HRESULT result;
				ModuleID moduleId = methodMetadata->GetModuleId();
				mdToken functionToken = methodMetadata->GetMethodToken();

				ICorProfilerInfo2* profilerInfo = null;
				result = _metadataProvider->GetCorProfilerInfo2(&profilerInfo);

				IMetaDataImport2* metaDataImport;
				result = profilerInfo->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**)&metaDataImport);

				HCORENUM phEnum = 0;
				const __byte paramsMaxCount = 16;
				ULONG paramsCount = 0;
				mdToken params[paramsMaxCount];
				result = metaDataImport->EnumParams(&phEnum, functionToken, params, paramsMaxCount, &paramsCount);
				if (_arguments->size() != paramsCount)
				{
					_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCompilationStarted, eventArgs);
					return;
				}
				for (__uint k = 0; k < paramsCount; k++)
				{
					mdParamDef paramToken = params[k];
					mdMethodDef methodToken = 0;
					ULONG pulSequence = 0;
					const __byte paramNameMaxLength = 255;
					__wchar paramName[paramNameMaxLength];
					memset(&paramName, 0, sizeof(__wchar) * paramNameMaxLength);
					ULONG paramNameLength = 0;
					DWORD attributes = 0;
					DWORD typeFlags = 0;
					UVCP_CONSTANT uvcp = 0;
					ULONG uvcpLenght = 0;
					result = metaDataImport->GetParamProps(paramToken, &methodToken, &pulSequence, (LPWSTR)&paramName, paramNameMaxLength, &paramNameLength, &attributes, &typeFlags, &uvcp, &uvcpLenght);
					__int argumentIndex = pulSequence - 1;
					__string argument = _arguments->at(argumentIndex);
					if (argument.compare(paramName) != 0)
					{
						_subscription->RaiseNextEvent(RuntimeProfilingEvents::JITCompilationStarted, eventArgs);
						return;
					}
				}

				//all checks passed, call callback
				_callback->Call(eventArgs);
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