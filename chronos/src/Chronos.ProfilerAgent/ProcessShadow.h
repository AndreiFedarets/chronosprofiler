#pragma once
#include "AppDomainManager.h"
#include "AssemblyManager.h"
#include "ModuleManager.h"
#include "ClassManager.h"
#include "FunctionManager.h"
#include "ThreadManager.h"
#include "ExceptionManager.h"
#include "ProfilerController.h"
#include "Timer.h"
#include "CoreThread.h"
#include "StackTracer.h"
#include "SqlRequestManager.h"

class CProcessShadow
{
public:
	CProcessShadow(ICorProfilerInfo2* corProfilerInfo2, CProfilerController* profilerClient);
	~CProcessShadow(void);
	CAppDomainManager* AppDomainManager;
	CAssemblyManager* AssemblyManager;
	CModuleManager* ModuleManager;
	CClassManager* ClassManager;
	CFunctionManager* FunctionManager;
	CSqlRequestManager* SqlRequestManager;
	CExceptionManager* ExceptionManager;
	CThreadManager* ThreadManager;
	CStackTracer* StackTracer;
	void Flush();
private:
	void Close();
	void WaitForFlush();
	volatile __bool _flushInProgress;
	volatile __bool _disposed;
};

