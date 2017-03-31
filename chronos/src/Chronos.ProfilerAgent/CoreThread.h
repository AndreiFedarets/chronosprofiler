#pragma once

struct ICoreThread
{
	virtual void Start(LPVOID parameter) = 0;
	virtual void Stop() = 0;
	virtual __bool IsAlive() = 0;
};

class CSingleCoreThread : public ICoreThread
{
public:
	CSingleCoreThread(LPTHREAD_START_ROUTINE entryPoint);
	~CSingleCoreThread(void);
	void Start(LPVOID parameter);
	void Stop();
	__bool IsAlive();
private:
	LPTHREAD_START_ROUTINE _entryPoint;
	LPVOID _parameter;
	HANDLE _threadHandle;
	DWORD _win32ThreadID;
};

class CMultiCoreThread : public ICoreThread
{
public:
	CMultiCoreThread(LPTHREAD_START_ROUTINE entryPoint, __uint threadsCount, LPVOID parameter);
	CMultiCoreThread(LPTHREAD_START_ROUTINE entryPoint, __uint threadsCount);
	~CMultiCoreThread(void);
	void Start(LPVOID parameter);
	void Stop();
	__bool IsAlive();
private:
	CSingleCoreThread** _threads;
	__uint _threadsCount;
};

