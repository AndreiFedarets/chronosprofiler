#pragma once
#include "Macro.h"
#include <cor.h>
#include <corprof.h>

class CManagedProvider
{
public:
    static void Initialize(ICorProfilerInfo2* profilerInfo);
    static HRESULT FindFieldAddress(UINT_PTR objectPointer, wchar_t* fieldName, UINT_PTR* fieldValueAddress);
    static HRESULT GetFieldAddress(UINT_PTR objectPointer, wchar_t* fieldName, UINT_PTR* fieldValueAddress);
    static HRESULT GetFieldOffset(UINT_PTR objectPointer, wchar_t* fieldName, UINT_PTR* fieldValueAddress);
    static HRESULT GetStringValue(UINT_PTR stringAddress, std::wstring* value);
	static HRESULT GetFirstArgument(COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo, UINT_PTR* pointer);
private:
    static HRESULT GetMetadataForInstance(UINT_PTR objectPointer, IMetaDataImport2* metadata);
    static HRESULT GetMetadataForModule(UINT_PTR moduleId, IMetaDataImport2* metadata);
    static ICorProfilerInfo2* _corProfilerInfo;
    static std::map<ModuleID, IMetaDataImport2*> _metadata;
};

