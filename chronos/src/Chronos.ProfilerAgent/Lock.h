#pragma once
#include "Monitor.h"

class CLock
{
public:
	CLock(CMonitor* monitor);
	~CLock(void);
private:
	CMonitor* _monitor;
};

