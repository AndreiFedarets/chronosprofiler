#pragma once
#include "StdAfx.h"
#include "UnitManagerBase.h"

class CClassManager : public CUnitManagerBase<CClassInfo>
{
public:
	CClassManager(ICorProfilerInfo2* corProfilerInfo2);
	void Initialize(CClassInfo* unit);
	__uint GetUnitType();
	void Serialize(CClassInfo* unit, CBaseStream* stream);
};
