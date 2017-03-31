#include "StdAfx.h"
#include "ProcessShadow.h"

CProcessShadow::CProcessShadow(ICorProfilerInfo2* corProfilerInfo2, CProfilerController* profilerController)
{
	_disposed = false;
	
	AppDomainManager = new CAppDomainManager(corProfilerInfo2);
	AppDomainManager->Connect(profilerController);

	AssemblyManager = new CAssemblyManager(corProfilerInfo2);
	AssemblyManager->Connect(profilerController);

	ModuleManager = new CModuleManager(corProfilerInfo2);
	ModuleManager->Connect(profilerController);

	ClassManager = new CClassManager(corProfilerInfo2);
	ClassManager->Connect(profilerController);

	FunctionManager = new CFunctionManager(corProfilerInfo2);
	FunctionManager->Connect(profilerController);
	
	SqlRequestManager = new CSqlRequestManager(corProfilerInfo2);
	SqlRequestManager->Connect(profilerController);
	
	ThreadManager = new CThreadManager(corProfilerInfo2);
	ThreadManager->Connect(profilerController);
	
	StackTracer = new CStackTracer(FunctionManager, corProfilerInfo2);

	ExceptionManager = new CExceptionManager(corProfilerInfo2, StackTracer);
	ExceptionManager->Connect(profilerController);
	
	_flushInProgress = false;
}

CProcessShadow::~CProcessShadow(void)
{
	if (_disposed)
	{
		return;
	}
	_disposed = true;
	WaitForFlush();
	Close();
	Flush();

	__FREEOBJ(AppDomainManager);
	__FREEOBJ(AssemblyManager);
	__FREEOBJ(ModuleManager);
	__FREEOBJ(ClassManager);
	__FREEOBJ(FunctionManager);
	__FREEOBJ(ThreadManager);
	__FREEOBJ(ExceptionManager);
}

void CProcessShadow::Flush()
{
	_flushInProgress = true;
	AppDomainManager->Flush();
	AssemblyManager->Flush();
	ModuleManager->Flush();
	ClassManager->Flush();
	FunctionManager->Flush();
	ThreadManager->Flush();
	ExceptionManager->Flush();
	_flushInProgress = false;
}

void CProcessShadow::Close()
{
	FunctionManager->CloseAll();
	ClassManager->CloseAll();
	ModuleManager->CloseAll();
	AssemblyManager->CloseAll();
	AppDomainManager->CloseAll();
	ThreadManager->CloseAll();
	ExceptionManager->CloseAll();
}

void CProcessShadow::WaitForFlush()
{
	while (_flushInProgress)
	{
		Sleep(5);
	}
}