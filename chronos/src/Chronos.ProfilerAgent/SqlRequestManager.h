#pragma once
#include "UnitManagerBase.h"
#include <cor.h>

class CSqlRequestManager : public CUnitManagerBase<CSqlRequestInfo>
{
public:
	CSqlRequestManager(ICorProfilerInfo2* corProfilerInfo2);
	void Initialize(CSqlRequestInfo* unit);
	__uint GetUnitType();
	void Serialize(CSqlRequestInfo* unit, CBaseStream* stream);
};
