#pragma once
#include "ProfilerController.h"
#include "ThreadLogger.h"
#include "Lock.h"
#include "CallstackProcessor.h"
#include "FastDictionary.h"

class CThreadLoggerPool
{
public:
	CThreadLoggerPool(ICorProfilerInfo2* corProfilerInfo2, CProfilerController* profilerController, CFunctionManager* functionManager);
	~CThreadLoggerPool(void);
	CThreadLogger* GetThreadLogger(ThreadID threadID);
	CThreadLogger* GetCurrentThreadLogger();
	CThreadLogger* CreateThreadLogger(CThreadInfo* threadInfo);
	void DestroyThreadLogger(ThreadID threadID);
	void Dispose();
#ifdef THREAD_LOGGER_TLS
	__declspec(thread) static CThreadLogger* CurrentLogger;
#endif
private:
	ICorProfilerInfo2* _corProfilerInfo2;
	CTimer* _timer;
	CFunctionManager* _functionManager;
#ifdef THREAD_LOGGER_FAST_DICTIONARY
	CFastDictionary<CThreadLogger*>* _loggers;
#else
	std::map<ThreadID, CThreadLogger*>* _loggers;
#endif

	CMonitor _monitor;
	CProfilerController* _profilerController;
};