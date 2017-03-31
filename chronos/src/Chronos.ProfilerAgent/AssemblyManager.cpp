#pragma once
#include "StdAfx.h"
#include "AssemblyManager.h"

CAssemblyManager::CAssemblyManager(ICorProfilerInfo2* corProfilerInfo2)
		: CUnitManagerBase<CAssemblyInfo>(corProfilerInfo2)
{

}

void CAssemblyManager::Initialize(CAssemblyInfo* unit)
{
	__try
	{
		UINT_PTR id = static_cast<UINT_PTR>(unit->ManagedId);
		UINT_PTR appDomainId = 0;
		ULONG nameLength = 0;
		_corProfilerInfo2->GetAssemblyInfo(id, 0, &nameLength, 0, &appDomainId, 0);
		__wchar* nameBuffer = new __wchar[nameLength];
		_corProfilerInfo2->GetAssemblyInfo(id, nameLength, 0, nameBuffer, 0, 0);
		unit->Name.assign(nameBuffer);
		unit->AppDomainManagedId = appDomainId;
        __FREEARR(nameBuffer);
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"assembly"))
	{
	}
}
	
__uint CAssemblyManager::GetUnitType()
{
	return CUnitType::Assembly;
}
	
void CAssemblyManager::Serialize(CAssemblyInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//LoadedResult
	stream->Write(&(unit->LoadResult), TypeSize::_INT32);
	//AppDomainId
	stream->Write(&(unit->AppDomainManagedId), TypeSize::_INT64);
}
