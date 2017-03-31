#include "StdAfx.h"
#include "Lock.h"


CLock::CLock(CMonitor* monitor)
	: _monitor(monitor)
{
	_monitor->Lock();
}

CLock::~CLock(void)
{
	_monitor->Unlock();
}
