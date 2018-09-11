#include "stdafx.h"
#include "Chronos.DotNet.ExceptionMonitor.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace ExceptionMonitor
			{
				void UnitMarshaler::MarshalManagedException(ManagedExceptionInfo* managedExceptionInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(managedExceptionInfo, stream);
					//Exception ClassID
					Marshaler::MarshalULong(managedExceptionInfo->GetExceptionClassId(), stream);
					//Message
					Marshaler::MarshalString(managedExceptionInfo->GetMessage(), stream);
					//Stack
					__vector<FunctionID> stack = managedExceptionInfo->GetStack();
					__uint stackSize = (__uint)stack.size();
					Marshaler::MarshalUInt(stackSize, stream);
					for (__vector<FunctionID>::iterator i = stack.begin(); i != stack.end(); ++i)
					{
						FunctionID functionUid = *i;
						Marshaler::MarshalULong(functionUid, stream);
					}
				}
			}
		}
	}
}