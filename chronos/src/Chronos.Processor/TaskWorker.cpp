#include "StdAfx.h"
#include "TaskWorker.h"



DWORD WINAPI DoWork(LPVOID parameter) 
{
    CTaskWorker* worker = (CTaskWorker*)parameter;
    worker->DoWorkInternal();
    return 0;
}

CTaskWorker::CTaskWorker(__uint threadsCount)
    : _threadsCount(threadsCount)
{
    _workingThread = new CMultiCoreThread(DoWork, threadsCount);
    _tasks = new std::queue<CTask*>();
}

CTaskWorker::~CTaskWorker(void)
{
    Stop();
    __FREEOBJ(_workingThread);
    __FREEOBJ(_tasks);
}

void CTaskWorker::Start()
{
    _stopped = false;
    _doWork = true;
    _workingThread->Start(this);
}

void CTaskWorker::DoWorkInternal()
{
    while (true)
    {
        CTask* task = Take();
        if (task == null)
        {
            if (_doWork)
            {
                Sleep(1);
            }
            else
            {
                return;
            }
        }
        else
        {
            task->Execute();
            __FREEOBJ(task);
        }
    }
}

void CTaskWorker::Push(CTask* task)
{
	if (_stopped)
	{
		_ASSERT(false);
	}
    CLock lock(&_monitor);
    _tasks->push(task);
}

CTask* CTaskWorker::Take()
{
    CTask* task = null;
    CLock lock(&_monitor);
    if (!_tasks->empty())
    {
        task = _tasks->front();
        _tasks->pop();
    }
    return task;
}

void CTaskWorker::Stop()
{
    if (!_stopped)
    {
        _stopped = true;
        _doWork = false;
        while (_workingThread->IsAlive())
        {
            Sleep(100);
        }
    }
}