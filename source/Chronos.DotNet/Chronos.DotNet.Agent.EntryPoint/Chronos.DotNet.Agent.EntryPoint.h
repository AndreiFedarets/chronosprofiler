#pragma once
#include <Chronos.DotNet\Chronos.DotNet.Agent.h>

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace EntryPoint
			{
// ==================================================================================================================================================

				class AgentResolver
				{
					public:
						AgentResolver();
						~AgentResolver();
					private:
						__string GetVariable(__string variableName);
						void SetVariable(__string variableName, __string variableValue);
						void SetupAgentPath();
				};

// ==================================================================================================================================================

				template<typename T>
				class ThreadScopeDictionary
				{
				public:
					ThreadScopeDictionary<T>(void)
						: _metadataProvider(null)
					{
					}

					~ThreadScopeDictionary<T>(void)
					{
						//__FREEOBJ(_items);
					}

					void Initialize(Reflection::RuntimeMetadataProvider* metadataProvider)
					{
						_metadataProvider = metadataProvider;
					}

					HRESULT AttachItem(T item)
					{
						ThreadID threadId = 0;
						HRESULT result = _metadataProvider->GetCurrentThreadId(&threadId);
						__RETURN_IF_FAILED(result);
						CurrentItem = item;
						Lock lock(&_criticalSection);
						_items.insert(std::pair<UINT_PTR, T>(threadId, item));
						return S_OK;
					}

					HRESULT RemoveItem()
					{
						ThreadID threadId = 0;
						HRESULT result = _metadataProvider->GetCurrentThreadId(&threadId);
						__RETURN_IF_FAILED(result);
						Lock lock(&_criticalSection);
						std::map<UINT_PTR, T>::iterator i = _items.find(threadId);
						CurrentItem = null;
						if (i == _items.end())
						{
							return S_OK;
						}
						T item = i->second;
						__FREEOBJ(item);
						_items.erase(threadId);
						return S_OK;
					}

					T GetItem(ThreadID threadId)
					{
						Lock lock(&_criticalSection);
						std::map<UINT_PTR, T>::iterator i = _items.find(threadId);
						if (i == _items.end())
						{
							return null;
						}
						return i->second;
					}

					__declspec(thread) static T CurrentItem;
				private:
					std::map<UINT_PTR, T> _items;
					Reflection::RuntimeMetadataProvider* _metadataProvider;
					CriticalSection _criticalSection;
				};

				template<typename T>
				T ThreadScopeDictionary<T>::CurrentItem = null;

// ==================================================================================================================================================
				struct FunctionInfo
				{
					FunctionInfo()
					{
						FunctionId = 0;
						ClientData = null;
						HookFunction = false;
					}
					FunctionID FunctionId;
					UINT_PTR ClientData;
					__bool HookFunction;
				};

// ==================================================================================================================================================
				class FunctionInfoCollection
				{
					public:
						FunctionInfoCollection();
						~FunctionInfoCollection();
						FunctionInfo* FindFunction(FunctionID functionId);
						void RemoveFunction(FunctionID functionId);
						FunctionInfo* CreateFunction(FunctionID functionId, UINT_PTR clientData, __bool hookFunction);
					private:
						DynamicBlockFactory<FunctionInfo>* _functions;
				};
			
// ==================================================================================================================================================
				class FunctionExceptionTracer
				{
					public:
						FunctionExceptionTracer(RuntimeProfilingEvents* profilingEvents, FunctionInfoCollection* functions);
						~FunctionExceptionTracer(void);

						void ExceptionThrown(ObjectID objectId);

						void ExceptionSearchFunctionEnter(FunctionID functionId);
						void ExceptionSearchFunctionLeave();

						void ExceptionSearchCatcherFound(FunctionID functionId);

						void ExceptionUnwindFunctionEnter(FunctionID functionId);
						void ExceptionUnwindFunctionLeave();

						void ExceptionCatcherEnter(FunctionID functionId, ObjectID exceptionId);
						void ExceptionCatcherLeave();
					private:
						void RaiseFunctionExceptionHook(FunctionInfo* function);
					private:
						RuntimeProfilingEvents* _profilingEvents;
						FunctionInfoCollection* _functions;
						ObjectID _currentException;
						FunctionInfo* _currentFunction;
						FunctionInfo* _catcherFunction;
						std::queue<FunctionInfo*>* _exceptionStack;
				};
			}
// ==================================================================================================================================================
		}
	}
}