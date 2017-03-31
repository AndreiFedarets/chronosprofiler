#pragma once
#include "CoreThread.h"
#include "Lock.h"
#include "Task.h"

class CTaskWorker
{
public:
    CTaskWorker(__uint threadsCount);
    ~CTaskWorker(void);
    void Start();
    void DoWorkInternal();
    void Push(CTask* task);
    CTask* Take();
    void Stop();
private:
    __uint _threadsCount;
    ICoreThread* _workingThread;
    std::queue<CTask*>* _tasks;
    CMonitor _monitor;
    volatile __bool _doWork;
    volatile __bool _stopped;
};