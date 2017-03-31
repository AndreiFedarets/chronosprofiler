#pragma once
#include "CallstackStorage.h"
#include "DaemonClient.h"
#include "CallstackProcessor.h"

class CCallstackProcessorPool
{
public:
	CCallstackProcessorPool(__uint capacity, CCallstackStorage* storage, CDaemonClient* daemonClient);
	~CCallstackProcessorPool(void);
private:
	__uint _capacity;
	CCallstackProcessor** _processors;
	CCallstackStorage* _storage;
};

