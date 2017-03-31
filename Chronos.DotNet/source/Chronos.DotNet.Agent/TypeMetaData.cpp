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
						IMetaDataImport2* metaDataImport =  moduleMetadata->GetMetadataImport();
						
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						result = metaDataImport->GetTypeDefProps(_typeToken, nativeName, NameMaxLength, 0, 0, 0);

						_name = new __string(nativeName);
					}
				}
			}
		}
	}
}