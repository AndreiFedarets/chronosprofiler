#pragma once
#include "StdAfx.h"
#include "ExceptionManager.h"

CExceptionManager::CExceptionManager(ICorProfilerInfo2* corProfilerInfo2, CStackTracer* stackTracer)
		: CUnitManagerBase<CExceptionInfo>(corProfilerInfo2), _stackTracer(stackTracer), _exceptionMessageField(0)
{

}

void CExceptionManager::Initialize(CExceptionInfo* unit)
{
	__try
	{
		_stackTracer->DumpStack(unit->Stack);
		UINT_PTR id = static_cast<UINT_PTR>(unit->ManagedId);
		UINT_PTR threadId;
		_corProfilerInfo2->GetCurrentThreadID(&threadId);
		UINT_PTR ñlassID;
		HRESULT result = _corProfilerInfo2->GetClassFromObject(id, &ñlassID);
		if (SUCCEEDED(result))
		{
			UINT_PTR moduleId;
			mdTypeDef classMetaToken;
			result = _corProfilerInfo2->GetClassIDInfo(ñlassID, &moduleId, &classMetaToken);

			if (SUCCEEDED(result))
			{
				IMetaDataImport* metaDataImport = null ;

				result = _corProfilerInfo2->GetModuleMetaData (moduleId, ofRead, IID_IMetaDataImport, (IUnknown**)&metaDataImport );
				if (SUCCEEDED(result))
				{
					ULONG nameLength = 0;
					result = metaDataImport->GetTypeDefProps(classMetaToken, 0, 0, &nameLength, null, null);
					__wchar* nameBuffer = new __wchar[nameLength];
					result = metaDataImport->GetTypeDefProps(classMetaToken, nameBuffer, nameLength, 0, null, null);
					unit->Name.assign(nameBuffer);
					metaDataImport->Release();
					__FREEARR(nameBuffer);
				}
			}
		}
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"exception"))
	{
	}
}

HRESULT CExceptionManager::FindExceptionMessageField(UINT_PTR exceptionObject)
{
	//get class id by exception object
	ClassID classId = 0;
	HRESULT result = _corProfilerInfo2->GetClassFromObject(exceptionObject, &classId);
	if (FAILED(result))
	{
		return result;
	}
	//get module id and class token by class id
	ModuleID moduleId = 0;
	mdTypeDef classToken;
	result = _corProfilerInfo2->GetClassIDInfo(classId, &moduleId, &classToken);
	if (FAILED(result))
	{
		return result;
	}
	//get module metadata by module id
	IMetaDataImport2* metaDataImport;
	result = _corProfilerInfo2->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &metaDataImport);
	if (FAILED(result))
	{
		return result;
	}
	//find field in class by name and class token
	PCCOR_SIGNATURE signature;
	mdFieldDef fieldToken;
	result = metaDataImport->FindField(classToken, L"_message", signature, 0, &fieldToken);
	if (FAILED(result))
	{
		return result;
	}

	ULONG messageSize = 0;
	result = metaDataImport->GetUserString(fieldToken, 0, 0, &messageSize);
	if (FAILED(result))
	{
		return result;
	}

	__wchar* messageRaw = new __wchar[messageSize];
	metaDataImport->GetUserString(fieldToken, messageRaw, messageSize, &messageSize);
	return S_OK;
}
	
__uint CExceptionManager::GetUnitType()
{
	return CUnitType::Exception;
}
	
void CExceptionManager::Serialize(CExceptionInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//ThreadId
	stream->Write(&(unit->ThreadId), TypeSize::_INT64);
	//IsCatched
	stream->Write(&(unit->IsCatched), TypeSize::_BOOL);
	//Message
	CStringMarshaler::Marshal(&(unit->Message), stream);
	//Stack Size
	std::queue<CFunctionInfo*>* stack = unit->Stack;
	__uint stackSize = static_cast<__uint>(stack->size());
	stream->Write(&stackSize, TypeSize::_INT32);
	for (__uint i = 0; i < stackSize; i++)
	{
		CFunctionInfo* functionInfo = stack->front();
		stream->Write(&(functionInfo->Id), TypeSize::_INT32);
		stack->pop();
	}
}
