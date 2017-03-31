#pragma once
#include "StdAfx.h"
#include "ModuleManager.h"

CModuleManager::CModuleManager(ICorProfilerInfo2* corProfilerInfo2)
		: CUnitManagerBase<CModuleInfo>(corProfilerInfo2)
{
}

void CModuleManager::Initialize(CModuleInfo* unit)
{
	__try
	{
		UINT_PTR id = static_cast<UINT_PTR>(unit->ManagedId);
		UINT_PTR assemblyId = 0;
		ULONG nameLength = 0;
		_corProfilerInfo2->GetModuleInfo(id, 0, 0, &nameLength, 0, &assemblyId);
		__wchar* nameBuffer = new __wchar[nameLength];
		_corProfilerInfo2->GetModuleInfo(id, 0, nameLength, 0, nameBuffer, 0);
		unit->Name.assign(nameBuffer);
		unit->AssemblyManagedId = assemblyId;
        __FREEARR(nameBuffer);
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"module"))
	{
	}
}
	
__uint CModuleManager::GetUnitType()
{
	return CUnitType::Module;
}
	
void CModuleManager::Serialize(CModuleInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//LoadedResult
	stream->Write(&(unit->LoadResult), TypeSize::_INT32);
	//AssemblyId
	stream->Write(&(unit->AssemblyManagedId), TypeSize::_INT64);
}