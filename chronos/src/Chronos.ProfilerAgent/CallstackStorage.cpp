#include "StdAfx.h"
#include "CallstackStorage.h"

CCallstackStorage::CCallstackStorage(void)
	: _pages(new std::queue<CCallstackPage*>()), _pagesCount(0)
{
}

CCallstackStorage::~CCallstackStorage(void)
{
	__FREEOBJ(_pages);
}

void CCallstackStorage::Push(CCallstackPage* page)
{
	CLock lock(&_monitor);
	_pages->push(page);
	_pagesCount++;
}

CCallstackPage* CCallstackStorage::Take()
{
	CLock lock(&_monitor);
	CCallstackPage* page = null;
	if (!_pages->empty())
	{
		page = _pages->front();
		_pages->pop();
		_pagesCount--;
	}
	return page;
}

__bool CCallstackStorage::Empty()
{
	CLock lock(&_monitor);
	return _pages->empty();
}

__bool CCallstackStorage::Full()
{
	return _pagesCount >= 100;
}
