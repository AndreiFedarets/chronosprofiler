#pragma once
#include <Chronos.Agent.h>

#ifdef CHRONOS_COMMON_EVENTSTREE_EXPORT_API
#define CHRONOS_COMMON_EVENTSTREE_API __declspec(dllexport) 
#else
#define CHRONOS_COMMON_EVENTSTREE_API __declspec(dllimport) 
#endif

namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
			namespace EventsTree
			{
// ==================================================================================================================================================
				#pragma pack(push,1)
				struct CHRONOS_COMMON_EVENTSTREE_API EventsPageHeader
				{
					EventsPageHeader();
					__bool LastPage();

					__byte PageType;
					__byte Flag;
					__uint ThreadUid;
					__uint ThreadOsId;
					__ulong EventsTreeGlobalId;
					__ushort EventsTreeLocalId;
					__int BeginLifetime;
					__int EndLifetime;
					__int PageIndex;
					__uint EventsDataSize;

					enum PageType
					{
						DetailedPageType = 0,
						MergedPageType = 1,
						FinalPageType = 2
					};

					enum PageFlag
					{
						BreakPageFlag = 0,
						ClosePageFlag = 1,
						ContinuePageFlag = 2,
					};
				};
				#pragma pack(pop)

// ==================================================================================================================================================
				#pragma pack(push,4)
				class CHRONOS_COMMON_EVENTSTREE_API DetailedEventsPage
				{
					public:
						EventsPageHeader Header;
						__byte* GetEvents();
						static void FreeEventsPage(DetailedEventsPage* eventsPage);
				};
				#pragma pack(pop)
				
// ==================================================================================================================================================
				CHRONOS_COMMON_EVENTSTREE_API void EventsTreeLoggerCollection_LogEventEnter(__byte eventType, __uint unit);
				CHRONOS_COMMON_EVENTSTREE_API void EventsTreeLoggerCollection_LogEventLeave(__byte eventType, __uint unit);
				CHRONOS_COMMON_EVENTSTREE_API void EventsTreeLoggerCollection_LogEventEnterLeave(__byte eventType, __uint unit);
				/*EXTERN_C CHRONOS_COMMON_EVENTSTREE_API void EventsTreeLoggerCollection_LogEventEnter(__uint osThreadId, __byte eventType, __uint unit);
				EXTERN_C CHRONOS_COMMON_EVENTSTREE_API void EventsTreeLoggerCollection_LogEventLeave(__uint osThreadId, __byte eventType, __uint unit);
				EXTERN_C CHRONOS_COMMON_EVENTSTREE_API void EventsTreeLoggerCollection_LogEventEnterLeave(__uint osThreadId, __byte eventType, __uint unit);*/

				struct CHRONOS_COMMON_EVENTSTREE_API IEventsTreeLoggerCollection
				{
					virtual ~IEventsTreeLoggerCollection(void) { };

					virtual void LogEventEnter(__byte eventType, __uint unit) = 0;
					virtual void LogEventLeave(__byte eventType, __uint unit) = 0;
					virtual void LogEventEnterLeave(__byte eventType, __uint unit) = 0;
					
					virtual void LogEventEnter(__uint osThreadId, __byte eventType, __uint unit) = 0;
					virtual void LogEventLeave(__uint osThreadId, __byte eventType, __uint unit) = 0;
					virtual void LogEventEnterLeave(__uint osThreadId, __byte eventType, __uint unit) = 0;

					const static __guid ServiceToken;
				};

// ==================================================================================================================================================
				typedef void (__stdcall *EventsTreeMergeCompletedCallback)(__byte*, __uint);

				struct CHRONOS_COMMON_EVENTSTREE_API IEventsTreeMergerCollection
				{
					virtual ~IEventsTreeMergerCollection() { };
					virtual void PushPage(DetailedEventsPage* page) = 0;
				};

// ==================================================================================================================================================
			}
		}
	}
}