#pragma once
#include "UnitManagerBase.h"

class CModuleManager : public CUnitManagerBase<CModuleInfo>
{
public:
	CModuleManager(ICorProfilerInfo2* corProfilerInfo2);
	void Initialize(CModuleInfo* unit);
	__uint GetUnitType();
	void Serialize(CModuleInfo* unit, CBaseStream* stream);
};