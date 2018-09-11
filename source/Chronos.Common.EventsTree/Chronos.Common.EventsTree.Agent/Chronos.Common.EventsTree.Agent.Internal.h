#pragma once
#include "Chronos.Common.EventsTree.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
			namespace EventsTree
			{
// ==================================================================================================================================================
				#define STACK_MAX_DEPTH 32000
				//#define EVENT_DEPTH_OFFSET 1
		
// ==================================================================================================================================================
				#pragma pack(push,1)
				struct EventLeave
				{
					__byte EventType;
					__uint EndTime;
				};
				#pragma pack(pop)
				
// ==================================================================================================================================================
				#pragma pack(push,1)
				struct EventEnter
				{
					__byte EventType;
					__uint Unit;
					__uint BeginTime;
				};
				#pragma pack(pop)

// ==================================================================================================================================================
				#pragma pack(push,1)
				struct MergedEvent
				{
					__byte EventType;
					__short Depth;
					__uint Unit;
					__uint Time;
					__uint Hits;
				};
				#pragma pack(pop)

//==================================================================================================================================================
				struct MergedEventContainer
				{
					MergedEventContainer()
					{
						memset(this, 0, sizeof(MergedEventContainer));
					}
					
					MergedEventContainer(EventEnter* eventEnter, __short depth)
					{
						MergedEventContainer::MergedEventContainer();
						Initialize(eventEnter);
					}

					void Initialize(EventEnter* eventEnter)
					{
						EventType = eventEnter->EventType;
						Unit = eventEnter->Unit;
					}

					void Write(MemoryStream* stream, __short depth)
					{
						MergedEvent mergedEvent;
						mergedEvent.EventType = EventType;
						mergedEvent.Depth = depth;
						mergedEvent.Unit = Unit;
						mergedEvent.Time = Time;
						mergedEvent.Hits = Hits;
						stream->Write(&mergedEvent, sizeof(MergedEvent));
						MergedEventContainer* child = ChildEventContainer;
						while (child != null)
						{
							child->Write(stream, depth + 1);
							child = child->NextEventContainer;
						}
					}
					
					__byte EventType;
					__uint Unit;
					__uint Time;
					__uint Hits;
					MergedEventContainer* NextEventContainer;
					MergedEventContainer* ChildEventContainer;
				};

// ==================================================================================================================================================
				class MergedEventsPage
				{
					public:
						MergedEventsPage(__bool isDisposable);
						~MergedEventsPage();
						void Append(DetailedEventsPage* detailedEventsPage);
						EventsPageHeader Header;
						void Save(MemoryStream* stream);
						__bool IsDisposable();
						void Reset(__uint factoryResetLimit);

					private:
						__bool EventsEquals(MergedEventContainer* eventContainer, EventEnter* eventEnter);
					private:
						__bool _isDisposable;
						Tuple<MergedEventContainer*, __uint> _stack[STACK_MAX_DEPTH];
						MergedEventContainer* _rootEventContainer;
						DynamicBlockFactory<MergedEventContainer>* _factory;
						__short _depth;
				};

// ==================================================================================================================================================
				class Block
				{
					public:
						Block(void);
						~Block(void);
						void Init(__byte* e);
						Block* AppendChild(Block* block);
						void GetCount(__int* count);
						__byte* Save(__byte* buffer);
					public:
						MergedEvent* Event;
						Block* Prev;
						Block* Next;
						Block* FirstChild;
						Block* LastChild;
						Block* Parent;
				};
				
// ==================================================================================================================================================
				class HeaderBlock
				{
					public:
						HeaderBlock(DynamicBlockFactory<Block>* factory);
						~HeaderBlock(void);
						void Append(__byte* event);
						void GetCount(__int* count);
						void Load(__byte* pageStart, __byte* pageEnd);
						__byte* Save(__byte* page);
					private:
						DynamicBlockFactory<Block>* _blockFactory;
						Block* _header;
						Block* _current;
						MergedEvent* _headerEvent;
				};

// ==================================================================================================================================================
				class BlockedCallstack
				{
					public:
						BlockedCallstack(void);
						~BlockedCallstack(void);
						__int GetCount();
						void Load(__byte* page, __int pageSize);
						__byte* Save(__int* pageSize);
					private:
						DynamicBlockFactory<Block>* _blockFactory;
						HeaderBlock* _header;
				};

// ==================================================================================================================================================
				class ComplexEventsTreeMerger
				{
					public:
						ComplexEventsTreeMerger(ICallback* mergeCompletedCallback);
						~ComplexEventsTreeMerger();
						void PushPage(DetailedEventsPage* page);
						void Start();

					private:
						void MergeInternal(void* parameter);

					private:
						SingleCoreThread* _mergingThread;
						CriticalSection _pagesCriticalSection;
						InterlockedContainer<DetailedEventsPage>* _pages;
						ICallback* _mergeCompletedCallback;
						const static __uint Capacity;
				};
				
// ==================================================================================================================================================
				class SingleEventsTreeMerger
				{
					public:
						SingleEventsTreeMerger(ICallback* mergeCompletedCallback);
						~SingleEventsTreeMerger();
						void PushPage(DetailedEventsPage* page);
						void Start();
					private:
						void MergeInternal(void* parameter);
						DetailedEventsPage* GetNextPage();
						DetailedEventsPage* GetPage(__uint index);
					private:
						volatile __bool _started;
						IThread* _mergingThread;
						ICallback* _mergeCompletedCallback;
						InterlockedContainer<DetailedEventsPage>* _pages;
						volatile __uint _currentIndex;
						const static __uint Capacity;
				};

// ==================================================================================================================================================
				class EventsTreeMergerCollection : public IEventsTreeMergerCollection
				{
					public:
						EventsTreeMergerCollection(EventsTreeMergeCompletedCallback mergedCompletedCallback);
						~EventsTreeMergerCollection();
						void PushPage(DetailedEventsPage* page);
					private:
						void MergeCompletedInternal(void* parameter);
						ComplexEventsTreeMerger* TakeMerger(__ushort eventsTreeLocalId);
						void CloseMerger(__ushort eventsTreeLocalId);
						void WaitWhileMerging();
					private:
						CriticalSection _criticalSection;
						SingleEventsTreeMerger* _singleMerger;
						InterlockedContainer<ComplexEventsTreeMerger> _mergers[0xFFFF];
						EventsTreeMergeCompletedCallback _mergedCompletedCallback;
						ICallback* _mergeCompletedInternalCallback;
				};
				
// ==================================================================================================================================================
				class EventsTreeLogger
				{
					public:
						EventsTreeLogger(GatewayClient* gatewayClient, ProfilingTimer* profilingTimer, __byte dataMarker, __uint _threadUid,
										__uint threadOsId, __uint eventsBufferSize, __ushort eventsMaxDepth);
						~EventsTreeLogger(void);
						void Enter(__byte eventType, __uint unit);
						void Leave(__byte eventType, __uint unit);
						void EnterLeave(__byte eventType, __uint unit);
						__uint ThreadOsId;

					private:
						void InitializePage();
						void InitializePackage(__byte dataMarker, __uint eventsBufferSize);
						void FlushPage(__byte flag);
						void StartNewEventTree();
						void StartNewPage();
						void ResetCursor();

					private:
						ProfilingTimer* _profilingTimer;
						GatewayClient* _gatewayClient;
						__short _stackDepth;
						__bool _pageIsReady;
						__ushort _eventsMaxDepth;
						__uint _threadUid;
						GatewayPackage* _package;
						EventsPageHeader* _eventsPageHeader;
						__byte* _eventsBufferBegin;
						__byte* _eventsBufferCursor;
						__byte* _eventsBufferEnd;
						EventEnter* _stack;

						static volatile __ulong EventTreeGlobalId;
						static volatile __short EventTreeLocalId;
				};

// ==================================================================================================================================================
				class EventsTreeLoggerCollection : public IEventsTreeLoggerCollection
				{
					public:
						EventsTreeLoggerCollection(void);
						~EventsTreeLoggerCollection(void);
						HRESULT Initialize(GatewayClient* gatewayClient, ProfilingTimer* profilingTimer, __byte dataMarker, __uint eventsBufferSize, __int eventsMaxDepth);

						void LogEventEnter(__byte eventType, __uint unit);
						void LogEventLeave(__byte eventType, __uint unit);
						void LogEventEnterLeave(__byte eventType, __uint unit);

						void LogEventEnter(__uint threadOsId, __byte eventType, __uint unit);
						void LogEventLeave(__uint threadOsId, __byte eventType, __uint unit);
						void LogEventEnterLeave(__uint threadOsId, __byte eventType, __uint unit);
						void DestroyLogger();

						__inline EventsTreeLogger* GetOrCreateLogger(__uint osThreadId);
						__inline EventsTreeLogger* GetOrCreateLogger();
					public:
						static EventsTreeLoggerCollection* Instance;
					/*private:
						__forceinline EventsTreeLogger* GetOrCreateLogger(__uint osThreadId);
						__forceinline EventsTreeLogger* GetOrCreateLogger();*/
					private:
						GatewayClient* _gatewayClient;
						__byte _dataMarker;
						ProfilingTimer* _profilingTimer;
						CriticalSection _criticalSection;
						std::map<__uint, EventsTreeLogger*> _loggers;
						__uint _eventsBufferSize;
						__int _eventsMaxDepth;
						volatile __uint _lastThreadUid;

						__declspec(thread) static EventsTreeLogger* CurrentLogger;
				};
				
// ==================================================================================================================================================
				class ProfilingTypeAdapter : public IProfilingTypeAdapter
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
						EventsTreeLoggerCollection* _loggers;
						GatewayClient* _gatewayClient;
						__byte _dataMarker;
						ProfilingTimer* _profilingTimer;
						ProfilingTypeSettings* _settings;
				};

