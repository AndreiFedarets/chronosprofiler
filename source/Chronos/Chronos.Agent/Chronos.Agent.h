#pragma once
#define __FREEOBJ(variable) if (variable != null) { delete variable; variable = null; }
#define __FREEARR(variable) if (variable != null) { delete[] variable; variable = null; }
#ifdef _DEBUG
	#define __ASSERT(expression, message) { if (!(expression)) { if (MessageBox(null, message, L"Assertion failed - Attach?", MB_YESNO | MB_ICONERROR) == IDYES) { __debugbreak(); }; } }
#else
	#define __ASSERT(expression, message) { }
#endif
#define __RETURN_IF_FAILED(action) { { HRESULT __resultValue__ = action; if (FAILED(__resultValue__)) { return __resultValue__; } } }
#define __RETURN_VOID_IF_FAILED(action) { { HRESULT __resultValue__ = action; if (FAILED(__resultValue__)) { return; } } }
#define __RETURN_NULL_IF_FAILED(action) { { HRESULT __resultValue__ = action; if (FAILED(__resultValue__)) { return null; } } }

//#define __ASSERT(expression, message) { }
#define __RESOLVE_SERVICE(CONTAINER, TYPE, INSTANCE) { if (!CONTAINER->ResolveService(TYPE::ServiceToken, (void**)&INSTANCE)) { return E_FAIL; } }
#define __WEAK_RESOLVE_SERVICE(CONTAINER, TYPE, INSTANCE) { if (!CONTAINER->ResolveService(Chronos::Agent::Converter::ConvertStringToGuid(TYPE ## ServiceToken), (void**)&INSTANCE)) { return E_FAIL; } }
#define __REGISTER_SERVICE(CONTAINER, TYPE, INSTANCE) { if (!CONTAINER->RegisterService(TYPE::ServiceToken, INSTANCE)) { return E_FAIL; } }

#define __short __int16
#define __int __int32
#define __long __int64
#define __byte unsigned __int8
#define __ushort unsigned __int16
#define __uint unsigned __int32
#define __size __int32
#define __ulong unsigned __int64
#define __wchar wchar_t
#define __bool bool
#define __string std::wstring
#define __guid GUID
#define __uptr UINT_PTR
#define __vector std::vector
#define __map std::map
#define null 0

#pragma once
#include <Windows.h>
#include <Guiddef.h>
#include <string>
#include <vector>
#include <queue>
#include <map>
#include <list>
#include <stack>

inline bool operator < (const GUID & left, const GUID & right)
{
	return memcmp(&left, &right, sizeof(GUID)) < 0;
}

#define ENVIRONMENT_VARIABLE_MAX_SIZE 32767
#define PROFILER_CONFIGURATION_TOKEN_VARIABLE L"CHRONOS_PROFILER_CONFIGURATION_TOKEN"

#ifdef CHRONOS_EXPORT_API
#define CHRONOS_API __declspec(dllexport) 
#else
#define CHRONOS_API __declspec(dllimport) 
#endif


namespace Chronos
{
	namespace Agent
	{
// ==================================================================================================================================================
		template<typename T1, typename T2>
		struct Tuple
		{
			Tuple<T1, T2>()
			{
			}

			Tuple<T1, T2>(T1 item1, T2 item2)
			{
				Item1 = item1;
				Item2 = item2;
			}

			T1 Item1;
			T2 Item2;
		};
		
// ==================================================================================================================================================
		struct CHRONOS_API IDisposable
		{
			virtual ~IDisposable() { }
			virtual void Dispose() = 0;
		};

// ==================================================================================================================================================
		template<typename T>
		struct InterlockedContainer
		{
			InterlockedContainer<T>()
			{
				Value = null;
			}

			~InterlockedContainer()
			{
				Value = null;
			}

			T* SetValue(T* value)
			{
				T* temp = (T*)InterlockedExchangePointer((PVOID*)&(Value), (PVOID)value);
				return temp;
			}

			volatile T* Value;
		};

// ==================================================================================================================================================
		struct CHRONOS_API ICallback
		{
			virtual ~ICallback() { }
			virtual void Call(void* parameter) = 0;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API SimpleCallback : public virtual ICallback
		{
			public:
				SimpleCallback(void (*callbackFunction)(void*));
				SimpleCallback(SimpleCallback& callback);
				void Call(void* parameter);
			private:
				void (*_callbackFunction)(void*);
		};

// ==================================================================================================================================================
		template<typename T>
		class ThisCallback : public virtual ICallback
		{
			public:
				ThisCallback<T>(T* thisObject, void (T::*callbackFunction)(void*))
				{
					_thisObject = thisObject;
					_callbackFunction = callbackFunction;
				}
				ThisCallback<T>(ThisCallback<T>& callback)
				{
					_thisObject = callback._thisObject;
					_callbackFunction = callback._callbackFunction;
				}
				void Call(void* parameter)
				{
					(_thisObject->*_callbackFunction)(parameter);
				}
			private:
				T* _thisObject;
				void (T::*_callbackFunction)(void*);
		};

// ==================================================================================================================================================
		template<typename T>
		struct IEnumerator
		{
			IEnumerator<T>() { }
			virtual ~IEnumerator<T>() { }
			virtual void MoveNext() { }
			T* Current;
		};

// ==================================================================================================================================================
		struct CHRONOS_API Buffer
		{
			Buffer();
			Buffer(__byte* data, __uint size);
			Buffer(__uint size);
			~Buffer();
			Buffer* Clone();
			__byte* Data;
			__uint Size;
		};

// ==================================================================================================================================================
		class CHRONOS_API Path
		{
			public:
				static __string GetFileName(__string path);
				static __string Combine(__string path1, __string path2);
		};

// ==================================================================================================================================================
		class CHRONOS_API CriticalSection
		{
			public:
				CriticalSection(void);
				~CriticalSection(void);
				void Enter();
				void Leave();
			private:
				CRITICAL_SECTION _criticalSection;
		};

// ==================================================================================================================================================
		class CHRONOS_API Lock
		{
			public:
				Lock(CriticalSection* criticalSection);
				~Lock(void);
			private:
				CriticalSection* _criticalSection;
		};

		
// ==================================================================================================================================================
		struct CHRONOS_API IThread
		{
			virtual ~IThread() { }
			virtual void Start(LPVOID parameter) = 0;
			virtual void Start() = 0;
			virtual void Stop() = 0;
			virtual __bool IsAlive() = 0;
			virtual __bool SetPriority(__int priority) = 0;
			virtual __bool SetPriorityClass(__int priorityClass) = 0;
			
			enum ThreadPriority
			{
				AboveNormalPriority = THREAD_PRIORITY_ABOVE_NORMAL,
				BelowNormalPriority = THREAD_PRIORITY_BELOW_NORMAL,
				HighestPriority = THREAD_PRIORITY_HIGHEST,
				IdlePriority = THREAD_PRIORITY_IDLE,
				LowestPriority = THREAD_PRIORITY_LOWEST,
				NormalPriority = THREAD_PRIORITY_NORMAL,
				TimeCriticalPriority = THREAD_PRIORITY_TIME_CRITICAL
			};
			
