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
				ModuleInfo::ModuleInfo()
					: _assemblyId(0)
				{
				}
				
				void ModuleInfo::PrepareClose()
				{
					_assemblyId = GetMetadata()->GetAssemblyId();
				}

				AssemblyID ModuleInfo::GetAssemblyId()
				{
					if (GetIsAlive() && _assemblyId == 0)
					{
						_assemblyId = GetMetadata()->GetAssemblyId();
					}
					return _assemblyId;
				}

				Reflection::ModuleMetadata* ModuleInfo::GetMetadataInternal()
				{
					Reflection::ModuleMetadata* metadata;
					_metadataProvider->GetModule(Id, &metadata);
					return metadata;
				}

				void ModuleInfo::OnLoaded()
				{
					PrepareClose();
				}
			}
		}
	}
}