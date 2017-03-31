#include "ManagedProvider.h"
#include "stdafx.h"

void CManagedProvider::Initialize(ICorProfilerInfo2* corProfilerInfo)
{
    _corProfilerInfo = corProfilerInfo;
}

HRESULT CManagedProvider::GetFieldAddress(UINT_PTR objectPointer, wchar_t* fieldName, UINT_PTR* fieldValueAddress)
{
	UINT_PTR fieldOffsetAddress = 0;
    HRESULT result = GetFieldOffset(objectPointer, fieldName, &fieldOffsetAddress);
    if (FAILED(result))
    {
        return result;
    }
	*fieldValueAddress = *((UINT_PTR*)fieldOffsetAddress);
    return S_OK;
}

HRESULT CManagedProvider::GetFirstArgument(COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo, UINT_PTR* pointer)
{
	COR_PRF_FUNCTION_ARGUMENT_RANGE range = argumentInfo->ranges[0];
	UINT_PTR argumentAddress = range.startAddress;
	*pointer = *((UINT_PTR*)argumentAddress);
	return S_OK;
}

HRESULT CManagedProvider::FindFieldAddress(UINT_PTR objectPointer, wchar_t* fieldName, UINT_PTR* fieldValueAddress)
{
    /*IMetaDataImport2* metaDataImport = NULL;
    HRESULT result = GetMetadataForInstance(objectPointer,metaDataImport);
    if (FAILED(result))
    {
        return result;
    }
    do
    {
        result = GetFieldOffset(objectPointer, fieldName, fieldValueAddress);
        if (SUCCEEDED(result))
        {
            return S_OK;
        }
    }*/
    return E_FAIL;
}

HRESULT CManagedProvider::GetFieldOffset(UINT_PTR objectPointer, wchar_t* fieldName, UINT_PTR* fieldOffsetAddress)
{
    //Getting _request field from context
	ClassID classId = 0;
	HRESULT result = _corProfilerInfo->GetClassFromObject(objectPointer, &classId);
	if (FAILED(result))
	{
		return result;
	}
	ModuleID moduleId = 0;
	mdTypeDef classToken = 0;
	result = _corProfilerInfo->GetClassIDInfo(classId, &moduleId, &classToken);
	if (FAILED(result))
	{
		return result;
	}
	//get module metadata by module id
	IMetaDataImport2* metaDataImport = NULL;
	result = _corProfilerInfo->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &metaDataImport);
	if (FAILED(result))
	{
		return result;
	}
	//find field in class by name and class token
	COR_SIGNATURE signature;
	mdFieldDef fieldToken = 0;
	result = metaDataImport->FindField(classToken, fieldName, NULL, 0, &fieldToken);
	if (FAILED(result))
	{
		metaDataImport->Release();
		return result;
	}
	ULONG fieldsCount = 0;
	const ULONG fieldsMaxCount = 128;
	COR_FIELD_OFFSET* fieldsOffset = new COR_FIELD_OFFSET[fieldsMaxCount];
	DWORD packSize = 0;
	ULONG classSize = 0;
	result = _corProfilerInfo->GetClassLayout(classId, fieldsOffset, fieldsMaxCount, &fieldsCount, &classSize); 
	if (FAILED(result))
	{
		metaDataImport->Release();
		__FREEARR(fieldsOffset)
		return result;
	}
	result = E_FAIL;
	ULONG fieldOffset = 0;
	for (int i = 0; i < fieldsCount && i < fieldsMaxCount; i++)
	{
		COR_FIELD_OFFSET corField = fieldsOffset[i];
		if (corField.ridOfField == fieldToken)
		{
			fieldOffset = corField.ulOffset;
			result = S_OK;
			break;
		}
	}
	if (FAILED(result))
	{
		metaDataImport->Release();
		__FREEARR(fieldsOffset)
		return result;
	}
	metaDataImport->Release();
	__FREEARR(fieldsOffset);
    *fieldOffsetAddress = (objectPointer + fieldOffset);
	return S_OK;
}

HRESULT CManagedProvider::GetStringValue(UINT_PTR stringAddress, std::wstring* value)
{
	if (stringAddress == 0)
	{
		return E_FAIL;
	}
	ULONG bufferLengthOffset, stringLengthOffset, bufferOffset;
	HRESULT result = _corProfilerInfo->GetStringLayout(&bufferLengthOffset, &stringLengthOffset, &bufferOffset);
	if (FAILED(result))
	{
		return result;
	}

	UINT_PTR stringBufferAddress = stringAddress + bufferOffset;
	UINT_PTR stringLengthAddress = stringAddress + stringLengthOffset;
	long stringLength = (long)(*(long*)stringLengthAddress);
	wchar_t* stringData = new wchar_t[stringLength + 1];
	memset(stringData, 0, (stringLength + 1) * sizeof(wchar_t));
	memcpy(stringData, (UINT_PTR*)stringBufferAddress, stringLength * sizeof(wchar_t));
	value->append(stringData);
	__FREEARR(stringData);
	return S_OK;
}

HRESULT CManagedProvider::GetMetadataForInstance(UINT_PTR objectPointer, IMetaDataImport2* metadata)
{
	ClassID classId = 0;
	HRESULT result = _corProfilerInfo->GetClassFromObject(objectPointer, &classId);
	if (FAILED(result))
	{
		return result;
	}
	ModuleID moduleId = 0;
	mdTypeDef classToken = 0;
	result = _corProfilerInfo->GetClassIDInfo(classId, &moduleId, &classToken);
	if (FAILED(result))
	{
		return result;
	}
    return GetMetadataForModule(moduleId, metadata);
}

HRESULT CManagedProvider::GetMetadataForModule(UINT_PTR moduleId, IMetaDataImport2* metadata)
{
    std::map<ModuleID, IMetaDataImport2*>::iterator i = _metadata.find(moduleId);
    if (i != _metadata.end())
    {
        metadata = i->second;
        return S_OK;
    }
    HRESULT result = _corProfilerInfo->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &metadata);
    if (FAILED(result))
    {
        return result;
    }
    _metadata.insert(std::pair<ModuleID, IMetaDataImport2*>(moduleId, metadata));
    return S_OK;
}


ICorProfilerInfo2* CManagedProvider::_corProfilerInfo = null;
std::map<ModuleID, IMetaDataImport2*> CManagedProvider::_metadata;