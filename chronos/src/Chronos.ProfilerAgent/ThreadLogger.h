#pragma once
#include "Callstack.h"
#include "ExceptionStack.h"
#include "BaseStream.h"

class CThreadLogger
{
public:
	CThreadLogger(CProfilerController* controller, ICorProfilerInfo2* corProfilerInfo2, CFunctionManager* functionManager, __uint pageSize, CThreadInfo* threadInfo);
	~CThreadLogger(void);
	void Dispose();
	CCallstack* Callstack;
	CExceptionStack* ExceptionStack;
	CThreadInfo* ThreadInfo;
private:
	__bool _isDisposed;
};
