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
				MergedEventsPage::MergedEventsPage(__bool isDisposable)
				{
					_depth = -1;
					_rootEventContainer = null;
					_isDisposable = isDisposable;
					_factory = new DynamicBlockFactory<MergedEventContainer>(false);
					for (__short i = 0; i < STACK_MAX_DEPTH; i++)
					{
						Tuple<MergedEventContainer*, __uint>* tuple = &(_stack[i]);
						tuple->Item1 = null;
						tuple->Item2 = 0;
					}
				}

				MergedEventsPage::~MergedEventsPage()
				{
					__FREEOBJ(_factory);
				}

				void MergedEventsPage::Append(DetailedEventsPage* eventsPage)
				{
					//if this is first page we should take header from it
					if (eventsPage->Header.PageIndex == 0)
					{
						//copy header from DetailedEventsPage
						Header = eventsPage->Header;
						//update page type
						Header.PageType = EventsPageHeader::MergedPageType;
					}
					else
					{
						Header.EndLifetime = eventsPage->Header.EndLifetime;
						Header.Flag = eventsPage->Header.Flag;
					}

					__byte* eventsBufferCursor = eventsPage->GetEvents();
					__byte* eventsBufferEnd = eventsPage->GetEvents() + eventsPage->Header.EventsDataSize;

					while (eventsBufferCursor < eventsBufferEnd)
					{
						__byte eventType = *eventsBufferCursor;
						switch (eventType & (__byte)0x80) //eventType & 10000000 <- take first bit only
						{
							//event enter
							case 0x00:
							{
								_depth++;
								EventEnter* eventEnter = (EventEnter*)eventsBufferCursor;
								MergedEventContainer* currentEventContainer = null;
								if (_depth == 0)
								{
									//this is first event in the stack
									currentEventContainer = _factory->Next();
									currentEventContainer->Initialize(eventEnter);
									_rootEventContainer = currentEventContainer;
								}
								else
								{
									//take parent event container
									MergedEventContainer* parentContainer = _stack[_depth - 1].Item1;
									currentEventContainer = parentContainer->ChildEventContainer;
									//if we will not find child to merge, then we need last child to link current eventEnter to it
									MergedEventContainer* lastChildContainer = currentEventContainer;
									//is this child first?
									if (currentEventContainer == null)
									{
										//this child is first. all we need - append it to the parent
										currentEventContainer = _factory->Next();
										currentEventContainer->Initialize(eventEnter);
										parentContainer->ChildEventContainer = currentEventContainer;
									}
									else
									{
										//try to find existing child to merge with current eventEnter
										while (currentEventContainer != null)
										{
											if (EventsEquals(currentEventContainer, eventEnter))
											{
												break;
											}
											lastChildContainer = currentEventContainer;
											currentEventContainer = currentEventContainer->NextEventContainer;
										}
										if (currentEventContainer == null)
										{
											//there is no existing event to merge with eventEnter
											currentEventContainer = _factory->Next();
											currentEventContainer->Initialize(eventEnter);
											lastChildContainer->NextEventContainer = currentEventContainer;
										}
									}
								}
								Tuple<MergedEventContainer*, __uint>* tuple = &(_stack[_depth]);
								tuple->Item1 = currentEventContainer;
								tuple->Item2 = eventEnter->BeginTime;
								eventsBufferCursor += sizeof(EventEnter);
								break;
							}
							//event leave
							case 0x80:
							{
								EventLeave* eventLeave = (EventLeave*)eventsBufferCursor;
								Tuple<MergedEventContainer*, __uint>* tuple = &(_stack[_depth]);
								tuple->Item1->Time += eventLeave->EndTime - tuple->Item2;
								tuple->Item1->Hits++;
								eventsBufferCursor += sizeof(EventLeave);
								_depth--;
								break;
							}
						}
					}
				}

				__inline __bool MergedEventsPage::EventsEquals(MergedEventContainer* eventContainer, EventEnter* eventEnter)
				{
					return eventContainer->Unit == eventEnter->Unit && eventContainer->EventType == eventEnter->EventType;
				}

				void MergedEventsPage::Save(MemoryStream* stream)
				{
					EventsPageHeader header = Header;
					stream->Write(&header, sizeof(EventsPageHeader));
					_rootEventContainer->Write(stream, 0);
					header.EventsDataSize = stream->GetLength() - sizeof(EventsPageHeader);
					stream->Seek(0);
					stream->Write(&header, sizeof(EventsPageHeader));
				}

				void MergedEventsPage::Reset(__uint factoryResetLimit)
				{
					_depth = -1;
					_rootEventContainer = null;
					if (_factory->GetPagesCount() > factoryResetLimit)
					{
						_factory->Reset();
					}
					_factory->Reset();
					for (__short i = 0; i < STACK_MAX_DEPTH; i++)
					{
						Tuple<MergedEventContainer*, __uint>* tuple = &(_stack[i]);
						tuple->Item1 = null;
						tuple->Item2 = 0;
					}
				}

				__bool MergedEventsPage::IsDisposable()
				{
					return _isDisposable;
				}
			}
		}
	}
}