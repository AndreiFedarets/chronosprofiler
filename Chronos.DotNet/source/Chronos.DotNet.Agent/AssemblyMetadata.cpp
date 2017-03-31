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
				AssemblyMetadata::AssemblyMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, AssemblyID assemblyId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_assemblyId = assemblyId;
					_appDomainId = 0;
					_manifestModuleId = 0;
					_name = null;
				}

				AssemblyMetadata::~AssemblyMetadata()
				{
					__FREEOBJ(_name);
				}

				AssemblyID AssemblyMetadata::GetId()
				{
					return _assemblyId;
				}

				AppDomainID AssemblyMetadata::GetAppDomainId()
				{
					Initialize();
					return _appDomainId;
				}

				ModuleID AssemblyMetadata::GetManifestModuleId()
				{
					Initialize();
					return _manifestModuleId;
				}

				__string* AssemblyMetadata::GetName()
				{
					Initialize();
					return _name;
				}

				void AssemblyMetadata::Initialize()
				{
					if (_name == null)
					{
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						HRESULT result = _corProfilerInfo->GetAssemblyInfo(_assemblyId, NameMaxLength, 0, nativeName, &_appDomainId, &_manifestModuleId);
						_name = new __string(nativeName);
					}
				}
			}
		}
	}
}