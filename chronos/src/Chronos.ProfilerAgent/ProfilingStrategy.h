#pragma once
#include "ServiceContainer.h"

class IProfilingStrategy
{
public:
	virtual void Initialize(CServiceContainer* serviceContainer) = 0;
	virtual ~IProfilingStrategy(void) { }
};

