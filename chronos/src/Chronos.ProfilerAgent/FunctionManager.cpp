#pragma once
#include "StdAfx.h"
#include "FunctionManager.h"

CFunctionManager::CFunctionManager(ICorProfilerInfo2* corProfilerInfo2)
		: CUnitManagerBase<CFunctionInfo>(corProfilerInfo2)
{

}

void CFunctionManager::Initialize(CFunctionInfo* unit)
{
	__try
	{
		UINT_PTR id = static_cast<UINT_PTR>(unit->ManagedId);
		UINT_PTR moduleId;
		UINT_PTR classId;
		_corProfilerInfo2->GetFunctionInfo(id, &classId, &moduleId, 0);
		if (classId != 0)
		{
			UINT_PTR assemblyId;
			_corProfilerInfo2->GetModuleInfo(moduleId, 0, 0, 0, 0, &assemblyId);
			unit->AssemblyManagedId = assemblyId;
		}
		unit->ClassManagedId = classId;

		IMetaDataImport* metaDataImport = 0;
		HRESULT result = S_OK;
		mdMethodDef methodDef = 0;
			
		result = _corProfilerInfo2->GetTokenAndMetaDataFromFunction(id, IID_IMetaDataImport, (LPUNKNOWN *) &metaDataImport, &methodDef);
		if(SUCCEEDED(result))
		{
			ULONG nameLength = 0;
			result = metaDataImport->GetMethodProps(methodDef, 0, 0, 0, &nameLength, 0, 0, 0, 0, 0);

			__wchar* nameBuffer = new __wchar[nameLength];

			result = metaDataImport->GetMethodProps(methodDef, 0, nameBuffer, nameLength, 0, 0, 0, 0, 0, 0);
			unit->Name.assign(nameBuffer);

			metaDataImport->Release();
            __FREEARR(nameBuffer);
		}
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"function"))
	{
	}
}
	
__uint CFunctionManager::GetUnitType()
{
	return CUnitType::Function;
}
	
void CFunctionManager::Serialize(CFunctionInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//ClassId
	stream->Write(&(unit->ClassManagedId), TypeSize::_INT64);
	//Hits
	stream->Write(&(unit->Hits), TypeSize::_INT32);
	//TotalTime
	stream->Write(&(unit->TotalTime), TypeSize::_INT32);
}