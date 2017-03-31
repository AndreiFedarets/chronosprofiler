#include "stdafx.h"
#include "Chronos.Common.EventsTree.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
			namespace EventsTree
			{
				EventsTreeMergerCollection::EventsTreeMergerCollection(EventsTreeMergeCompletedCallback mergedCompletedCallback)
				{
					_mergedCompletedCallback = mergedCompletedCallback;
					_mergeCompletedInternalCallback = new ThisCallback<EventsTreeMergerCollection>(this, &EventsTreeMergerCollection::MergeCompletedInternal);
					//for (__uint i = 0; i < 0xFFFF; i++)
					//{
					//	_mergers[i] = new InterlockedContainer<ComplexEventsTreeMerger>();
					//}
					_singleMerger = new SingleEventsTreeMerger(_mergeCompletedInternalCallback);
					_singleMerger->Start();
				}

				EventsTreeMergerCollection::~EventsTreeMergerCollection()
				{
					WaitWhileMerging();
					__FREEOBJ(_singleMerger);
					__FREEOBJ(_mergeCompletedInternalCallback);
					Lock lock(&_criticalSection);
					for (__uint i = 0; i < 0xFFFF; i++)
					{
						InterlockedContainer<ComplexEventsTreeMerger>* container = &(_mergers[i]);
						ComplexEventsTreeMerger* merger = container->SetValue(null);
						__ASSERT(merger == null, L"EventsTreeMergerCollection::~EventsTreeMergerCollection: some merger is not completed yet");
						__FREEOBJ(merger);
					}
				}

				void EventsTreeMergerCollection::PushPage(DetailedEventsPage* page)
				{
					ComplexEventsTreeMerger* merger = null;
					if (page->Header.PageIndex == 0)
					{
						switch (page->Header.Flag)
						{
							case EventsPageHeader::BreakPageFlag:
								DetailedEventsPage::FreeEventsPage(page);
								break;
							case EventsPageHeader::ContinuePageFlag:
								merger = TakeMerger(page->Header.EventsTreeLocalId);
								merger->PushPage(page);
								break;
							//Header.PageIndex == 0 && Header.Flag == EventsPageHeader::ClosePageFlag
							//means that this is single-page-tree
							case EventsPageHeader::ClosePageFlag:
								_singleMerger->PushPage(page);
								break;
						}
					}
					else
					{
						merger = TakeMerger(page->Header.EventsTreeLocalId);
						merger->PushPage(page);
					}
				}

				ComplexEventsTreeMerger* EventsTreeMergerCollection::TakeMerger(__ushort eventsTreeLocalId)
				{
					InterlockedContainer<ComplexEventsTreeMerger>* container = &(_mergers[eventsTreeLocalId]);
					ComplexEventsTreeMerger* merger = const_cast<ComplexEventsTreeMerger*>(container->Value);
					if (merger == null)
					{
						Lock lock(&_criticalSection);
						merger = const_cast<ComplexEventsTreeMerger*>(container->Value);
						if (merger == null)
						{
							merger =  new ComplexEventsTreeMerger(_mergeCompletedInternalCallback);
							container->SetValue(merger);
							merger->Start();
						}
					}
					return merger;
				}
				
				void EventsTreeMergerCollection::MergeCompletedInternal(void* parameter)
				{
					MergedEventsPage* mergedPage = reinterpret_cast<MergedEventsPage*>(parameter);
					if (mergedPage->Header.Flag == EventsPageHeader::ClosePageFlag)
					{
						MemoryStream stream;
						mergedPage->Save(&stream);
						Buffer* buffer = stream.ToArray();
						_mergedCompletedCallback(buffer->Data, buffer->Size);
						//page->Header->RootEvent = EventFrameHelper::GetRootEvent(mergedPage->Data, mergedPage->DataSize);
						//result.RootEvent = EventFrameHelper::GetRootEvent(mergedPage->Data, mergedPage->DataSize);
					}
					if (mergedPage->IsDisposable())
					{
						__uint eventsTreeLocalId = mergedPage->Header.EventsTreeLocalId;
						CloseMerger(eventsTreeLocalId);
						__FREEOBJ(mergedPage);
					}
				}

				void EventsTreeMergerCollection::CloseMerger(__ushort eventsTreeLocalId)
				{
					InterlockedContainer<ComplexEventsTreeMerger>* container = &(_mergers[eventsTreeLocalId]);
					ComplexEventsTreeMerger* merger = container->SetValue(null);
					__ASSERT(merger != null, L"EventsTreeMergerCollection::CloseMerger: appropriate merger was not found");
					if (merger != null)
					{
						__FREEOBJ(merger);
					}
				}

				void EventsTreeMergerCollection::WaitWhileMerging()
				{
					/*while (HasPages())
					{
						Sleep(100);
					}
					_started = false;
					_merging = false;
					while (_preprocessThread->IsAlive())
					{
						Sleep(100);
					}
					_taskWorker->Stop();*/
				}

			}
		}
	}
}