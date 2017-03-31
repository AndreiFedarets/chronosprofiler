#include "StdAfx.h"
#include "Monitor.h"

CMonitor::CMonitor(void)
{
	if (!InitializeCriticalSectionAndSpinCount(&_criticalSection, 10000))
	{
		_ASSERT(false);
	}
}

CMonitor::~CMonitor(void)
{
	DeleteCriticalSection(&_criticalSection);
}

void CMonitor::Lock()
{
	EnterCriticalSection(&_criticalSection);
}

void CMonitor::Unlock()
{
	LeaveCriticalSection(&_criticalSection);
}

