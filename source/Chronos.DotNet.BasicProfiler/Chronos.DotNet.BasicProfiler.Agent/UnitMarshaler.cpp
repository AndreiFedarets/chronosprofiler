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
				void UnitMarshaler::MarshalAppDomain(AppDomainInfo* appDomainInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(appDomainInfo, stream);
				}
		
				void UnitMarshaler::MarshalAssembly(AssemblyInfo* assemblyInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(assemblyInfo, stream);
					//AppDomainId
					Marshaler::MarshalULong(assemblyInfo->GetAppDomainId(), stream);
				}

				void UnitMarshaler::MarshalModule(ModuleInfo* moduleInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(moduleInfo, stream);
					//AssemblyId
					Marshaler::MarshalULong(moduleInfo->GetAssemblyId(), stream);
				}

				void UnitMarshaler::MarshalClass(ClassInfo* classInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(classInfo, stream);
					//TypeToken
					Marshaler::MarshalUInt(classInfo->GetTypeToken(), stream);
					//ModuleId
					Marshaler::MarshalULong(classInfo->GetModuleId(), stream);
				}

				void UnitMarshaler::MarshalFunction(FunctionInfo* functionInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(functionInfo, stream);
					//TypeToken
					Marshaler::MarshalUInt(functionInfo->GetTypeToken(), stream);
					//ClassId
					Marshaler::MarshalULong(functionInfo->GetClassId(), stream);
					//ModuleId
					Marshaler::MarshalULong(functionInfo->GetModuleId(), stream);
					//AssemblyId
					Marshaler::MarshalULong(functionInfo->GetAssemblyId(), stream);
				}

				void UnitMarshaler::MarshalThread(ThreadInfo* threadInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(threadInfo, stream);
					//OsThreadId
					Marshaler::MarshalUInt(threadInfo->GetOsThreadId(), stream);
				}
		
			}
		}
	}
}
