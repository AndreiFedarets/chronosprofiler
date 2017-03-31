#pragma once
#include "UnitManagerBase.h"

class CFunctionManager : public CUnitManagerBase<CFunctionInfo>
{
public:
	CFunctionManager(ICorProfilerInfo2* corProfilerInfo2);
	void Initialize(CFunctionInfo* unit);
	__uint GetUnitType();
	void Serialize(CFunctionInfo* unit, CBaseStream* stream);
};