// ==================================================================================================================================================
				class DataHandler : public IDataHandler
				{
					public:
						DataHandler(EventsTreeMergeCompletedCallback callback);
						~DataHandler();
						__bool HandlePackage(__byte* data, __uint size);
					private:
						EventsTreeMergerCollection* _merger;
				};
				
// ==================================================================================================================================================
				/*template<typename T>
				class SLFFactory final
				{
					public:
						SLFFactory<T>(__int capacity)
						{
							_items = new T[capacity];
							_currentIndex = 0;
						}
						~SLFFactory<T>()
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
				*/
// ==================================================================================================================================================
				/*class Fragment
				{
					public:
						Fragment(void);
						~Fragment(void);
						void Init(MergedEvent* e);
						Fragment* AppendChild(Fragment* fragment);
						void GetCount(__uint* count);
						__byte* Save(__byte* buffer);
					public:
						MergedEvent Event;
						__long Token;
						Fragment* Prev;
						Fragment* Next;
						Fragment* FirstChild;
						Fragment* LastChild;
						Fragment* Parent;
				};*/

// ==================================================================================================================================================
				/*class HeaderFragment
				{
					public:
						HeaderFragment(SLFFactory<Fragment>* factory);
						~HeaderFragment(void);
						void Append(MergedEvent* event);
						void GetCount(__uint* count);
						void Load(MergedEvent* firstEvent, MergedEvent* lastEvent);
						__byte* Save(__byte* page);
					private:
						SLFFactory<Fragment>* _fragmentFactory;
						Fragment* _header;
						Fragment* _current;
				};*/

