#include "StdAfx.h"
#include "Block.h"
#include "NativeHelpers.h"

CBlock::CBlock(void)
	 : Data(null), Next(null), Prev(null), FirstChild(null), LastChild(null), Parent(null), Depth(-1), Token(0)
{
}

CBlock::~CBlock(void)
{
}

void CBlock::Init(__byte* e)
{
	Data = e;
	Token = CNativeHelpers::GetToken(Data);
	Depth = *(__short*)(Data + EVENT_DEPTH_OFFSET);
}

CBlock* CBlock::AppendChild(CBlock* block)
{
	_ASSERT(block->Depth - Depth == 1);
	//It's first block in sequence
	if (FirstChild == null)
	{
		FirstChild = block;
		LastChild = FirstChild;
		block->Parent = this;
		return block;
	}
	//Try find block with the same token
	CBlock* currentChildBlock = FirstChild;
	while (currentChildBlock != null)
	{
		if (currentChildBlock->Token == block->Token)
		{
			//Merge blocks
			CNativeHelpers::MergeTimeAndHits(currentChildBlock->Data, block->Data);
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

void CBlock::GetCount(__int* count)
{
	CBlock* currentChild = LastChild;
	while (currentChild != null)
	{
		currentChild->GetCount(count);
		currentChild = currentChild->Prev;
	}
	(*count)++;
}

__byte* CBlock::Save(__byte* buffer)
{
	CBlock* currentChildBlock = LastChild;
	while (currentChildBlock != null)
	{
		buffer = currentChildBlock->Save(buffer);
		currentChildBlock = currentChildBlock->Prev;
	}
	memcpy(buffer, Data, HL_EVENT_SIZE);
	buffer += HL_EVENT_SIZE;
	return buffer;
}
