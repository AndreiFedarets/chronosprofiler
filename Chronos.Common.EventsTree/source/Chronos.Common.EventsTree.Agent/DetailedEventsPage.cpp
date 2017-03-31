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
				__byte* DetailedEventsPage::GetEvents()
				{
					return ((__byte*)this) + sizeof(EventsPageHeader);
				}

				void DetailedEventsPage::FreeEventsPage(DetailedEventsPage* eventsPage)
				{
					if (eventsPage != null)
					{
						__byte* buffer = (__byte*)eventsPage;
						__FREEARR(buffer);
					}
				}
			}
		}
	}
}