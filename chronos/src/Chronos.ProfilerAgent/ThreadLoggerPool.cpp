#include "StdAfx.h"
#include "ThreadLoggerPool.h"

CThreadLoggerPool::CThreadLoggerPool(ICorProfilerInfo2* corProfilerInfo2, CProfilerController* profilerController, CFunctionManager* functionManager)
	: _corProfilerInfo2(corProfilerInfo2), _profilerController(profilerController), _functionManager(functionManager)
{
#ifdef THREAD_LOGGER_FAST_DICTIONARY
	_loggers = new CFastDictionary<CThreadLogger*>();
#else
	_loggers = new std::map<ThreadID, CThreadLogger*>();
#endif
}

CThreadLoggerPool::~CThreadLoggerPool(void)
{
	Dispose();
}

CThreadLogger* CThreadLoggerPool::GetThreadLogger(ThreadID threadID)
{
#ifdef THREAD_LOGGER_FAST_DICTIONARY
	return _loggers->find(threadID);
#else
	return (*_loggers)[threadID];
#endif
}

CThreadLogger* CThreadLoggerPool::GetCurrentThreadLogger()
{
#ifdef THREAD_LOGGER_TLS
	return CurrentLogger;
#endif
	//====================
	//Get current ThreadID
	//====================
	ThreadID threadID;
	_corProfilerInfo2->GetCurrentThreadID(&threadID);
	//=============================
	//Find ThreadLogger by ThreadID
	//=============================
#ifdef THREAD_LOGGER_FAST_DICTIONARY
	return _loggers->find(threadID);
#else
	return (*_loggers)[threadID];
#endif
}

CThreadLogger* CThreadLoggerPool::CreateThreadLogger(CThreadInfo* threadInfo)
{
	CLock lock(&_monitor);
	CThreadLogger* logger = new CThreadLogger(_profilerController, _corProfilerInfo2, _functionManager, _profilerController->CallPageSize, threadInfo);
	ThreadID threadID = (ThreadID)threadInfo->ManagedId;

#ifdef THREAD_LOGGER_FAST_DICTIONARY
	_loggers->insert(threadID, logger);
#else
	_loggers->insert(std::pair<UINT_PTR, CThreadLogger*>(threadID, logger));
#endif

#ifdef THREAD_LOGGER_TLS
	CurrentLogger = logger;
#endif
	return logger;
}

void CThreadLoggerPool::DestroyThreadLogger(ThreadID threadID)
{
	CLock lock(&_monitor);
	CThreadLogger* logger;
#ifdef THREAD_LOGGER_FAST_DICTIONARY
	logger = _loggers->find(threadID);
#else
	logger = (*_loggers)[threadID];
#endif
	_loggers->erase(threadID);
	logger->Dispose();
}

void CThreadLoggerPool::Dispose()
{
	CLock lock(&_monitor);
#ifdef THREAD_LOGGER_FAST_DICTIONARY
	__FREEOBJ(_loggers);
#else
	if (_loggers != null)
	{
		for (std::map<UINT_PTR, CThreadLogger*>::iterator i = _loggers->begin(); i != _loggers->end(); i++)
		{
			i->second->Dispose();
		}
		_loggers->clear();
		__FREEOBJ(_loggers);
	}
#endif
}

#ifdef THREAD_LOGGER_TLS
	CThreadLogger* CThreadLoggerPool::CurrentLogger;
#endif