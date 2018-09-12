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
				BlockedCallstack::BlockedCallstack(void)
				{
					_blockFactory = null;
					_header = null;
				}

				BlockedCallstack::~BlockedCallstack(void)
				{
					__FREEOBJ(_blockFactory);
					__FREEOBJ(_header);
				}

				__int BlockedCallstack::GetCount()
				{
					__int count = 0;
					_header->GetCount(&count);
					return count;
				}

				void BlockedCallstack::Load(__byte* page, __int pageSize)
				{
					__byte* pageStart = page;
					__byte* pageEnd = page + pageSize;
					//init blocks factory - to save performance we are creating all needed count of blocks in one request
					//capacity - count of events in the page
					_blockFactory = new DynamicBlockFactory<Block>(false);
					_header = new HeaderBlock(_blockFactory);
					_header->Load(pageStart, pageEnd);
				}

				__byte* BlockedCallstack::Save(__int* pageSize)
				{
					*pageSize = GetCount() * sizeof(MergedEvent);
					__byte* page = new __byte[*pageSize];
					_header->Save(page);
					return page;
				}
			}
		}
	}
}