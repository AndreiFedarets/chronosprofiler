#pragma once
#include "StdAfx.h"
#include "AppDomainManager.h"

CAppDomainManager::CAppDomainManager(ICorProfilerInfo2* corProfilerInfo2)
	 : CUnitManagerBase<CAppDomainInfo>(corProfilerInfo2)
{
}

void CAppDomainManager::Initialize(CAppDomainInfo* unit)
{
	__try
	{
		UINT_PTR id = static_cast<UINT_PTR>(unit->ManagedId);
		ULONG nameLength = 0;
		_corProfilerInfo2->GetAppDomainInfo(id, 0, &nameLength, 0, 0);
		__wchar* nameBuffer = new __wchar[nameLength];
		_corProfilerInfo2->GetAppDomainInfo(id, nameLength, 0, nameBuffer, 0);
		unit->Name.assign(nameBuffer);
        __FREEARR(nameBuffer);
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"appdomain"))
	{
	}
}

__uint CAppDomainManager::GetUnitType()
{
	return CUnitType::AppDomain;
}

void CAppDomainManager::Serialize(CAppDomainInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//LoadedResult
	stream->Write(&(unit->LoadResult), TypeSize::_INT32);
}