			enum ThreadPriorityClass
			{
				IdlePriorityClass = IDLE_PRIORITY_CLASS,
				BelowNormalPriorityClass = BELOW_NORMAL_PRIORITY_CLASS,
				NormalPriorityClass = NORMAL_PRIORITY_CLASS,
				AboveNormalPriorityClass = ABOVE_NORMAL_PRIORITY_CLASS,
				HighPriorityClass = HIGH_PRIORITY_CLASS,
				RealityPriorityClass = REALTIME_PRIORITY_CLASS
			};
		};

// ==================================================================================================================================================
		class CHRONOS_API SingleCoreThread : public IThread
		{
			public:
				SingleCoreThread(ICallback* callback, __bool keepCallbackAlive);
				SingleCoreThread(ICallback* callback);
				~SingleCoreThread(void);
				void Start(void* parameter);
				void Start();
				void Stop();
				__bool IsAlive();
				HANDLE GetThreadHandle();
				__uint GetThreadId();
				__bool SetPriority(__int priority);
				__bool SetPriorityClass(__int priorityClass);

			private:
				ICallback* _callback;
				HANDLE _threadHandle;
				DWORD _win32ThreadID;
				__bool _keepCallbackAlive;
		};

// ==================================================================================================================================================
		class CHRONOS_API MultiCoreThread : public IThread
		{
			public:
				MultiCoreThread(ICallback* callback, __uint threadsCount);
				~MultiCoreThread(void);
				void Start(void* parameter);
				void Start();
				void Stop();
				__bool IsAlive();
				void GetWorkingThreads(std::vector<SingleCoreThread*>* threads);
				__bool SetPriority(__int priority);
				__bool SetPriorityClass(__int priorityClass);
			private:
				SingleCoreThread** _threads;
				__uint _threadsCount;
				ICallback* _callback;
		};

// ==================================================================================================================================================
		class CHRONOS_API StringComparer
		{
			public:
				static bool Equals(__string* value1, __string* value2, __bool ignoreCase);
		};

// ==================================================================================================================================================
		class CHRONOS_API Converter
		{
			public:
				static HRESULT TryConvertStringToGuid(__string value, __guid* result);
				static __guid ConvertStringToGuid(__string value);
				static __string NormalizeGuidString(__string value);
				static __string ConvertGuidToString(__guid value);
				static __string ConvertGuidToString(__guid value, __wchar code);
				static __string ConvertIntToString(__int value);
				static __string ConvertLongToString(__ulong value);
				static std::string ConvertStringToStringA(__string value);
		};

// ==================================================================================================================================================
		class CHRONOS_API Formatter
		{
			public:
				static __string Format(__string format, ...);
		};

// ==================================================================================================================================================
		class CHRONOS_API CurrentProcess
		{
			public:
				static __uint GetProcessId();
				static __string GetArguments();
				static __int GetProcessPlatform();
				static SYSTEMTIME GetCreationTime();
				static HANDLE GetProcessHandle();
				static __string GetProcessName();
				
				enum ProcessPlatform
				{
					I386 = 0x014c,
					Itanium = 0x0200,
					X64 = 0x8664,
					Unknown = -1
				};

			private:
				static void Initialize();
				static HANDLE _processHandle;
		};

// ==================================================================================================================================================
		class CHRONOS_API EnvironmentVariables
		{
			public:
				static __string Get(__string variableName);
				static void Set(__string variableName, __string variableValue);
				static void Remove(__string variableName);
		};


// ==================================================================================================================================================
		struct CHRONOS_API IStreamWriter
		{
			virtual ~IStreamWriter() { }
			virtual __uint Write(void* data, __uint size) = 0;
			virtual __bool Initialized() = 0;
		};

// ==================================================================================================================================================
		struct CHRONOS_API IStreamReader
		{
			virtual ~IStreamReader() { }
			virtual __uint Read(void* data, __uint size) = 0;
			virtual void Skip(__uint count) = 0;
			virtual __bool End() = 0;
			virtual __bool Initialized() = 0;
		};

// ==================================================================================================================================================
		struct CHRONOS_API IStream : public virtual IStreamReader, public virtual IStreamWriter
		{
			virtual ~IStream() { }
			virtual __bool Initialized() = 0;

			enum StreamAccessMode
			{
				Input = 1,
				Output = 2,
				Dumplex = 3
			};
		};

// ==================================================================================================================================================
		struct CHRONOS_API IServerStream : public virtual IStream
		{
			virtual __bool WaitForConnection() = 0;
			virtual __bool Disconnect() = 0;
			virtual __bool Disconnected() = 0;
		};

// ==================================================================================================================================================
		struct CHRONOS_API IStreamFactory
		{
			virtual ~IStreamFactory() { }
			virtual HRESULT InitializeDaemonStreams(__guid sessionToken) = 0;
			virtual HRESULT ConnectDaemonDataStream(IStream** stream) = 0;
			virtual HRESULT ConnectHostInvokeStream(IStream** stream) = 0;
			virtual HRESULT CreateAgentInvokeStream(IServerStream** stream, GUID agentApplicationUid) = 0;
			virtual HRESULT CreateDaemonDataStream(IServerStream** stream) = 0;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API MemoryStream : public IStream
		{
			public:
				MemoryStream(__byte* data, __uint dataSize);
				MemoryStream();
				~MemoryStream();
				__uint Write(void* data, __uint size);
				__uint Read(void* data, __uint size);
				void Skip(__uint count);
				__bool End();
				__bool Initialized();
				Buffer* ToArray();
				__uint GetLength();
				void Seek(__uint position);
				void CopyTo(IStreamWriter* writer);

				const static __uint PageDefaultSize;
			private:
				void Init(__byte* buffer, __uint dataSize, __uint bufferSize);
				void Resize(__uint needSize);
				__byte* _buffer;
				__uint _bufferSize;
				__uint _dataSize;
				__uint _position;
				__uint _bufferPageSize;
		};

// ==================================================================================================================================================
		class CHRONOS_API NamedPipeStream : public virtual IStream
		{
			public:
				NamedPipeStream(void);
				~NamedPipeStream(void);
				__uint Write(void* data, __uint size);
				__uint Read(void* data, __uint size);
				void Skip(__uint count);
				__bool End();
				__bool Initialized();
			protected:
				__uint ConvertAccessMode(__uint accessMode);
				__uint ConvertShareMode(__uint accessMode);
				HANDLE _pipeHandle;
				__uint _lastError;
		};

// ==================================================================================================================================================
		class CHRONOS_API NamedPipeClientStream : public NamedPipeStream
		{
			public:
				NamedPipeClientStream(__string name, __uint accessMode);
		};

// ==================================================================================================================================================
#pragma warning(push) 
#pragma warning(disable:4250)
		class CHRONOS_API NamedPipeServerStream : public NamedPipeStream, public IServerStream
		{
			public:
				NamedPipeServerStream(__string name, __uint accessMode);
				__bool WaitForConnection();
				__bool Disconnect();
				__bool Disconnected();
		};
#pragma warning(pop)

// ==================================================================================================================================================
		class CHRONOS_API NamedPipeStreamFactory : public IStreamFactory
		{
			public:
				NamedPipeStreamFactory();
				~NamedPipeStreamFactory();
				HRESULT InitializeDaemonStreams(__guid sessionToken);
				HRESULT ConnectDaemonDataStream(IStream** stream);
				HRESULT ConnectHostInvokeStream(IStream** stream);
				HRESULT CreateAgentInvokeStream(IServerStream** stream, GUID agentApplicationUid);
				HRESULT CreateDaemonDataStream(IServerStream** stream);
			private:
				volatile __uint _lastDaemonDataStreamIndex;
				__guid _sessionToken;
		};

// ==================================================================================================================================================
		class CHRONOS_API Marshaler
		{
			public:
				static void MarshalBool(__bool value, IStreamWriter* stream);
				static void MarshalByte(__byte value, IStreamWriter* stream);
				static void MarshalInt(__int value, IStreamWriter* stream);
				static void MarshalUInt(__uint value, IStreamWriter* stream);
				static void MarshalLong(__long value, IStreamWriter* stream);
				static void MarshalULong(__ulong value, IStreamWriter* stream);
				static void MarshalSize(__size value, IStreamWriter* stream);
				static void MarshalString(__string* value, IStreamWriter* stream);
				static void MarshalGuid(__guid* value, IStreamWriter* stream);
				static void MarshalBuffer(Buffer* value, IStreamWriter* stream);
				
