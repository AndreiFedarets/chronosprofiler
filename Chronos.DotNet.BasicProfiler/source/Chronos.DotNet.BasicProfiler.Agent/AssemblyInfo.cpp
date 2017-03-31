#include "stdafx.h"
#include "Chronos.DotNet.BasicProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace BasicProfiler
			{
				AssemblyInfo::AssemblyInfo()
					: _appDomainId(0)
				{
				}

				void AssemblyInfo::PrepareClose()
				{
					_appDomainId = GetMetadata()->GetAppDomainId();
				}

				AppDomainID AssemblyInfo::GetAppDomainId()
				{
					if (GetIsAlive() && _appDomainId == 0)
					{
						_appDomainId = GetMetadata()->GetAppDomainId();
					}
					return _appDomainId;
				}

				Reflection::AssemblyMetadata* AssemblyInfo::GetMetadataInternal()
				{
					Reflection::AssemblyMetadata* metadata;
					_metadataProvider->GetAssembly(Id, &metadata);
					return metadata;
				}

				void AssemblyInfo::OnLoaded()
				{
					PrepareClose();
				}
			}
		}
	}
}