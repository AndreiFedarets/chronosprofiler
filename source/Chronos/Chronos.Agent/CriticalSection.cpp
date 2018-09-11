#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		CriticalSection::CriticalSection(void)
		{
			if (!InitializeCriticalSectionAndSpinCount(&_criticalSection, 10000))
			{
				__ASSERT(false, L"CriticalSection::CriticalSection: failed to initialize critical section");
			}
		}

		CriticalSection::~CriticalSection(void)
		{
			DeleteCriticalSection(&_criticalSection);
		}

		void CriticalSection::Enter()
		{
			EnterCriticalSection(&_criticalSection);
		}

		void CriticalSection::Leave()
		{
			LeaveCriticalSection(&_criticalSection);
		}
	}
}