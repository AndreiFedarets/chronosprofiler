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
				ModuleMetadata::ModuleMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ModuleID moduleId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_metaDataImport = null;
					_moduleId = moduleId;
					_assemblyId = 0;
					_baseLoadAddress = 0;
					_name = null;
				}

				ModuleMetadata::~ModuleMetadata()
				{
					__FREEOBJ(_name);
					if (_metaDataImport != null)
					{
						_metaDataImport->Release();
					}
				}

				ModuleID ModuleMetadata::GetId()
				{
					return _moduleId;
				}

				AssemblyID ModuleMetadata::GetAssemblyId()
				{
					Initialize();
					return _assemblyId;
				}

				LPCBYTE ModuleMetadata::GetBaseLoadAddress()
				{
					Initialize();
					return _baseLoadAddress;
				}

				__string* ModuleMetadata::GetName()
				{
					Initialize();
					return _name;
				}

				IMetaDataImport2* ModuleMetadata::GetMetadataImport()
				{
					if (_metaDataImport == null)
					{
						HRESULT result = _corProfilerInfo->GetModuleMetaData(_moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &_metaDataImport);
					}
					return _metaDataImport;
				}
				
				void ModuleMetadata::Initialize()
				{
					if (_name == null)
					{
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						HRESULT result = _corProfilerInfo->GetModuleInfo(_moduleId, &_baseLoadAddress, NameMaxLength, 0, nativeName, &_assemblyId);
						_name = new __string(nativeName);
					}
				}
			}
		}
	}
}