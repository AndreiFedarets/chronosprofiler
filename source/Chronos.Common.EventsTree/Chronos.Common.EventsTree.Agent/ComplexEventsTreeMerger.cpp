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
				ComplexEventsTreeMerger::ComplexEventsTreeMerger(ICallback* mergeCompletedCallback)
				{
					_pages = new InterlockedContainer<DetailedEventsPage>[Capacity];
					_mergeCompletedCallback = mergeCompletedCallback;
					ICallback* callback = new ThisCallback<ComplexEventsTreeMerger>(this, &ComplexEventsTreeMerger::MergeInternal);
					_mergingThread = new SingleCoreThread(callback);
				}

				ComplexEventsTreeMerger::~ComplexEventsTreeMerger()
				{
					//TODO: delete thread
					__FREEOBJ(_mergingThread);
					for (__uint i = 0; i < Capacity; i++)
					{
						DetailedEventsPage* detailedPage = _pages[i].SetValue(null);
						__ASSERT(detailedPage == null, L"ComplexEventsTreeMerger::~ComplexEventsTreeMerger: queue contains non-processed pages");
						DetailedEventsPage::FreeEventsPage(detailedPage);
					}
					__FREEARR(_pages);
				}

				void ComplexEventsTreeMerger::Start()
				{
					_mergingThread->Start();
				}

				void ComplexEventsTreeMerger::MergeInternal(void* parameter)
				{
					MergedEventsPage* mergedPage = new MergedEventsPage(true);
					__uint globalPageIndex = 0;
					__uint currentPageIndex = 0;
					while (true)
					{
						DetailedEventsPage* detailedPage = _pages[currentPageIndex].SetValue(null);
						if (detailedPage == null)
						{
							Sleep(1);
						}
						else
						{
							__ASSERT(detailedPage->Header.PageIndex == globalPageIndex, L"ComplexEventsTreeMerger::MergeInternal: page index is different");
							if (currentPageIndex > 0)
							{
								__ASSERT(mergedPage->Header.EventsTreeGlobalId == detailedPage->Header.EventsTreeGlobalId, L"ComplexEventsTreeMerger::MergeInternal: events tree global id is different");
								__ASSERT(mergedPage->Header.ThreadUid == detailedPage->Header.ThreadUid, L"ComplexEventsTreeMerger::MergeInternal: thread id is different");
							}
							mergedPage->Append(detailedPage);
							DetailedEventsPage::FreeEventsPage(detailedPage);
							if (mergedPage->Header.LastPage())
							{
								_mergeCompletedCallback->Call(mergedPage);
								return;
							}
							globalPageIndex++;
							currentPageIndex = globalPageIndex % Capacity;
						}
					}
				}

				void ComplexEventsTreeMerger::PushPage(DetailedEventsPage* page)
				{
					__uint pageIndex = page->Header.PageIndex;
					pageIndex = pageIndex % Capacity;
					InterlockedContainer<DetailedEventsPage>* container = &_pages[pageIndex];
					//wait while container is busy
					while (container->Value != null)
					{
						Sleep(1);
					}
					DetailedEventsPage* previousPage = _pages[pageIndex].SetValue(page);
					__ASSERT(previousPage == null, L"ComplexEventsTreeMerger::PushPage: new overrided existing page");
				}

				const __uint ComplexEventsTreeMerger::Capacity = 1000;
				
				/*InterlockedContainer<DetailedEventsPage>* EventsTreeMerger::CreatePages(__uint capacity)
				{
					InterlockedContainer<DetailedEventsPage>* pages = new InterlockedContainer<DetailedEventsPage>[capacity];
					return pages;
				}*/
				
				/*void EventsTreeMerger::CheckCapacity(__uint requiredCapacity)
				{
					if (_capacity <= requiredCapacity)
					{
						Lock lock(&_pagesCriticalSection);
						if (_capacity <= requiredCapacity)
						{
							__uint newCapacity = _capacity * 2;
							if (requiredCapacity <= newCapacity)
							{
								newCapacity = requiredCapacity + 1;
							}
							InterlockedContainer<DetailedEventsPage>* pages = CreatePages(newCapacity);
							for (__uint i = 0; i < _capacity; i++)
							{
								pages[i] = _pages[i];
							}
							__FREEARR(_pages);
							_pages = pages;
							_capacity = newCapacity;
						}
					}
				}*/
			}
		}
	}
}