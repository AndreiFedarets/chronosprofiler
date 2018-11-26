#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace Reflection
			{
				TypeMetadata::TypeMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ClassID classId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_classId = classId;
					_moduleId = 0;
					_typeToken = 0;
					_name = null;
					_fields = null;
				}

				TypeMetadata::~TypeMetadata()
				{
					__FREEOBJ(_name);
				}

				ClassID TypeMetadata::GetId()
				{
					return _classId;
				}
				
				mdTypeDef TypeMetadata::GetTypeToken()
				{
					Initialize();
					return _typeToken;
				}
				
				ModuleID TypeMetadata::GetModuleId()
				{
					Initialize();
					return _moduleId;
				}
				
				__string* TypeMetadata::GetName()
				{
					Initialize();
					return _name;
				}
				
				void TypeMetadata::Initialize()
				{
					if (_name == null)
					{
						HRESULT result = _corProfilerInfo->GetClassIDInfo(_classId, &_moduleId, &_typeToken);

						ModuleMetadata* moduleMetadata = null;
						result = _provider->GetModule(_moduleId, &moduleMetadata);
						IMetaDataImport2* metadataImport = moduleMetadata->GetMetadataImport();
						
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						result = metadataImport->GetTypeDefProps(_typeToken, nativeName, NameMaxLength, 0, 0, 0);

						_name = new __string(nativeName);
					}
				}

				__vector<FieldMetadata*>* TypeMetadata::GetFields()
				{
					if (_fields == null)
					{
						Initialize();
						_fields = new __vector<FieldMetadata*>();
						ModuleMetadata* moduleMetadata = null;
						__RETURN_NULL_IF_FAILED(_provider->GetModule(_moduleId, &moduleMetadata));
						IMetaDataImport2* metadataImport = moduleMetadata->GetMetadataImport();

						HCORENUM fieldEnumerator = null;
						const ULONG fieldTokensMax = 256;
						mdFieldDef fieldTokens[fieldTokensMax] {0};
						ULONG fieldTokensCount = 0;
						__RETURN_NULL_IF_FAILED(metadataImport->EnumFields(&fieldEnumerator, GetTypeToken(), fieldTokens, fieldTokensMax, &fieldTokensCount));

						for (__uint i = 0; i < fieldTokensCount; i++)
						{
							mdFieldDef fieldToken = fieldTokens[i];
							FieldMetadata* fieldMetadata = new FieldMetadata(metadataImport, fieldToken);
							_fields->push_back(fieldMetadata);
						}

					}
					return _fields;
				}

				FieldMetadata* TypeMetadata::FindField(__string* fieldName)
				{
					__vector<FieldMetadata*>* fields = GetFields();
					for (__vector<Reflection::FieldMetadata*>::iterator i = fields->begin(); i != fields->end(); ++i)
					{
						FieldMetadata* field = *i;
						if (StringComparer::Equals(fieldName, field->GetName(), false))
						{
							return field;
						}
					}
					return null;
				}
			}
		}
	}
}