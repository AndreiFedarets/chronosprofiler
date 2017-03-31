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
				AppDomainInfo::AppDomainInfo()
				{
				}

				Reflection::AppDomainMetadata* AppDomainInfo::GetMetadataInternal()
				{
					Reflection::AppDomainMetadata* metadata;
					_metadataProvider->GetAppDomain(Id, &metadata);
					return metadata;
				}

				void AppDomainInfo::OnLoaded()
				{
					PrepareClose();
				}
			}
		}
	}
}