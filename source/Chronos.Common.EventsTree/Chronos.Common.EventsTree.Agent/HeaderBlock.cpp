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
				HeaderBlock::HeaderBlock(DynamicBlockFactory<Block>* blockFactory)
				{
					_blockFactory = blockFactory;
					_header = null;
					_current = null;
					_headerEvent = new MergedEvent();
					memset(_headerEvent, 0, sizeof(MergedEvent));
				}

				HeaderBlock::~HeaderBlock(void)
				{
					__FREEOBJ(_header);
					__FREEOBJ(_headerEvent);
				}

				void HeaderBlock::Append(__byte* event)
				{
					Block* block = _blockFactory->Next();
					block->Init(event);
					if (block->Event->Depth < _current->Event->Depth)
					{
						while (block->Event->Depth != _current->Event->Depth)
						{
							_current = _current->Parent;
						}
						while (_current->Next != null)
						{
							_current = _current->Next;
						}
					}
					if (block->Event->Depth == _current->Event->Depth)
					{
						_current = _current->Parent->AppendChild(block);
					}
					else
					{
						_current = _current->AppendChild(block);
					}
				}

				void HeaderBlock::GetCount(__int* count)
				{
					_header->GetCount(count);
					(*count)--;
				}

				void HeaderBlock::Load(__byte* pageStart, __byte* pageEnd)
				{
					_header = new Block();
					_header->Init((__byte*)_headerEvent);
					_headerEvent->Depth = -1;
					_current = _header;
					while (pageStart < pageEnd)
					{
						Append(pageStart);
						pageStart += sizeof(MergedEvent);
					}
				}

				__byte* HeaderBlock::Save(__byte* page)
				{
					Block* currentChildBlock = _header->FirstChild;
					while (currentChildBlock != null)
					{
						page = currentChildBlock->Save(page);
						currentChildBlock = currentChildBlock->Next;
					}
					return page;
				}
			}
		}
	}
}