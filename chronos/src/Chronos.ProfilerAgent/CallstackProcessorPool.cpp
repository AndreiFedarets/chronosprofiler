#include "StdAfx.h"
#include "CallstackProcessorPool.h"


CCallstackProcessorPool::CCallstackProcessorPool(__uint capacity, CCallstackStorage* storage, CDaemonClient* daemonClient)
	: _capacity(capacity), _storage(storage), _processors(new CCallstackProcessor*[capacity])
{
	for (__uint i = 0; i < _capacity; i++)
	{
		CBaseStream* stream = daemonClient->QueryThreadStream();
		_processors[i] = new CCallstackProcessor(stream, _storage);
	}
}

CCallstackProcessorPool::~CCallstackProcessorPool(void)
{
	for (__uint i = 0; i < _capacity; i++)
	{
		__FREEOBJ(_processors[i]);
	}
	__FREEARR(_processors);
}
