#pragma once
#include "StdAfx.h"
#include "SqlRequestManager.h"
#include "ManagedProvider.h"

CSqlRequestManager::CSqlRequestManager(ICorProfilerInfo2* corProfilerInfo2)
		: CUnitManagerBase<CSqlRequestInfo>(corProfilerInfo2)
{

}
//
//CSqlRequestInfo* CSqlRequestManager::Create(COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
//{
//	CSqlRequestInfo* unit = new CSqlRequestInfo(
//	COR_PRF_FUNCTION_ARGUMENT_RANGE range = argumentInfo->ranges[0];
//    UINT_PTR commandArgumentAddress = range.startAddress;
//	UINT_PTR commandAddress = *((UINT_PTR*)commandArgumentAddress);
//	
//	UINT_PTR commandTextAddress = 0;
//	HRESULT result = CManagedProvider::GetFieldAddress(commandAddress, L"_commandText", &commandTextAddress);
//	if (FAILED(result))
//	{
//		return false;
//	}
//	result = CManagedProvider::GetString(commandTextAddress, commandText);
//	if (FAILED(result))
//	{
//		return false;
//	}
//	return true;
//	if (CManagedProvider::GetSqlQuery(argumentInfo, &commandText))
//    {
//		functionInfo = _functionManager->GetOrCreateSqlFunction(functionInfo, commandText);
//		_pageTransaction->Trace->Call(functionInfo);
//		_sqlTransaction = new CSqlTransaction(functionInfo);
//    }
//	return null;
//}

void CSqlRequestManager::Initialize(CSqlRequestInfo* unit)
{
	__try
	{
		/*UINT_PTR id = static_cast<UINT_PTR>(unit->ManagedId);
		UINT_PTR assemblyId;
		UINT_PTR moduleId;
		UINT_PTR classId;
		_corProfilerInfo2->GetFunctionInfo(id, &classId, &moduleId, 0);
		if (classId == 0)
		{
			return;
		}
		_corProfilerInfo2->GetModuleInfo(moduleId, 0, 0, 0, 0, &assemblyId);
		unit->AssemblyManagedId = assemblyId;
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
		}*/
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"sql_request"))
	{
	}
}
	
__uint CSqlRequestManager::GetUnitType()
{
	return CUnitType::SqlRequest;
}
	
void CSqlRequestManager::Serialize(CSqlRequestInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//Hits
	stream->Write(&(unit->Hits), TypeSize::_INT32);
	//TotalTime
	stream->Write(&(unit->TotalTime), TypeSize::_INT32);
}