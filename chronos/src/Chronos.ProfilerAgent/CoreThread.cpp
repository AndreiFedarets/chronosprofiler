#include "StdAfx.h"
#include "CoreThread.h"

CSingleCoreThread::CSingleCoreThread(LPTHREAD_START_ROUTINE entryPoint)
	: _entryPoint(entryPoint), _parameter(null), _threadHandle(null), _win32ThreadID(0)
{
}

CSingleCoreThread::~CSingleCoreThread(void)
{
	if (_threadHandle != null)
	{
		Stop();
	}
}

void CSingleCoreThread::Start(LPVOID parameter)
{
	if (_threadHandle != null)
	{
		return;
	}
	_parameter = parameter;
	_threadHandle = CreateThread(null, 0, _entryPoint, _parameter, 0, &_win32ThreadID);
	SetThreadPriority(_threadHandle, THREAD_PRIORITY_HIGHEST);
}

void CSingleCoreThread::Stop()
{
	if (_threadHandle == null)
	{
		return;
	}
	TerminateThread(_threadHandle, 0);
	_threadHandle = null;
}

__bool CSingleCoreThread::IsAlive()
{
	// Read thread's exit code.
	DWORD exitCode = 0;
	if(GetExitCodeThread(_threadHandle, &exitCode))
	{
		// if return code is STILL_ACTIVE,
		// then thread is live.
		return (exitCode == STILL_ACTIVE);
	}
    else
    {
        return false;
    }
}

//====================================================================================================================
CMultiCoreThread::CMultiCoreThread(LPTHREAD_START_ROUTINE entryPoint, __uint threadsCount)
	: _threadsCount(threadsCount)
{
	_threads = new CSingleCoreThread*[_threadsCount];
	for (__uint i = 0; i < _threadsCount; i++)
	{
		_threads[i] = new CSingleCoreThread(entryPoint);
	}
}

CMultiCoreThread::~CMultiCoreThread(void)
{
	if (_threads != null)
	{
		Stop();
	}
}

void CMultiCoreThread::Start(LPVOID parameter)
{
	for (__uint i = 0; i < _threadsCount; i++)
	{
		_threads[i]->Start(parameter);
	}
}

void CMultiCoreThread::Stop()
{
	for (__uint i = 0; i < _threadsCount; i++)
	{
		_threads[i]->Stop();
		__FREEOBJ(_threads[i]);
	}
	__FREEARR(_threads);
	_threadsCount = 0;
}

__bool CMultiCoreThread::IsAlive()
{
	if (_threads != null)
	{
		for (__uint i = 0; i < _threadsCount; i++)
		{
			if (_threads[i]->IsAlive())
			{
				return true;
			}
		}
	}
	return false;
}