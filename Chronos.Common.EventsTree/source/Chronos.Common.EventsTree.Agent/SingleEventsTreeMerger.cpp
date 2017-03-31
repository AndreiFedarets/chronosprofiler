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
				SingleEventsTreeMerger::SingleEventsTreeMerger(ICallback* mergeCompletedCallback)
				{
					_currentIndex = 0;
					_started = false;
					_mergeCompletedCallback = mergeCompletedCallback;
					_pages = new InterlockedContainer<DetailedEventsPage>[Capacity];
					ICallback* callback = new ThisCallback<SingleEventsTreeMerger>(this, &SingleEventsTreeMerger::MergeInternal);
					_mergingThread = new MultiCoreThread(callback, 3);
				}

				SingleEventsTreeMerger::~SingleEventsTreeMerger()
				{
					//wait while merging
					_started = false;
					while (_mergingThread->IsAlive())
					{
						Sleep(1);
					}
					__FREEOBJ(_mergingThread);
				}

				void SingleEventsTreeMerger::PushPage(DetailedEventsPage* page)
				{
					if (_started)
					{
						do
						{
							__uint currentIndex = InterlockedIncrement(&_currentIndex) % Capacity;
							page = _pages[currentIndex].SetValue(page);
						}
						while (page != null);
					}
					else
					{
						__ASSERT(false, L"SingleEventsTreeMerger::PushPage: attempt to push page when merger is stopped");
						DetailedEventsPage::FreeEventsPage(page);
					}
				}

				void SingleEventsTreeMerger::Start()
				{
					_started = true;
					_mergingThread->Start();
				}

				void SingleEventsTreeMerger::MergeInternal(void* parameter)
				{
					const __uint factoryResetLimit = 8; //count of pages in the factory
					const __byte attemptsToTakePage = 2;
					MergedEventsPage* mergedPage = new MergedEventsPage(false);
					while (_started)
					{
						DetailedEventsPage* detailedPage = null;
						for (__byte i = 0; i < attemptsToTakePage; i++)
						{
							while ((detailedPage = GetNextPage()) != null)
							{
								mergedPage->Append(detailedPage);
								DetailedEventsPage::FreeEventsPage(detailedPage);
								_mergeCompletedCallback->Call(mergedPage);
								mergedPage->Reset(factoryResetLimit);
							}
						}
						Sleep(10);
					}
					__FREEOBJ(mergedPage);
				}

				DetailedEventsPage* SingleEventsTreeMerger::GetNextPage()
				{
					__uint currentIndex = _currentIndex;
					currentIndex = currentIndex % (Capacity / 2);
					DetailedEventsPage* page = null;
					//------------------------------------------------
					// Check root element
					//------------------------------------------------
					page = GetPage(0);
					if (page != null)
					{
						return page;
					}
					//------------------------------------------------
					// Go forward first half of the array
					//------------------------------------------------
					for (__uint i = currentIndex; i > 0; i--)
					{
						page = GetPage(i);
						if (page != null)
						{
							return page;
						}
					}
					//------------------------------------------------
					// Go backward second half of the array
					//------------------------------------------------
					for (__uint i = currentIndex; i < Capacity; i++)
					{
						page = GetPage(i);
						if (page != null)
						{
							return page;
						}
					}
					return page;
				}

				__inline DetailedEventsPage* SingleEventsTreeMerger::GetPage(__uint index)
				{
					InterlockedContainer<DetailedEventsPage>* container = &_pages[index];
					DetailedEventsPage* page = null;
					if (container->Value != null)
					{
						page = container->SetValue(null);
					}
					return page;
				}
				
				const __uint SingleEventsTreeMerger::Capacity = 2000;
				
			}
		}
	}
}