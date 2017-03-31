#include "stdafx.h"
#include "Chronos.Common.EventsTree.Agent.Internal.h"
#include <Winnt.h>

namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
			namespace EventsTree
			{
				EventsTreeLogger::EventsTreeLogger(GatewayClient* gatewayClient, ProfilingTimer* profilingTimer, __byte dataMarker,
					__uint threadUid, __uint threadOsId, __uint eventsBufferSize, __ushort eventsMaxDepth)
				{
					_stackDepth = -1;
					_pageIsReady = false;
					_gatewayClient = gatewayClient;
					_profilingTimer = profilingTimer;
					_eventsMaxDepth = eventsMaxDepth;
					ThreadOsId = threadOsId;
					_threadUid = threadUid;
					_stack = new EventEnter[eventsMaxDepth];
					memset(_stack, 0, sizeof(EventEnter) * eventsMaxDepth);
					InitializePackage(dataMarker, eventsBufferSize);
				}

				EventsTreeLogger::~EventsTreeLogger(void)
				{
					if (_pageIsReady)
					{
						//unexpected situation - logger is going to be released, but the tree is not completed
						//we should notify client that this event tree is broken
						FlushPage(EventsPageHeader::BreakPageFlag);
					}
					__FREEOBJ(_stack);
					__FREEOBJ(_package);
				}
				
				void EventsTreeLogger::InitializePackage(__byte dataMarker, __uint eventsBufferSize)
				{
					//calculate events page size
					__uint pageSize = sizeof(EventsPageHeader) + eventsBufferSize;
					//create package
					_package = GatewayPackage::CreateStatic(dataMarker, pageSize);
					//get pointer to package data
					__byte* packageData = _package->GetData();
					//get EventsPageHeader from package data
					_eventsPageHeader = (EventsPageHeader*)packageData;
					//offset to events buffer in package data
					packageData += sizeof(EventsPageHeader);
					//get events buffer
					_eventsBufferBegin = packageData;
					_eventsBufferEnd = _eventsBufferBegin + eventsBufferSize;
					_eventsBufferCursor = _eventsBufferBegin;
					//init global fields of events page
					_eventsPageHeader->PageType = EventsPageHeader::DetailedPageType;
					_eventsPageHeader->ThreadUid = _threadUid;
					_eventsPageHeader->ThreadOsId = ThreadOsId;
					StartNewEventTree();
					ResetCursor();
				}
				
				void EventsTreeLogger::Enter(__byte eventType, __uint unit)
				{
					_stackDepth++;
					if (_stackDepth >= _eventsMaxDepth)
					{
						return;
					}
					if (!_pageIsReady)
					{
						InitializePage();
					}
					EventEnter eventEnter;
					//set EventType
					eventEnter.EventType = eventType;
					//set UnitId
					eventEnter.Unit = unit;
					//set Time (current)
					eventEnter.BeginTime = _profilingTimer->CurrentTime;
					//save event
					_stack[_stackDepth] = eventEnter;
					//check if we are out of boundaries of events buffer
					if (_eventsBufferCursor + sizeof(EventEnter) > _eventsBufferEnd)
					{
						FlushPage(EventsPageHeader::ContinuePageFlag);
					}
					//write event to page
					*((EventEnter*)_eventsBufferCursor) = eventEnter;
					_eventsBufferCursor += sizeof(EventEnter);
				}

				void EventsTreeLogger::Leave(__byte eventType, __uint unit)
				{
					if (_stackDepth >= _eventsMaxDepth)
					{
						_stackDepth--;
						return;
					}
					//TODO: why should we check that _stackDepth less than 0 ?
					if (_stackDepth < 0)
					{
						return;
					}
					if (!_pageIsReady)
					{
						InitializePage();
					}
					EventLeave eventLeave;
					//set EventType with flag 'leave' - first bit of the
					eventLeave.EventType = eventType | (__byte)0x80; //event leave flag (bin): 1000 0000
					//set Time (current)
					eventLeave.EndTime = _profilingTimer->CurrentTime;
					
					EventEnter eventEnter = _stack[_stackDepth];
					if (eventEnter.EventType != eventType || eventEnter.Unit != unit)
					{
						__ASSERT(false, L"EventsTreeLogger::Leave: events tree is broken");
					}

					//check if we are out of boundaries of events buffer
					if (_eventsBufferCursor + sizeof(EventLeave) > _eventsBufferEnd)
					{
						FlushPage(EventsPageHeader::ContinuePageFlag);
					}
					//write event to page
					*((EventLeave*)_eventsBufferCursor) = eventLeave;
					_eventsBufferCursor += sizeof(EventLeave);

					_stackDepth--;
					//Check that this is end of the tree
					if (_stackDepth == -1)
					{
						FlushPage(EventsPageHeader::ClosePageFlag);
					}
				}

				void EventsTreeLogger::EnterLeave(__byte eventType, __uint unit)
				{
					Enter(eventType, unit);
					Leave(eventType, unit);
				}

				void EventsTreeLogger::InitializePage()
				{
					_eventsPageHeader->BeginLifetime = _profilingTimer->CurrentTime;
					_pageIsReady = true;
				}

				void EventsTreeLogger::ResetCursor()
				{
					_eventsBufferCursor = _eventsBufferBegin;
				}

				void EventsTreeLogger::FlushPage(__byte flag)
				{
					_eventsPageHeader->Flag = flag;
					_eventsPageHeader->EndLifetime = _profilingTimer->CurrentTime;

					__uint eventsDataSize = (__uint)(_eventsBufferCursor - _eventsBufferBegin);
					_eventsPageHeader->EventsDataSize = eventsDataSize;

					_package->SetDataSize(sizeof(EventsPageHeader) + eventsDataSize);
					_gatewayClient->Send(_package);

					//Is this page the last in the tree?
					if (flag != EventsPageHeader::ContinuePageFlag)
					{
						StartNewEventTree();
					}
					else
					{
						StartNewPage();
					}
					ResetCursor();
					_pageIsReady = false;
				}

				void EventsTreeLogger::StartNewEventTree()
				{
					_eventsPageHeader->PageIndex = 0;
					_eventsPageHeader->EventsTreeGlobalId = InterlockedIncrement(&EventTreeGlobalId);
					_eventsPageHeader->EventsTreeLocalId = _InterlockedIncrement16(&EventTreeLocalId);
					/*if (_eventsPageHeader->EventsTreeLocalId > EventTreeLocalId)
					{
						MessageBox(null, L"Overflow", null, MB_OK);
					}*/
				}

				void EventsTreeLogger::StartNewPage()
				{
					//increase page index
					_eventsPageHeader->PageIndex++;
				}

				volatile __ulong EventsTreeLogger::EventTreeGlobalId = 0;
				volatile __short EventsTreeLogger::EventTreeLocalId = 0;
			}
		}
	}
}
