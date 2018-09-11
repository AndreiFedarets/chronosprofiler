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
				ClassInfo::ClassInfo()
					: _moduleId(0), _typeToken(0)
				{
				}
				
				void ClassInfo::PrepareClose()
				{
					_moduleId = GetMetadata()->GetModuleId();
					_typeToken = GetMetadata()->GetTypeToken();
				}

				ModuleID ClassInfo::GetModuleId()
				{
					if (GetIsAlive() && _moduleId == 0)
					{
						_moduleId = GetMetadata()->GetModuleId();
					}
					return _moduleId;
				}

				mdTypeDef ClassInfo::GetTypeToken()
				{
					if (GetIsAlive() && _typeToken == 0)
					{
						_typeToken = GetMetadata()->GetTypeToken();
					}
					return _typeToken;
				}

				Reflection::TypeMetadata* ClassInfo::GetMetadataInternal()
				{
					Reflection::TypeMetadata* metadata;
					_metadataProvider->GetType(Id, &metadata);
					return metadata;
				}

				void ClassInfo::OnLoaded()
				{
					PrepareClose();
				}
			}
		}
	}
}