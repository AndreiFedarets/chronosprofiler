#include "stdafx.h"
#include "Chronos.DotNet.ExceptionMonitor.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace ExceptionMonitor
			{
				ManagedExceptionCollection::ManagedExceptionCollection()
				{
				}

				HRESULT ManagedExceptionCollection::InitializeUnitSpecial(ManagedExceptionInfo* unit)
				{
					ICorProfilerInfo2* corProfilerInfo;
					__RETURN_IF_FAILED( _metadataProvider->GetCorProfilerInfo2(&corProfilerInfo) );

					ObjectID objectId = (ObjectID)unit->Id;
					ClassID classId = 0;
					__RETURN_IF_FAILED( corProfilerInfo->GetClassFromObject(objectId, &classId) );

					mdFieldDef exceptionMessageField = 0;
					ClassID exceptionClassId = 0;
					IMetaDataImport2* metaDataImport = null;

					__RETURN_IF_FAILED( GetExceptionMessageField(objectId, &exceptionClassId, &exceptionMessageField, &metaDataImport) );

					COR_FIELD_OFFSET fieldOffsets[255];
					ULONG count = 0;
					ULONG pulClassSize = 0;

					__RETURN_IF_FAILED( corProfilerInfo->GetClassLayout(exceptionClassId, fieldOffsets, 255, &count, &pulClassSize) );

					COR_FIELD_OFFSET exceptionMessageOffset;
					
					for (__uint i = 0; i < count; i++)
					{
						COR_FIELD_OFFSET offset = fieldOffsets[i];
						if (offset.ridOfField == exceptionMessageField)
						{
							exceptionMessageOffset = offset;
							break;
						}
					}

					objectId += exceptionMessageOffset.ulOffset;
					
					UINT_PTR messageAddress = *((UINT_PTR*)objectId);

					ULONG bufferLengthOffset = 0;
					ULONG stringLengthOffset = 0;
					ULONG bufferOffset = 0;

					__RETURN_IF_FAILED( corProfilerInfo->GetStringLayout(&bufferLengthOffset, &stringLengthOffset, &bufferOffset) );

					UINT_PTR messageSizeAddress = messageAddress + stringLengthOffset;

					__uint stringLength = *(__uint*)messageSizeAddress;
					__wchar* stringValue = (__wchar*)(messageAddress + bufferOffset);

					unit->InitializeSpecial(new __string(stringValue), classId);
					
					return S_OK;
				}

				HRESULT ManagedExceptionCollection::GetExceptionMessageField(ObjectID exceptionObjectId, ClassID* exceptionClassId, mdFieldDef* fieldToken, IMetaDataImport2** metaDataImport)
				{
					ICorProfilerInfo2* corProfilerInfo = null;
					__RETURN_IF_FAILED( _metadataProvider->GetCorProfilerInfo2(&corProfilerInfo) );

					//get class id by object instance id
					__RETURN_IF_FAILED( corProfilerInfo->GetClassFromObject(exceptionObjectId, exceptionClassId) );

					HRESULT result;
					do 
					{
						//get module id and class token by class id
						ModuleID moduleId = 0;
						mdTypeDef classToken;

						__RETURN_IF_FAILED(corProfilerInfo->GetClassIDInfo(*exceptionClassId, &moduleId, &classToken));

						//get module metadata by module id
						__RETURN_IF_FAILED( corProfilerInfo->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**)metaDataImport) );

						//find field in class by name and class token
						result = (*metaDataImport)->FindField(classToken, L"_message", null, 0, fieldToken);

						if (FAILED(result))
						{
							ClassID parentClassId = 0;
							__RETURN_IF_FAILED( corProfilerInfo->GetClassIDInfo2(*exceptionClassId, &moduleId, &classToken, &parentClassId, 0, null, null) );
							*exceptionClassId = parentClassId;
						}
					}
					while (!SUCCEEDED(result));
					if (*fieldToken == 0 || *metaDataImport == null || *exceptionClassId == 0)
					{
						return E_FAIL;
					}
					return S_OK;
				}
	
				const __guid ManagedExceptionCollection::ServiceToken = Converter::ConvertStringToGuid(L"{07616B9B-0F12-4035-B710-8BA4F6BC1C79}");
			}
		}
	}
}