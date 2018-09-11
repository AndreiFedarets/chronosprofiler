#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		Lock::Lock(CriticalSection* criticalSection)
			: _criticalSection(criticalSection)
		{
			_criticalSection->Enter();
		}

		Lock::~Lock(void)
		{
			_criticalSection->Leave();
		}
	}
}