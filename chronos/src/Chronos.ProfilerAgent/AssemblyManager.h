#pragma once
#include "UnitManagerBase.h"

class CAssemblyManager : public CUnitManagerBase<CAssemblyInfo>
{
public:
	CAssemblyManager(ICorProfilerInfo2* corProfilerInfo2);
	void Initialize(CAssemblyInfo* unit);
	__uint GetUnitType();
	void Serialize(CAssemblyInfo* unit, CBaseStream* stream);
};