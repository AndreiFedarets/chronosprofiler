#pragma once
#include "FunctionManager.h"

struct CTraceSnapshotContext
{
	CTraceSnapshotContext(CFunctionManager* functionManager, std::queue<CFunctionInfo*>* stack)
	{
		Stack = stack;
		FunctionManager = functionManager;
	}
	std::queue<CFunctionInfo*>* Stack;
	CFunctionManager* FunctionManager;
};

class CStackTracer
{
public:
	CStackTracer(CFunctionManager* functionManager, ICorProfilerInfo2* corProfilerInfo2);
	~CStackTracer(void);
	void DumpStack(std::queue<CFunctionInfo*>* stack);
private:
	CFunctionManager* _functionManager;
	ICorProfilerInfo2* _corProfilerInfo2;
};