// ==================================================================================================================================================
				/*class FragmentedEventsTree
				{
					public:
						FragmentedEventsTree(void);
						~FragmentedEventsTree(void);
						__uint GetSize();
						__int GetCount();
						void LoadFull(MergedEvent* events, __uint eventsCount);
						void LoadSafe(MergedEvent* events, __uint eventsCount);
						__byte* WriteTo(__byte* buffer);
					private:
						void LoadInternal(MergedEvent* firstEvent, MergedEvent* lastEvent);
						SLFFactory<Fragment>* _fragmentFactory;
						std::vector<HeaderFragment*>* _headers;
						MergedEvent* _firstPeakEvents;
						__uint _firstPeakEventsCount;
				};*/

// ==================================================================================================================================================
				//class EventTreeNativeHelper
				//{
				//	public:
				//		static MergedEventsPage* MergeMergedPage(MergedEventsPage* mergedPage);
				//		static MergedEventsPage* MergeMergedPages(MergedEventsPage* leftPage, MergedEventsPage* rightPage);
				//		//static __long GetEventToken(MergedEvent* data);
				//		//static void MergeTimeAndHits(MergedEvent* target, MergedEvent* source);
				//		static MergedEvent* FindFirstPeak(MergedEvent* firstEvent, MergedEvent* lastEvent);
				//		static MergedEvent* FindLastPeak(MergedEvent* firstEvent, MergedEvent* lastEvent);
				//};
			}
		}
	}
}