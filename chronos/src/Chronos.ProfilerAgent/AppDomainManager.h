#pragma once
#include "UnitManagerBase.h"

class CAppDomainManager : public CUnitManagerBase<CAppDomainInfo>
{
public:
	CAppDomainManager(ICorProfilerInfo2* corProfilerInfo2);
	void Initialize(CAppDomainInfo* unit);
	__uint GetUnitType();
	void Serialize(CAppDomainInfo* unit, CBaseStream* stream);
};