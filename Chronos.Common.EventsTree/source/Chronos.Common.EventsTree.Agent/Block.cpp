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
				Block::Block(void)
				{
					Event = null;
					Next = null;
					Prev = null;
					FirstChild = null;
					LastChild = null;
					Parent = null;
				}

				Block::~Block(void)
				{
				}

				void Block::Init(__byte* e)
				{
					Event = (MergedEvent*)e;
				}

				Block* Block::AppendChild(Block* block)
				{
					_ASSERT(block->Event->Depth - Event->Depth == 1);
					//It's first block in sequence
					if (FirstChild == null)
					{
						FirstChild = block;
						LastChild = FirstChild;
						block->Parent = this;
						return block;
					}
					//Try find block with the same token
					Block* currentChildBlock = FirstChild;
					while (currentChildBlock != null)
					{
						if (currentChildBlock->Event->Unit == block->Event->Unit && 
							currentChildBlock->Event->EventType == block->Event->EventType)
						{
							//Merge blocks
							currentChildBlock->Event->Hits += block->Event->Hits;
							currentChildBlock->Event->Time += block->Event->Time;
							return currentChildBlock;
						}
						currentChildBlock = currentChildBlock->Next;
					}
					//There is no blocks to merge into, append as last child
					LastChild->Next = block;
					block->Prev = LastChild;
					block->Parent = this;
					LastChild = block;
					return block;
				}

				void Block::GetCount(__int* count)
				{
					Block* currentChild = LastChild;
					while (currentChild != null)
					{
						currentChild->GetCount(count);
						currentChild = currentChild->Prev;
					}
					(*count)++;
				}

				__byte* Block::Save(__byte* buffer)
				{
					Block* currentChildBlock = FirstChild;
					memcpy(buffer, Event, sizeof(MergedEvent));
					buffer += sizeof(MergedEvent);
					while (currentChildBlock != null)
					{
						buffer = currentChildBlock->Save(buffer);
						currentChildBlock = currentChildBlock->Next;
					}
					return buffer;
				}
			}
		}
	}
}