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