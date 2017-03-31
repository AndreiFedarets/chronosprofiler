#pragma once
#include "UnitManagerBase.h"

class CThreadManager : public CUnitManagerBase<CThreadInfo>
{
public:
	CThreadManager(ICorProfilerInfo2* corProfilerInfo2);
	void Initialize(CThreadInfo* unit);
	__uint GetUnitType();
	void Serialize(CThreadInfo* unit, CBaseStream* stream);
private:
	std::wstring _mainThreadName;
	std::wstring _finalizationThreadName;
	std::wstring _workerThreadName;
};
