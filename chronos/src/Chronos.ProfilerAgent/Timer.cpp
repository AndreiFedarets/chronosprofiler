#include "StdAfx.h"
#include "Timer.h"


DWORD WINAPI RefreshTimeInternal(LPVOID parameter)
{
	while (true)
	{
        CTimer::SuspendThread(1);
		CTimer::CurrentTime = CTimer::GetTime() - CTimer::BeginTime;
	}
}

void CTimer::Initialize()
{
	_timeThread = new CSingleCoreThread(RefreshTimeInternal);
	BeginTime = GetTime();
	_timeThread->Start(null);
#ifdef HIGH_RESOLUTION_SLEEP
    _timerHandle = CreateWaitableTimer(NULL, TRUE, NULL); 
#endif
}

void CTimer::Destroy()
{
	_timeThread->Stop();
#ifdef HIGH_RESOLUTION_SLEEP
    CloseHandle(_timerHandle);
#endif
}

__inline void CTimer::SuspendThread(__uint time)
{
#ifdef HIGH_RESOLUTION_SLEEP
    LARGE_INTEGER period;
    period.QuadPart = -(100000000*time); // Convert to 100 nanosecond interval, negative value indicates relative time
    SetWaitableTimer(_timerHandle, &period, 0, NULL, NULL, 0); 
    WaitForSingleObject(_timerHandle, INFINITE); 
#else
	Sleep(time);
#endif
}

__inline __uint CTimer::GetTime()
{
#ifdef HIGH_RESOLUTION_TIMER
	LARGE_INTEGER performanceFrequency;
	LARGE_INTEGER performanceCounter;
	QueryPerformanceFrequency(&performanceFrequency);
	QueryPerformanceCounter(&performanceCounter);
	double freq = 1000.0 / double(performanceFrequency.QuadPart);
	return static_cast<__uint>(double(performanceCounter.QuadPart) * freq); 
#else
	return GetTickCount();
#endif
}

volatile __uint CTimer::CurrentTime = 0;
volatile __uint CTimer::BeginTime = 0;
CSingleCoreThread* CTimer::_timeThread = null;

#ifdef HIGH_RESOLUTION_SLEEP
HANDLE CTimer::_timerHandle = NULL;
#endif