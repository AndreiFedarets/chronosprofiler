#pragma once
#include "UnitManagerBase.h"
#include "StackTracer.h"

class CExceptionManager : public CUnitManagerBase<CExceptionInfo>
{
public:
	CExceptionManager(ICorProfilerInfo2* corProfilerInfo2, CStackTracer* stackTracer);
	void Initialize(CExceptionInfo* unit);
	__uint GetUnitType();
	void Serialize(CExceptionInfo* unit, CBaseStream* stream);
private:
	HRESULT FindExceptionMessageField(UINT_PTR exceptionObject);
	CStackTracer* _stackTracer;
	mdFieldDef _exceptionMessageField;
};