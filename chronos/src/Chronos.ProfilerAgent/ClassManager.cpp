#pragma once
#include "StdAfx.h"
#include "ClassManager.h"

CClassManager::CClassManager(ICorProfilerInfo2* corProfilerInfo2)
		: CUnitManagerBase<CClassInfo>(corProfilerInfo2)
{

}

void CClassManager::Initialize(CClassInfo* unit)
{
	__try
	{
		UINT_PTR id = static_cast<UINT_PTR>(unit->ManagedId);
		UINT_PTR moduleId = 0;

		mdTypeDef classTypeDef = null;
		HRESULT result = _corProfilerInfo2->GetClassIDInfo(id, &moduleId, &classTypeDef);
		unit->ModuleManagedId = moduleId;
			
		IMetaDataImport2* metaDataImport;
		_corProfilerInfo2->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &metaDataImport);

		ULONG nameLength = 0;
		result = metaDataImport->GetTypeDefProps(classTypeDef, 0, 0, &nameLength, 0, 0);

		__wchar* nameBuffer = new __wchar[nameLength];
		result = metaDataImport->GetTypeDefProps(classTypeDef, nameBuffer, nameLength, 0, 0, 0);

		unit->Name.assign(nameBuffer);
        __FREEARR(nameBuffer);
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"class"))
	{
	}
}
	
__uint CClassManager::GetUnitType()
{
	return CUnitType::Class;
}
	
void CClassManager::Serialize(CClassInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//LoadedResult
	stream->Write(&(unit->LoadResult), TypeSize::_INT32);
	//ModuleId
	stream->Write(&(unit->ModuleManagedId), TypeSize::_INT64);
}