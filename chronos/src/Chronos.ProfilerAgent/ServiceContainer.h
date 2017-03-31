#pragma once
#include <Windows.h>
#include "Macro.h"
#include "Lock.h"
#include "Monitor.h"

class CServiceContainer
{
public:
	CServiceContainer(void);
	bool ResolveService(GUID serviceToken, void** service);
	bool RegisterService(GUID serviceToken, void* service);
	bool UnregisterService(GUID serviceToken);
	~CServiceContainer(void);
private:
	std::map<GUID, void*> _services;
	CMonitor _monitor;
};

