#include "StdAfx.h"
#include "ThreadLogger.h"
#include "FunctionManager.h"

CThreadLogger::CThreadLogger(CProfilerController* controller, ICorProfilerInfo2* corProfilerInfo2, CFunctionManager* functionManager, __uint pageSize, CThreadInfo* threadInfo)
	: Callstack(null), ExceptionStack(null), ThreadInfo(threadInfo)
{
	if (controller->LogFunctionCalls)
	{
		Callstack = new CCallstack(controller, pageSize, threadInfo->Id);
	}
	if (controller->LogExceptions)
	{
		ExceptionStack = new CExceptionStack(controller, Callstack);
	}
}

CThreadLogger::~CThreadLogger(void)
{
	Dispose();
}

void CThreadLogger::Dispose()
{
	__FREEOBJ(ExceptionStack);
	__FREEOBJ(Callstack);
}