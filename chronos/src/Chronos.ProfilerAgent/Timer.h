#pragma once
#include "CoreThread.h"

class CTimer
{
public:
	static void Initialize();
	static void Destroy();
	static volatile __uint CurrentTime;
	static volatile __uint BeginTime;
    static __uint GetTime();
    static void SuspendThread(__uint time);
private:
	static CSingleCoreThread* _timeThread;
#ifdef HIGH_RESOLUTION_SLEEP
    static HANDLE _timerHandle;
#endif
};

