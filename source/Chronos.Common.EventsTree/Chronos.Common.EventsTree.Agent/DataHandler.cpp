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
				DataHandler::DataHandler(EventsTreeMergeCompletedCallback callback)
				{
					_merger = new EventsTreeMergerCollection(callback);
				}

				DataHandler::~DataHandler()
				{
					__FREEOBJ(_merger);
				}

				__bool DataHandler::HandlePackage(__byte* data, __uint size)
				{
					DetailedEventsPage* eventsPage = (DetailedEventsPage*)data;
					__ASSERT(sizeof(EventsPageHeader) + eventsPage->Header.EventsDataSize == size, L"DataHandler::HandlePackage: actual size of package is not equal to expected");
					_merger->PushPage(eventsPage);
					return false;
				}
			}
		}
	}
}