#include "StdAfx.h"
#include "ExceptionStack.h"

CExceptionStack::CExceptionStack(CProfilerController* controller, CCallstack* callstack)
	: _controller(controller), _callstack(callstack), _exceptionStack(null)
{
}

CExceptionStack::~CExceptionStack(void)
{
	Flush();
}

void CExceptionStack::ExceptionThrown(CExceptionInfo* exceptionInfo)
{
	_currentExceptionInfo = exceptionInfo;
	CallRet(EventTypes::ExceptionThrown, exceptionInfo);
	if (_exceptionStack != null)
	{
		__FREEOBJ(_exceptionStack);
	}
	_exceptionStack = new std::queue<CFunctionInfo*>();
	_currentFunctionInfo = null;
	_catcherFunctionInfo = null;
}

///ExceptionSearchFunction==============================================================================================
void CExceptionStack::ExceptionSearchFunctionEnter(CFunctionInfo* functionInfo)
{
	if (functionInfo->IsTarget)
	{
		_exceptionStack->push(functionInfo);
	}
	_currentExceptionInfo->Stack->push(functionInfo);
}

void CExceptionStack::ExceptionSearchFunctionLeave()
{

}
///ExceptionSearchCatcher==============================================================================================
void CExceptionStack::ExceptionSearchCatcherFound(CFunctionInfo* functionInfo)
{
	if (functionInfo->IsTarget)
	{
		_catcherFunctionInfo = functionInfo;
	}
}

void CExceptionStack::ExceptionCatcherEnter(CFunctionInfo* functionInfo, CExceptionInfo* exceptionInfo)
{
	exceptionInfo->IsCatched = true;
	exceptionInfo->Catcher = functionInfo;
}

void CExceptionStack::ExceptionCatcherLeave()
{
	
}

CExceptionInfo* CExceptionStack::GetCurrentException()
{
	return _currentExceptionInfo;
}

///ExceptionUnwindFunction==============================================================================================
void CExceptionStack::ExceptionUnwindFunctionEnter(CFunctionInfo* functionInfo)
{
	if (functionInfo->IsTarget)
	{
		_currentFunctionInfo = _exceptionStack->front();
		_exceptionStack->pop();
	}
}

void CExceptionStack::ExceptionUnwindFunctionLeave()
{
	if (_currentFunctionInfo != null)
	{
		if (_currentFunctionInfo != _catcherFunctionInfo)
		{
			Ret(EventTypes::FunctionCall, _currentFunctionInfo);
		}
		_currentFunctionInfo = null;
	}
}

///======================================================================================================================
void CExceptionStack::Flush()
{
	if (_exceptionStack == null)
	{
		return;
	}
	while (!_exceptionStack->empty())
	{
		CFunctionInfo* functionInfo = _exceptionStack->front();
		_exceptionStack->pop();
		Ret(EventTypes::FunctionCall, functionInfo);
	}
	__FREEOBJ(_exceptionStack);
}

void CExceptionStack::Call(__byte eventType, CUnitBase* unit)
{
	if (_controller->LogFunctionCalls)
	{
		_callstack->Call(eventType, unit);
	}
}

void CExceptionStack::Ret(__byte eventType, CUnitBase* unit)
{
	if (_controller->LogFunctionCalls)
	{
		_callstack->Ret(eventType, unit);
	}
}

void CExceptionStack::CallRet(__byte eventType, CUnitBase* unit)
{
	if (_controller->LogFunctionCalls)
	{
		_callstack->CallRet(eventType, unit);
	}
}
