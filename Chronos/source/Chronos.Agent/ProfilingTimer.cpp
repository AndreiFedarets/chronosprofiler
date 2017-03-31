#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		ProfilingTimer::ProfilingTimer()
		{
			ThisCallback<ProfilingTimer>* callback = new ThisCallback<ProfilingTimer>(this, &ProfilingTimer::StartTimeRefreshing);
			_timeThread = new SingleCoreThread(callback);
			_beginTime = GetTime();
			_timeThread->Start();
		}

		ProfilingTimer::~ProfilingTimer()
		{
			_timeThread->Stop();
			__FREEOBJ(_timeThread);
		}

		void ProfilingTimer::UpdateTime()
		{
			__uint newTime = GetTime();
			CurrentTime = newTime - _beginTime;
		}
		
		void ProfilingTimer::StartTimeRefreshing(void* parameter)
		{
			while (true)
			{
				Sleep(1);
				UpdateTime();
			}
		}

		__forceinline __uint ProfilingTimer::GetTime()
		{
			LARGE_INTEGER performanceFrequency;
			LARGE_INTEGER performanceCounter;
			QueryPerformanceFrequency(&performanceFrequency);
			QueryPerformanceCounter(&performanceCounter);
			double freq = 1000.0 / double(performanceFrequency.QuadPart);
			return static_cast<__uint>(double(performanceCounter.QuadPart) * freq); 
		}

		const __guid ProfilingTimer::ServiceToken = Converter::ConvertStringToGuid(L"{42DE49E0-A823-4E64-A419-CD5402B4D9E4}");
	}
}