				static __bool DemarshalBool(IStreamReader* stream);
				static __byte DemarshalByte(IStreamReader* stream);
				static __int DemarshalInt(IStreamReader* stream);
				static __uint DemarshalUInt(IStreamReader* stream);
				static __long DemarshalLong(IStreamReader* stream);
				static __ulong DemarshalULong(IStreamReader* stream);
				static __size DemarshalSize(IStreamReader* stream);
				static __string DemarshalString(IStreamReader* stream);
				static __guid DemarshalGuid(IStreamReader* stream);
				static Buffer* DemarshalBuffer(IStreamReader* stream);

				const static __uint BoolSize;
				const static __uint ByteSize;
				const static __uint IntSize;
				const static __uint LongSize;
				const static __uint SizeSize;
				const static __uint CharSize;
				const static __uint GuidSize;
		};

// ==================================================================================================================================================
		class CHRONOS_API DynamicSettingBlock
		{
			public:
				DynamicSettingBlock(__int blockSize, __byte* block);
				~DynamicSettingBlock(void);
				__byte AsByte();
				__bool AsBool();
				__int AsInt();
				__uint AsUInt();
				__long AsLong();
				__ulong AsULong();
				__size AsSize();
				__string AsString();
				__guid AsGuid();
				IStreamReader* OpenRead();

			private:
				Buffer* _buffer;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API DynamicSettings
		{
			public:
				DynamicSettings();
				~DynamicSettings();
				DynamicSettingBlock* GetSettingBlock(__guid settingToken);
				bool Initialize(IStreamReader* stream);

			protected:
				__bool _initialized;
				std::map<__guid, DynamicSettingBlock*>* _settings;
			private:
				const static __guid AgentDllIndex;
		};

// ==================================================================================================================================================
		class CHRONOS_API UniqueSettings : public DynamicSettings
		{
			public:
				UniqueSettings();
				~UniqueSettings();
				__bool GetUid(__guid* value);

