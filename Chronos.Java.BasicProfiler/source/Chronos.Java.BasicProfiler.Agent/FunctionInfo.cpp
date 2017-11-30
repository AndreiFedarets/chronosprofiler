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
				FunctionInfo::FunctionInfo()
					: _assemblyId(0), _moduleId(0), _classId(0), _typeToken(0)
				{
				}
				
				void FunctionInfo::PrepareClose()
				{
					_assemblyId = GetMetadata()->GetAssemblyId();
					_moduleId = GetMetadata()->GetModuleId();
					_classId = GetMetadata()->GetTypeId();
					_typeToken = GetMetadata()->GetTypeToken();
				}
				
				mdTypeDef FunctionInfo::GetTypeToken()
				{
					if (GetIsAlive() && _typeToken == 0)
					{
						_typeToken = GetMetadata()->GetTypeToken();
					}
					return _typeToken;
				}

				AssemblyID FunctionInfo::GetAssemblyId()
				{
					if (GetIsAlive() && _assemblyId == 0)
					{
						_assemblyId = GetMetadata()->GetAssemblyId();
					}
					return _assemblyId;
				}

				ModuleID FunctionInfo::GetModuleId()
				{
					if (GetIsAlive() && _moduleId == 0)
					{
						_moduleId = GetMetadata()->GetModuleId();
					}
					return _moduleId;
				}

				ClassID FunctionInfo::GetClassId()
				{
					if (GetIsAlive() && _classId == 0)
					{
						_classId = GetMetadata()->GetTypeId();
					}
					return _classId;
				}

				Reflection::MethodMetadata* FunctionInfo::GetMetadataInternal()
				{
					Reflection::MethodMetadata* metadata;
					_metadataProvider->GetMethod(Id, &metadata);
					return metadata;
				}

				void FunctionInfo::OnLoaded()
				{
					PrepareClose();
				}
			}
		}
	}
}