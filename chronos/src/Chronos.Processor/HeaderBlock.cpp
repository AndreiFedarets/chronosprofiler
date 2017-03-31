#include "StdAfx.h"
#include "HeaderBlock.h"

CHeaderBlock::CHeaderBlock(CSLFFactory<CBlock>* blockFactory)
	 : _blockFactory(blockFactory), _header(null), _current(null)
{
}

CHeaderBlock::~CHeaderBlock(void)
{
	__FREEOBJ(_header);
}

void CHeaderBlock::Append(__byte* event)
{
	CBlock* block = _blockFactory->Next();
	block->Init(event);
	if (block->Depth < _current->Depth)
	{
		while (block->Depth != _current->Depth)
		{
			_current = _current->Parent;
		}
		while (_current->Next != null)
		{
			_current = _current->Next;
		}
	}
	if (block->Depth == _current->Depth)
	{
		_current = _current->Parent->AppendChild(block);
	}
	else
	{
		_current = _current->AppendChild(block);
	}
}

void CHeaderBlock::GetCount(__int* count)
{
	_header->GetCount(count);
	(*count)--;
}

void CHeaderBlock::Load(__byte* pageStart, __byte* pageEnd)
{
	_header = new CBlock();
	_header->Depth = (*(__short*)(pageEnd + EVENT_DEPTH_OFFSET)) - 1;
	_current = _header;
	while (pageEnd >= pageStart)
	{
		Append(pageEnd);
		pageEnd -= HL_EVENT_SIZE;
	}
}

__byte* CHeaderBlock::Save(__byte* page)
{
	CBlock* currentChildBlock = _header->LastChild;
	while (currentChildBlock != null)
	{
		page = currentChildBlock->Save(page);
		currentChildBlock = currentChildBlock->Prev;
	}
	return page;
}