			private:
				const static __guid UidIndex;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API ExportSettings : public UniqueSettings
		{
			public:
				ExportSettings();
				~ExportSettings();
				__bool GetAgentDll(__string* value);

			private:
				const static __guid AgentDllIndex;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API GatewaySettings : public DynamicSettings
		{
			public:
				GatewaySettings();
				~GatewaySettings();
				__bool GetSyncStreamsCount(__uint* value);
				__bool GetAsyncStreamsCount(__uint* value);
			private:
				const static __guid SyncStreamsCountIndex;
				const static __guid AsyncStreamsCountIndex;
		};

// ==================================================================================================================================================
		class CHRONOS_API ProfilingTargetSettings : public ExportSettings
		{
			public:
				__bool GetProfileChildProcess(__bool* value);

			private:
				const static __guid ProfileChildProcessIndex;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API ProfilingTypeSettings : public ExportSettings
		{
			public:
				__bool GetDataMarker(__byte* value);
				__bool GetFrameworkUid(__guid* value);
				__bool GetDependencies(std::vector<__guid>* value);

			private:
				const static __guid DataMarkerIndex;
				const static __guid FrameworkUidIndex;
				const static __guid DependenciesIndex;
		};

// ==================================================================================================================================================
		class CHRONOS_API FrameworkSettings : public ExportSettings
		{
		};
		
// ==================================================================================================================================================
		template<typename T>
		class ExportSettingsCollection
		{
			public:	
				ExportSettingsCollection<T>()
				{
					_initialized = false;
					_items = new __vector<T*>();
				}
				~ExportSettingsCollection<T>()
				{
					if (_items == null)
					{
						return;
					}
					while (!_items->empty())
					{
						T* settings = _items->back();
						_items->pop_back();
						__FREEOBJ(settings);
					}
					__FREEOBJ(_items);
				}
				bool Initialize(IStreamReader* stream)
				{
					if (_initialized)
					{
						return true;
					}
					_initialized = true;
					__int count = Marshaler::DemarshalInt(stream);
					for (__int i = 0; i < count; i++)
					{
						T* settings = new T();
						if (!settings->Initialize(stream))
						{
							return false;
						}
						_items->push_back(settings);
					}
					return true;
				}
				std::vector<T*> GetItems()
				{
					std::vector<T*> items = *_items;
					return items;
				}
			protected:
				__vector<T*>* _items;
			private:
				__bool _initialized;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API ProfilingTypeSettingsCollection : public ExportSettingsCollection<ProfilingTypeSettings>
		{
			public:
				ProfilingTypeSettingsCollection(void);
				~ProfilingTypeSettingsCollection(void);
				HRESULT ValidateAndSort();
			private:
				HRESULT Validate();
				HRESULT Sort();
				__bool CanBeInitialized(ProfilingTypeSettings* item, __vector<ProfilingTypeSettings*>* sortedItems);
				__bool ContainsItem(__vector<ProfilingTypeSettings*>* items, __guid itemUid);
				__bool ContainsItem(__vector<__guid>* items, __guid itemUid);
		};
		
// ==================================================================================================================================================
		class CHRONOS_API FrameworkSettingsCollection : public ExportSettingsCollection<FrameworkSettings>
		{
			public:
				FrameworkSettingsCollection(void);
				~FrameworkSettingsCollection(void);
				HRESULT ValidateAndSort();
			private:
				HRESULT Sort();
		};

// ==================================================================================================================================================
		class CHRONOS_API SessionSettings : public UniqueSettings
		{
			public:
				SessionSettings(void);
				~SessionSettings(void);
				__bool GetProfilingTypesSettings(ProfilingTypeSettingsCollection** profilingTypesSettings);
				__bool GetFrameworksSettings(FrameworkSettingsCollection** frameworksSettings);
				__bool GetGatewaySettings(GatewaySettings** gatewaySettings);
				__bool GetProfilingTargetSettings(ProfilingTargetSettings** profilingTargetSettings);
				__bool GetSessionUid(__guid* uid);
				HRESULT ValidateAndSort();
			
			private:
				__bool _initialized;
				ProfilingTypeSettingsCollection* _profilingTypesSettings;
				FrameworkSettingsCollection* _frameworksSettings;
				ProfilingTargetSettings* _profilingTargetSettings;
				GatewaySettings* _gatewaySettings;

				const static __guid ProfilingTypesSettingsIndex;
				const static __guid ProfilingTargetSettingsIndex;
				const static __guid FrameworksSettingsSettingsIndex;
				const static __guid GatewaySettingsIndex;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API ServiceContainer
		{
			public:
				ServiceContainer(void);
				bool ResolveService(__guid serviceToken, void** service);
				bool RegisterService(__guid serviceToken, void* service);
				bool RegisterService(__guid serviceToken, void* service, __bool overrideExisting);
				bool UnregisterService(__guid serviceToken);
				~ServiceContainer(void);
			private:
				std::map<__guid, void*>* _services;
				CriticalSection _criticalSection;
		};
		
// ==================================================================================================================================================
		struct CHRONOS_API IProfilingTypeAdapter
		{
			virtual ~IProfilingTypeAdapter() { }
			virtual HRESULT BeginInitialize(ProfilingTypeSettings* settings) = 0;
			virtual HRESULT ExportServices(ServiceContainer* container) = 0;
			virtual HRESULT ImportServices(ServiceContainer* container) = 0;
			virtual HRESULT EndInitialize() = 0;
			virtual HRESULT SubscribeEvents() = 0;
			virtual HRESULT FlushData() = 0;
		};

		typedef void (__cdecl* CREATE_CHRONOS_PROFILING_TYPE)(IProfilingTypeAdapter** adapter);
		
// ==================================================================================================================================================
		class CHRONOS_API ProfilingType
		{
			public:
				ProfilingType(ProfilingTypeSettings* settings);
				~ProfilingType();
				HRESULT LoadAdapter();
				HRESULT BeginInitialize();
				HRESULT ExportServices(ServiceContainer* container);
				HRESULT ImportServices(ServiceContainer* container);
				HRESULT EndInitialize();
				HRESULT SubscribeEvents();
				HRESULT FlushData();

			private:
				IProfilingTypeAdapter* _adapter;
				ProfilingTypeSettings* _settings;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API ProfilingTypeCollection
		{
			public:
				ProfilingTypeCollection();
				~ProfilingTypeCollection();
				HRESULT Initialize(ProfilingTypeSettingsCollection* settings, ServiceContainer* container);
				HRESULT LoadExtensions();
				HRESULT BeginInitialize();
				HRESULT ExportServices();
				HRESULT ImportServices();
				HRESULT EndInitialize();
				HRESULT SubscribeEvents();
				HRESULT FlushData();
			private:
				std::vector<ProfilingType*>* _profilingTypes;
				ProfilingTypeSettingsCollection* _settings;
				ServiceContainer* _container;
		};

// ==================================================================================================================================================
		struct CHRONOS_API IProfilingTargetAdapter
		{
			virtual ~IProfilingTargetAdapter() { }
			virtual HRESULT BeginInitialize(ProfilingTargetSettings* settings) = 0;
			virtual HRESULT ExportServices(ServiceContainer* container) = 0;
			virtual HRESULT ImportServices(ServiceContainer* container) = 0;
			virtual HRESULT EndInitialize() = 0;
		};

		typedef void (__cdecl* CREATE_CHRONOS_PROFILING_TARGET)(IProfilingTargetAdapter** adapter);


// ==================================================================================================================================================
		class CHRONOS_API ProfilingTarget
		{
			public:
				ProfilingTarget(ProfilingTargetSettings* settings);
				~ProfilingTarget();
				HRESULT LoadAdapter();
				HRESULT BeginInitialize();
				HRESULT ExportServices(ServiceContainer* container);
				HRESULT ImportServices(ServiceContainer* container);
				HRESULT EndInitialize();
			private:
				IProfilingTargetAdapter* _adapter;
				ProfilingTargetSettings* _settings;
		};

// ==================================================================================================================================================
		struct CHRONOS_API IFrameworkAdapter
		{
			virtual ~IFrameworkAdapter() { }
			virtual HRESULT BeginInitialize(FrameworkSettings* framworkSettings, ProfilingTargetSettings* profilingTargetSettings) = 0;
			virtual HRESULT ExportServices(ServiceContainer* container) = 0;
			virtual HRESULT ImportServices(ServiceContainer* container) = 0;
			virtual HRESULT EndInitialize() = 0;
			virtual HRESULT SubscribeEvents() = 0;
			virtual HRESULT FlushData() = 0;
		};

		typedef void (__cdecl* CREATE_CHRONOS_FRAMEWORK)(IFrameworkAdapter** adapter);
		
// ==================================================================================================================================================
		class CHRONOS_API Framework
		{
			public:
				Framework(FrameworkSettings* frameworkSettings, ProfilingTargetSettings* profilingTargetSettings);
				~Framework();
				HRESULT LoadAdapter();
				HRESULT BeginInitialize();
				HRESULT ExportServices(ServiceContainer* container);
				HRESULT ImportServices(ServiceContainer* container);
				HRESULT EndInitialize();
				HRESULT SubscribeEvents();
				HRESULT FlushData();
			private:
				IFrameworkAdapter* _adapter;
				FrameworkSettings* _frameworkSettings;
				ProfilingTargetSettings* _profilingTargetSettings;
		};

// ==================================================================================================================================================
		//TODO: Create base class for ProfilingTypeCollection and FrameworkCollection
		class CHRONOS_API FrameworkCollection
		{
			public:
				FrameworkCollection();
				~FrameworkCollection();
				HRESULT Initialize(FrameworkSettingsCollection* frameworksSettings, ProfilingTargetSettings* profilingTargetSettings,  ServiceContainer* container);
				HRESULT LoadExtensions();
				HRESULT BeginInitialize();
				HRESULT ExportServices();
				HRESULT ImportServices();
				HRESULT EndInitialize();
				HRESULT SubscribeEvents();
				HRESULT FlushData();
			private:
				FrameworkSettingsCollection* _frameworksSettings;
				ProfilingTargetSettings* _profilingTargetSettings;
				__vector<Framework*>* _frameworks;
				ServiceContainer* _container;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API ProfilingTimer
		{
			public:
				ProfilingTimer();
				~ProfilingTimer();
				void UpdateTime();
				__uint GetBeginTime();
				volatile __uint CurrentTime;
				
				const static __guid ServiceToken;
			private:
				void StartTimeRefreshing(void* parameter);
				volatile __uint _beginTime;
				__uint GetTime();
				SingleCoreThread* _timeThread;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API GatewayPackage : public IStreamWriter
		{
			public:
				~GatewayPackage();
				__uint Write(void* data, __uint size);
				__bool Initialized();

				void SetDataSize(__uint dataSize);
				__uint GetDataSize();
				__uint GetBufferSize();
				__uint GetPayloadSize();
				__byte* GetBuffer();
				__byte* GetData();

				static GatewayPackage* CreateClone(GatewayPackage* package);
				static GatewayPackage* CreateDynamic(__byte dataMarker);
				static GatewayPackage* CreateStatic(__byte dataMarker, __uint dataSize);
				static void ReadPackage(IStream* stream, __byte* dataMarker, __byte** data, __uint* dataSize);

				const static __uint HeaderSize;
				const static __uint MaxDataSize;

			private:
				GatewayPackage(GatewayPackage* package);
				GatewayPackage(__byte dataMarker);
				GatewayPackage(__byte dataMarker, __uint bufferSize);

				void Initialize(__byte dataMarker, __uint bufferSize, __bool staticPackage);
				void Resize(__uint needSize);

				__uint* _dataSizePointer;
				__bool _staticPackage;
				__byte* _cursor;
				__byte* _bufferBegin;
				__byte* _bufferEnd;
				__uint _bufferSize;
				__uint _bufferPageSize;
		};

// ==================================================================================================================================================
		struct CHRONOS_API IDataHandler
		{
			virtual ~IDataHandler() { }
			virtual __bool HandlePackage(__byte* data, __uint size) = 0;
		};

// ==================================================================================================================================================
		typedef __bool (__stdcall *DataHandlerCallback)(__byte*, __uint);

		class CHRONOS_API DataHandlerRouter : public IDataHandler
		{
			public:
				DataHandlerRouter(DataHandlerCallback callback);
				~DataHandlerRouter();
				__bool HandlePackage(__byte* data, __uint size);
			private:
				DataHandlerCallback _callback;
		};

// ==================================================================================================================================================
		struct GatewayPackageContainer
		{
			GatewayPackageContainer()
			{
				Value = null;
			}

			GatewayPackage* SetValue(GatewayPackage* value)
			{
				GatewayPackage* temp = (GatewayPackage*)InterlockedExchangePointer((PVOID*)&(Value), (PVOID)value);
				return temp;
			}

			volatile GatewayPackage* Value;
		};
		
// ==================================================================================================================================================
		class SyncGatewayClient
		{
			public:
				SyncGatewayClient(IStreamFactory* streamFactory);
				~SyncGatewayClient();
				HRESULT Initialize(GatewaySettings* gatewaySettings);
				void Send(GatewayPackage* package);

			private:
				IStreamFactory* _streamFactory;
				IStream* _streams[10];
				CriticalSection _criticalSection;
				volatile __uint  _currentStreamId;
				__uint _syncStreamsCount;
				__declspec(thread) static IStream* _stream;
		};

// ==================================================================================================================================================
		class AsyncGatewayClient
		{
			public:
				AsyncGatewayClient(IStreamFactory* streamFactory);
				~AsyncGatewayClient();
				HRESULT Initialize(GatewaySettings* gatewaySettings);
				void Send(GatewayPackage* package);
				void GetWorkingThreads(std::vector<SingleCoreThread*>* threads);
				void WaitWhileSending();

			private:
				void StartSending(void* parameter);
				__size PackagesCount();

			private:
				__uint _asyncStreamsCount;
				IStreamFactory* _streamFactory;
				SingleCoreThread* _sendingThread;
				volatile __bool _sending;
				const static __uint PackagesMaxCount = 256;
				GatewayPackageContainer* _packages[PackagesMaxCount];
		};

// ==================================================================================================================================================
		class CHRONOS_API GatewayClient
		{
			public:
				GatewayClient(IStreamFactory* streamFactory);
				~GatewayClient(void);
				HRESULT Initialize(GatewaySettings* gatewaySettings);
				void Send(GatewayPackage* package, __bool leavePackageAlive);
				void Send(GatewayPackage* package);
				void GetWorkingThreads(std::vector<SingleCoreThread*>* threads);
				void WaitWhileSending();
				
				const static __guid ServiceToken;
			private:
				AsyncGatewayClient* _asyncGateway;
				SyncGatewayClient* _syncGateway;
		};

// ==================================================================================================================================================
		class CHRONOS_API GatewayServerStream
		{
			public:
				GatewayServerStream(IServerStream* stream, IDataHandler** handlers);
				~GatewayServerStream();
				void StartReading();
				void StopReading();

			private:
				void StartPackagesReading(void* parameter);
				
			private:
				IThread* _thread;
				IServerStream* _stream;
				IDataHandler** _handlers;
				volatile __bool _reading;
				volatile __bool _started;
		};

// ==================================================================================================================================================
		class CHRONOS_API GatewayServer
		{
			public:
				GatewayServer(IStreamFactory* streamFactory);
				~GatewayServer(void);
				HRESULT Start(__byte streamsCount);
				void StartReading();
				void StopReading();
				__bool IsLocked();
				HRESULT RegisterHandler(__byte dataMarker, IDataHandler* handler);
				void Lock();

				const static __guid ServiceToken;
			private:
				IStreamFactory* _streamFactory;
				__vector<GatewayServerStream*>* _streams;
				IDataHandler** _handlers;
				volatile __bool _isLocked;
		};

// ==================================================================================================================================================
		class CHRONOS_API GatewayPackageWriter : public IStreamWriter
		{
			public:
				GatewayPackageWriter(GatewayClient* gatewayClient, __byte dataMarker, __uint packageSize);
				~GatewayPackageWriter(void);
				__uint Write(void* data, __uint size);
				__bool Initialized();
			private:
				GatewayClient* _gatewayClient;
				__byte _dataMarker;
				__uint _packageSize;
				GatewayPackage* _currentPackage;
		};

// ==================================================================================================================================================
		typedef __int (*AgentInvokeCallback)(Buffer* argumentsBuffer, Buffer** returnBuffer);

		class CHRONOS_API AgentInvokeServer
		{
			public:
				AgentInvokeServer();
				~AgentInvokeServer();
				HRESULT Initialize(IStreamFactory* factory, GUID agentApplicationUid);
				__bool RegisterCallback(__guid operationId, AgentInvokeCallback callback);
				void LockChanges();
				void ReadAndInvoke();
				static const __guid ServiceToken;

			private:
				void StartMessagesProcessing(void* parameter);
				__bool _locked;
				IServerStream* _stream;
				std::map<__guid, AgentInvokeCallback>* _callbacks;
				IThread* _invokeServerThread;
				CriticalSection _criticalSection;
		};

// ==================================================================================================================================================
		class CHRONOS_API HostClient
		{
			public:
				HostClient(IStreamFactory* streamFactory);
				__bool StartProfilingSession(__guid configurationToken, __guid applicationUid, __uint profilingBeginTime, SessionSettings* settings);

			private:
				IStreamFactory* _streamFactory;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API RuntimeController
		{
			public:
				RuntimeController(Chronos::Agent::GatewayClient* gatewayClient);
				~RuntimeController(void);
				HRESULT SuspendRuntime();
				HRESULT ResumeRuntime();

				static const __guid ServiceToken;
			private:
				//map<THREAD_ID, THREAD_HANDLE>
				HRESULT GetNonSuspendedThreads(std::map<__uint, HANDLE>* threads);
				std::vector<__uint> GetIgnoredThreads();
				__bool IsThreadIgnored(std::vector<__uint>* ignoredThreads, __uint thread);

				CriticalSection _criticalSection;
				volatile __bool _suspended;
				//map<THREAD_ID, THREAD_HANDLE>
				std::map<__uint, HANDLE>* _suspendedThreads;
				Chronos::Agent::GatewayClient* _gatewayClient;
		};

// ==================================================================================================================================================
		struct CHRONOS_API ITask
		{
			virtual ~ITask() { }
			virtual void Execute() = 0;
		};

// ==================================================================================================================================================
		class CHRONOS_API TaskWorker
		{
			public:
				static void Push(ITask* task);
				static void SetMaxThreadsCount(__ushort threadsCount);
				static void SetMinThreadsCount(__ushort threadsCount);
		};

// ==================================================================================================================================================
		template<typename T>
		struct FastDictionaryContainer32
		{
			FastDictionaryContainer32<T>::FastDictionaryContainer32()
			{
				Content = null;
				for (__int i = 0; i <= 0xFF; i++)
				{
					Containers[i] = null;
				}
			}
			FastDictionaryContainer32<T>::~FastDictionaryContainer32()
			{
				__FREEOBJ(Content);
				for (__int i = 0; i <= 0xFF; i++)
				{
					__FREEOBJ(Containers[i]);
					Containers[i] = null;
				}
			}
			FastDictionaryContainer32<T>* Containers[0xFF];
			T Content;
		};

// ==================================================================================================================================================
		template<typename T>
		class FastDictionary32
		{
			public:
				FastDictionary32<T>::FastDictionary32(void)
				{
					_entryPoint = new FastDictionaryContainer32<T>();
				}

				FastDictionary32<T>::~FastDictionary32(void)
				{
					__FREEOBJ(_entryPoint);
				}

				T FastDictionary32<T>::Find(__uint key)
				{
					FastDictionaryContainer32<T>* current = _entryPoint;
					__short part = 0;

					part = (key >> 0) & 0x000000FF;
					current = current->Containers[part];
	
					part = (key >> 8) & 0x000000FF;
					current = current->Containers[part];
	
					part = (key >> 16) & 0x000000FF;
					current = current->Containers[part];
	
					part = (key >> 24) & 0x000000FF;
					current = current->Containers[part];

					return current->Content;
				}

				void FastDictionary32<T>::Insert(__uint key, T value)
				{
					FastDictionaryContainer32<T>* current = _entryPoint;
					__short part = 0;

					part = (key >> 0) & 0x000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer32<T>();
					}
					current = current->Containers[part];
	
					part = (key >> 8) & 0x000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer32<T>();
					}
					current = current->Containers[part];
	
					part = (key >> 16) & 0x000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer32<T>();
					}
					current = current->Containers[part];
	
					part = (key >> 24) & 0x000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer32<T>();
					}
					current = current->Containers[part];
	
					current->Content = value;
				}

				void FastDictionary32<T>::Erase(__uint key)
				{

				}

			private:
				FastDictionaryContainer32<T>* _entryPoint;
		};
		
		
// ==================================================================================================================================================
		template<typename T>
		struct FastDictionaryContainer64
		{
			FastDictionaryContainer64<T>::FastDictionaryContainer64()
			{
				Content = null;
				for (__int i = 0; i <= 0xFF; i++)
				{
					Containers[i] = null;
				}
			}
			FastDictionaryContainer64<T>::~FastDictionaryContainer64()
			{
				__FREEOBJ(Content);
				for (__int i = 0; i <= 0xFF; i++)
				{
					__FREEOBJ(Containers[i]);
					Containers[i] = null;
				}
			}
			FastDictionaryContainer64<T>* Containers[0xFF];
			T Content;
		};
// ==================================================================================================================================================
		template<typename T>
		class FastDictionary64
		{
			public:
				FastDictionary64<T>::FastDictionary64(void)
				{
					_entryPoint = new FastDictionaryContainer64<T>();
				}

				FastDictionary64<T>::~FastDictionary64(void)
				{
					__FREEOBJ(_entryPoint);
				}

				T FastDictionary64<T>::Find(__ulong key)
				{
					FastDictionaryContainer64<T>* current = _entryPoint;
					__short part = 0;

					part = (key >> 0) & 0x00000000000000FF;
					current = current->Containers[part];
	
					part = (key >> 8) & 0x00000000000000FF;
					current = current->Containers[part];
	
					part = (key >> 16) & 0x00000000000000FF;
					current = current->Containers[part];
	
					part = (key >> 24) & 0x00000000000000FF;
					current = current->Containers[part];
					
					part = (key >> 32) & 0x00000000000000FF;
					current = current->Containers[part];
					
					part = (key >> 40) & 0x00000000000000FF;
					current = current->Containers[part];
					
					part = (key >> 48) & 0x00000000000000FF;
					current = current->Containers[part];
					
					part = (key >> 56) & 0x00000000000000FF;
					current = current->Containers[part];

					return current->Content;
				}

				void FastDictionary64<T>::Insert(__ulong key, T value)
				{
					FastDictionaryContainer64<T>* current = _entryPoint;
					__short part = 0;
					//---------------------------------------------------------------------
					part = (key >> 0) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------
					part = (key >> 8) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------
					part = (key >> 16) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------
					part = (key >> 24) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------
					part = (key >> 32) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------
					part = (key >> 40) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------
					part = (key >> 48) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------
					part = (key >> 56) & 0x00000000000000FF;
					if (current->Containers[part] == null)
					{
						current->Containers[part] = new FastDictionaryContainer64<T>();
					}
					current = current->Containers[part];
					//---------------------------------------------------------------------

					current->Content = value;
				}

				void FastDictionary64<T>::Erase(__ulong key)
				{

				}

			private:
				FastDictionaryContainer64<T>* _entryPoint;
		};

// ==================================================================================================================================================
		template<typename T>
		class DynamicBlockPage
		{
			public:
				DynamicBlockPage<T>(__uint capacity, __int pageIndex, __bool threadSafe)
				{
					_threadSafe = threadSafe;
					Capacity = capacity;
					NextPage = null;
					PrevPage = null;
					CurrentIndex = -1;
					Full = false;
					LastIndex = Capacity - 1;
					PageIndex = pageIndex;
					Items = new T[Capacity];
				}
				~DynamicBlockPage<T>()
				{
					__FREEARR(Items);
					NextPage = null;
					PrevPage = null;
				}
				T* Next()
				{
					long currentIndex;
					if (_threadSafe)
					{
						currentIndex = InterlockedIncrement(&CurrentIndex);
					}
					else
					{
						CurrentIndex++;
						currentIndex = CurrentIndex;
					}
					Full = currentIndex >= LastIndex;
					if (currentIndex > LastIndex)
					{
						return null;
					}
					T* item = &(Items[currentIndex]);
					return item;
				}
				T* Next(long* index)
				{
					long currentIndex;
					if (_threadSafe)
					{
						currentIndex = InterlockedIncrement(&CurrentIndex);
					}
					else
					{
						CurrentIndex++;
						currentIndex = CurrentIndex;
					}
					Full = currentIndex >= LastIndex;
					if (currentIndex > LastIndex)
					{
						return null;
					}
					T* item = &(Items[currentIndex]);
					*index = currentIndex;
					return item;
				}
				void Reset()
				{
					NextPage = null;
					PrevPage = null;
					CurrentIndex = -1;
					Full = false;
					LastIndex = Capacity - 1;
					__FREEARR(Items);
					Items = new T[Capacity];
				}
				DynamicBlockPage<T>* NextPage;
				DynamicBlockPage<T>* PrevPage;
				__bool Full;
				__uint Capacity;
				volatile long CurrentIndex;
				__int LastIndex;
				T* Items;
				__int PageIndex;
			private:
				__bool _threadSafe;
		};

// ==================================================================================================================================================
		template<typename T>
		class DynamicBlockFactory
		{
			public:
				DynamicBlockFactory<T>()
					: _pageSize(FIRST_PAGE_SIZE), _lastPageIndex(0), _threadSafe(true)
				{
					FirstPage = new DynamicBlockPage<T>(_pageSize, _lastPageIndex, _threadSafe);
					LastPage = FirstPage;
				}
				DynamicBlockFactory<T>(__bool threadSafe)
					: _pageSize(FIRST_PAGE_SIZE), _lastPageIndex(0), _threadSafe(threadSafe)
				{
					FirstPage = new DynamicBlockPage<T>(_pageSize, _lastPageIndex, _threadSafe);
					LastPage = FirstPage;
				}
				DynamicBlockFactory<T>(__uint pageSize, __bool threadSafe)
					: _pageSize(pageSize), _lastPageIndex(0), _threadSafe(threadSafe)
				{
					FirstPage = new DynamicBlockPage<T>(_pageSize, _lastPageIndex, _threadSafe);
					LastPage = FirstPage;
				}
				~DynamicBlockFactory<T>()
				{
					DynamicBlockPage<T>* currentPage = FirstPage;
					while (currentPage != null)
					{
						DynamicBlockPage<T>* nextPage = currentPage->NextPage;
						__FREEOBJ(currentPage);
						currentPage = nextPage;
					}
					FirstPage = null;
					LastPage = null;
				}
				T* Next()
				{
					while (true)
					{
						DynamicBlockPage<T>* currentPage = LastPage;
						if (currentPage->Full)
						{
							currentPage = CreatePage();
						}
						T* item = currentPage->Next();
						if (item != null)
						{
							return item;
						}
					}
				}
				T* Next(__uint* globalIndex)
				{
					while (true)
					{
						DynamicBlockPage<T>* currentPage = LastPage;
						if (currentPage->Full)
						{
							currentPage = CreatePage();
						}
						long localIndex = 0;
						T* item = currentPage->Next(&localIndex);
						if (item != null)
						{
							*globalIndex = (currentPage->PageIndex * _pageSize + localIndex);
							return item;
						}
					}
				}
				__uint GetPagesCount()
				{
					return _lastPageIndex + 1;
				}
				void Reset()
				{
					if (_threadSafe)
					{
						Lock lock(&_criticalSection);
						ResetInternal();
					}
					else
					{
						ResetInternal();
					}
				}
				DynamicBlockPage<T>* FirstPage;
				DynamicBlockPage<T>* LastPage;
			private:
				void ResetInternal()
				{
					DynamicBlockPage<T>* currentPage = FirstPage->NextPage;
					while (currentPage != null)
					{
						DynamicBlockPage<T>* nextPage = currentPage->NextPage;
						__FREEOBJ(currentPage);
						currentPage = nextPage;
					}
					LastPage = FirstPage;
					FirstPage->Reset();
				}
				DynamicBlockPage<T>* CreatePage()
				{
					if (_threadSafe)
					{
						Lock lock(&_criticalSection);
						return CreatePageInternal();
					}
					return CreatePageInternal();
				}
				DynamicBlockPage<T>* CreatePageInternal()
				{
					if (LastPage->Full)
					{
						_lastPageIndex++;
						if (_pageSize < PAGE_SIZE_LIMIT)
						{
							_pageSize = _pageSize * 2;
							if (_pageSize > PAGE_SIZE_LIMIT)
							{
								_pageSize = PAGE_SIZE_LIMIT;
							}
						}
						DynamicBlockPage<T>* newpage = new DynamicBlockPage<T>(_pageSize, _lastPageIndex, _threadSafe);
						LastPage->NextPage = newpage;
						newpage->PrevPage = LastPage;
						LastPage = newpage;
					}
					return LastPage;
				}
				__bool _threadSafe;
				volatile __int _lastPageIndex;
				CriticalSection _criticalSection;
				__uint _pageSize;
				static const __uint FIRST_PAGE_SIZE = 1024;
				static const __uint PAGE_SIZE_LIMIT = 4096;
		};

// ==================================================================================================================================================
		template<typename T>
		class DynamicBlockFactoryEnumerator : public IEnumerator<T>
		{
			public:

				DynamicBlockFactoryEnumerator<T>(DynamicBlockFactory<T>* factory)
				{
					Current = null;
					_currentPage = factory->LastPage;
					_currentIndex = __min((__int)_currentPage->CurrentIndex, _currentPage->LastIndex);
					if (_currentIndex < 0)
					{
						_currentPage = _currentPage->PrevPage;
						if (_currentPage != null)
						{
							_currentIndex = __min((__int)_currentPage->CurrentIndex, _currentPage->LastIndex);
						}
					}
					if (_currentPage != null)
					{
						Current = &(_currentPage->Items[_currentIndex]);
					}
				}
				void MoveNext()
				{
					if (Current == null)
					{
						return;
					}
					_currentIndex--;
					if (_currentIndex < 0)
					{
						_currentPage = _currentPage->PrevPage;
						if (_currentPage == null)
						{
							Current = null;
							return;
						}
						else
						{
							_currentIndex = __min((__int)_currentPage->CurrentIndex, _currentPage->LastIndex);
						}
					}
					Current = &(_currentPage->Items[_currentIndex]);
				}
				T* Current;
			private:
				DynamicBlockPage<T>* _currentPage;
				__int _currentIndex;
		};

// ==================================================================================================================================================
		template<typename T>
		class StaticBlockFactory
		{
			public:
				StaticBlockFactory<T>(__int capacity)
				{
					_items = new T[capacity];
					_currentIndex = 0;
				}
				~StaticBlockFactory<T>()
				{
					__FREEARR(_items);
				}
				T* Next()
				{
					T* item = &(_items[_currentIndex]);
					_currentIndex++;
					return item;
				}
			private:
				T* _items;
				__int _currentIndex;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API Application
		{
			public:
				Application(void);
				~Application(void);
				// STARTUP
				HRESULT Run();
				HRESULT Shutdown();
				// ATTACH
				HRESULT Attach();
				HRESULT Attached();
				HRESULT Deattach();
				// FAULT
				HRESULT Fault(EXCEPTION_POINTERS* exceptionInfo);
				void FlushData();

				ServiceContainer* Container;

			private:
				HRESULT BeginInitialize();
				HRESULT ExportServices();
				HRESULT LoadExtensions();
				HRESULT EndInitialize();
				HRESULT ShutdownInternal();

				ProfilingTimer* _profilingTimer;
				IStreamFactory* _streamFactory;
				GatewayClient* _gatewayClient;
				ProfilingTarget* _profilingTarget;
				ProfilingTypeCollection* _profilingTypes;
				FrameworkCollection* _frameworks;
				RuntimeController* _runtimeController;
				AgentInvokeServer* _invokeServer;

				SessionSettings _sessionSettings;
				__guid _configurationToken;
				__guid _applicationUid;
		};
		
// ==================================================================================================================================================
		struct CHRONOS_API IUnit
		{
			public:
				virtual ~IUnit() { }
				virtual void Initialize(__uint uid, __uptr id, __uint beginLifetime) = 0;
				virtual void Close(__uint endLifetime) = 0;

				virtual __uint GetBeginLifetime() = 0;
				virtual __uint GetEndLifetime() = 0;
				virtual __string* GetName() = 0;
				virtual __bool GetIsAlive() = 0;

				__uint Uid;
				__uptr Id;
		};

// ==================================================================================================================================================
		struct CHRONOS_API UnitBase : public IUnit
		{
			public:
				UnitBase();
				~UnitBase();
				void Initialize(__uint uid, __uptr id, __uint beginLifetime);
				void Close(__uint endLifetime);

				__uint GetBeginLifetime();
				__uint GetEndLifetime();
				__string* GetName();
				__bool GetIsAlive();

			protected:
				__uint _beginLifetime;
				__uint _endLifetime;
				__string* _name;
		};

// ==================================================================================================================================================
		template<typename T>
		class UnitCollectionBase
		{
			public:
				template<typename T>
				struct UnitContainer
				{
					UnitContainer<T>()
						: Revision(-1), OwnerThreadId(0), IsAlive(false)
					{
					}
					T Unit;
					__uptr Id;
					volatile __int Revision;
					__bool IsAlive;
					__uptr OwnerThreadId;
				};

			public:
				UnitCollectionBase<T>()
					: _revision(0), _factory(new DynamicBlockFactory<UnitContainer<T>>())
				{
				}
	
				~UnitCollectionBase<T>()
				{
					__FREEOBJ(_factory);
				}

				void Initialize(ProfilingTimer* profilingTimer)
				{
					_profilingTimer = profilingTimer;
				}

				T* CreateUnit(__uptr id)
				{
					__uint uid = 0;
					UnitContainer<T>* container = _factory->Next(&uid);
					T* unit = &(container->Unit);
					unit->Initialize(uid, id, _profilingTimer->CurrentTime);
					InitializeUnitSpecial(unit);
					container->Id = id;
					container->IsAlive = true;
					container->Revision = _revision;
					return unit;
				}
				
				T* GetUnit(__uptr id, __bool aliveOnly)
				{
					T* unit = null;
					UnitContainer<T>* container = GetUnitContainer(id, aliveOnly);
					if (container != null)
					{
						unit = &(container->Unit);
					}
					return unit;
				}
				
				T* GetUnit(__uptr id)
				{
					return GetUnit(id, true);
				}

				__bool ContainUnit(__uptr id)
				{
					return GetUnit(id) != null;
				}
				
				void CloseUnit(__uptr id)
				{
					T* unit = null;
					UnitContainer<T>* container = GetUnitContainer(id, true);
					if (container != null)
					{
						container->Unit.Close(_profilingTimer->CurrentTime);
						container->IsAlive = false;
						container->Revision = _revision;
					}
				}
	
				void UpdateUnit(__uptr id)
				{
					UnitContainer<T>* container = GetUnitContainer(id, true);
					if (container != null)
					{
						container->Revision = _revision;
					}
				}
	
				void GetUpdates(std::list<T*>* updates)
				{
					__int revision = (__int)InterlockedIncrement(&_revision) - 1;
					DynamicBlockFactoryEnumerator<UnitContainer<T>> enumerator(_factory);
					UnitContainer<T>* container = null;
					while((container = enumerator.Current) != null)
					{
						if (container->Revision == revision)
						{
							updates->push_back(&(container->Unit));
						}
						enumerator.MoveNext();
					}
				}
				
				IEnumerator<UnitContainer<T>> EnumerateUnits()
				{
					DynamicBlockFactoryEnumerator<UnitContainer<T>> enumerator(_factory);
					return enumerator;
				}
				
			protected:

				virtual HRESULT InitializeUnitSpecial(T* unit) = 0;

				UnitContainer<T>* GetUnitContainer(__uptr id, __bool aliveOnly)
				{
					UnitContainer<T>* container = null;
					DynamicBlockFactoryEnumerator<UnitContainer<T>> enumerator(_factory);
					while((container = enumerator.Current) != null)
					{
						if (container->Id == id && (!aliveOnly || container->IsAlive))
						{
							return container;
						}
						enumerator.MoveNext();
					}
					return container;
				}
				
			protected:
				DynamicBlockFactory<UnitContainer<T>>* _factory;
				ProfilingTimer* _profilingTimer;
				volatile long _revision;
		};
		
// ==================================================================================================================================================
		class CHRONOS_API UnitMarshaler
		{
			public:
				static void MarshalUnit(IUnit* unit, IStreamWriter* stream);

				template <class T>
				static void SendUnits(__int unitType, UnitCollectionBase<T>* unitCollection, void (marshalUnit)(T* unit, IStreamWriter* stream), GatewayClient* gatewayClient, __byte dataMarker)
				{
					const __int packageSizeLimit = GatewayPackage::MaxDataSize * 0.75;
					std::list<T*> units;
					unitCollection->GetUpdates(&units);
					MemoryStream* memoryStream = new MemoryStream();
					__size packageUnitsCount = 0;
					__size remainingUnitsCount = units.size();
					for (std::list<T*>::iterator i = units.begin(); i != units.end(); ++i)
					{
						T* unit = *i;
						marshalUnit(unit, memoryStream);
						packageUnitsCount++;
						remainingUnitsCount--;
						if (memoryStream->GetLength() >= packageSizeLimit || remainingUnitsCount == 0)
						{
							GatewayPackage* package = GatewayPackage::CreateDynamic(dataMarker);
							Marshaler::MarshalInt(unitType, package);
							Marshaler::MarshalSize(packageUnitsCount, package);
							memoryStream->CopyTo(package);
							gatewayClient->Send(package, false);
							__FREEOBJ(memoryStream);
							if (remainingUnitsCount > 0)
							{
								memoryStream = new MemoryStream();
								packageUnitsCount = 0;
							}
						}
					}
				}
		};

// ==================================================================================================================================================
		

// ==================================================================================================================================================
	}
}
