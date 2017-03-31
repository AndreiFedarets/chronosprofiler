#pragma once
#include <queue>
#include "Callstack.h"
#include "EventTypes.h"
#include "Units.h"
#include "FunctionManager.h"

class CExceptionStack
{
public:
	CExceptionStack(CProfilerController* controller, CCallstack* callstack);
	~CExceptionStack(void);
	void ExceptionThrown(CExceptionInfo* exceptionInfo);

	void ExceptionSearchFunctionEnter(CFunctionInfo* functionInfo);
	void ExceptionSearchFunctionLeave();

	void ExceptionUnwindFunctionEnter(CFunctionInfo* functionInfo);
	void ExceptionUnwindFunctionLeave();

	void ExceptionSearchCatcherFound(CFunctionInfo* functionInfo);
	void ExceptionCatcherEnter(CFunctionInfo* functionInfo, CExceptionInfo* exceptionInfo);
	void ExceptionCatcherLeave();

	void Flush();

	CExceptionInfo* GetCurrentException();
private:
	void Call(__byte eventType, CUnitBase* unit);
	void Ret(__byte eventType, CUnitBase* unit);
	void CallRet(__byte eventType, CUnitBase* unit);
	std::queue<CFunctionInfo*>* _exceptionStack;
	CCallstack* _callstack;
	CFunctionInfo* _currentFunctionInfo;
	CFunctionInfo* _catcherFunctionInfo;
	CExceptionInfo* _currentExceptionInfo;
	CProfilerController* _controller;
};

