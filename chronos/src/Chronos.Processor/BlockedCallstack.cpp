#include "StdAfx.h"
#include "BlockedCallstack.h"


CBlockedCallstack::CBlockedCallstack(void)
	: _blockFactory(null), _headers(new std::vector<CHeaderBlock*>()), _firstPeakBlock(null), _firstPeakBlockSize(0)
{
}

CBlockedCallstack::~CBlockedCallstack(void)
{
	__FREEOBJ(_blockFactory);
	while (!_headers->empty())
	{
		CHeaderBlock* header = _headers->back();
		_headers->pop_back();
		__FREEOBJ(header);
	}
	__FREEOBJ(_headers);
	__FREEARR(_firstPeakBlock);
}

__int CBlockedCallstack::GetCount()
{
	__int count = 0;
	std::vector<CHeaderBlock*>::iterator i = _headers->begin();
	for (i; i != _headers->end(); ++i)
	{
		(*i)->GetCount(&count);
	}
	return count;
}

void CBlockedCallstack::LoadFull(__byte* page, __int pageSize)
{
	__byte* pageStart = page;
	__byte* pageEnd = page + pageSize;
	LoadInternal(pageStart, pageEnd);
}

void CBlockedCallstack::LoadSafe(__byte* page, __int pageSize)
{
	//we should find first peak and save it to prevent merging, because it is on border
	__byte* pageStart = page;
	__byte* pageEnd = page + pageSize;
	__byte* firstPeak = CNativeHelpers::FindFirstPeak(pageStart, pageEnd);
	_firstPeakBlockSize = firstPeak - pageStart;
	_firstPeakBlockSize += HL_EVENT_SIZE;
	_firstPeakBlock = new __byte[_firstPeakBlockSize];
	memcpy(_firstPeakBlock, pageStart, _firstPeakBlockSize);
	pageStart = pageStart + _firstPeakBlockSize;
	LoadInternal(pageStart, pageEnd);
}

void CBlockedCallstack::LoadInternal(__byte* pageStart, __byte* pageEnd)
{
	//init blocks factory - to save performance we are creating all needed count of blocks in one request
	//capacity - count of events in the page
	__int capacity = (__int)((pageEnd - pageStart) / HL_EVENT_SIZE);
	_blockFactory = new CSLFFactory<CBlock>(capacity);
	__byte* currentPeak = null;
	//to start merging we need peaks - events which have least depth
	//last peak because events in the page are reversed 
	while ((currentPeak = CNativeHelpers::FindLastPeak(pageStart, pageEnd)) != null)
	{
		CHeaderBlock* header = new CHeaderBlock(_blockFactory);
		header->Load(pageStart, currentPeak);
		_headers->push_back(header);
		pageStart = currentPeak + HL_EVENT_SIZE;
	}
}

__byte* CBlockedCallstack::Save(__int* pageSize)
{
	*pageSize = GetCount() * HL_EVENT_SIZE + _firstPeakBlockSize;
	__byte* page = new __byte[*pageSize];
	__byte* pageOffset = page;
	if (_firstPeakBlockSize > 0)
	{
		memcpy(pageOffset, _firstPeakBlock, _firstPeakBlockSize);
		pageOffset = pageOffset + _firstPeakBlockSize;
	}
	std::vector<CHeaderBlock*>::iterator i = _headers->begin();
	for (i; i != _headers->end(); i++)
	{
		pageOffset = (*i)->Save(pageOffset);
	}
	return page;
}
