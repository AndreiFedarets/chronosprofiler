#include "stdafx.h"
#include "Chronos.Java.BasicProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Java
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