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
				EventsPageHeader::EventsPageHeader()
				{
					PageType = EventsPageHeader::DetailedPageType;
				}

				/*EventsPageHeader::EventsPageHeader(EventsPageHeader& header)
				{
					PageType = header.PageType;
					Flag = header.Flag;
					ThreadId = header.ThreadId;
					EventsTreeGlobalId = header.EventsTreeGlobalId;
					EventsTreeLocalId = header.EventsTreeLocalId;
					BeginLifetime = header.BeginLifetime;
					EndLifetime = header.EndLifetime;
					PageIndex = header.PageIndex;
				}*/

				__bool EventsPageHeader::LastPage()
				{
					return Flag == EventsPageHeader::ClosePageFlag || Flag == EventsPageHeader::BreakPageFlag;
				}
			}
		}
	}
}