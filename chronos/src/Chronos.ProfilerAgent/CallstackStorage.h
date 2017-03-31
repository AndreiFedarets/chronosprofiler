#pragma once
#include "Lock.h"
#include "CallstackPage.h"

class CCallstackStorage
{
public:
	CCallstackStorage(void);
	~CCallstackStorage(void);
	void Push(CCallstackPage* page);
	CCallstackPage* Take();
	__bool Empty();
	__bool Full();
private:
	CMonitor _monitor;
	std::queue<CCallstackPage*>* _pages;
	volatile __int _pagesCount;
};

