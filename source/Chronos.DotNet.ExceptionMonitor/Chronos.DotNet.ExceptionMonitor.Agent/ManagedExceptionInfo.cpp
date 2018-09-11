#include "stdafx.h"
#include "Chronos.DotNet.ExceptionMonitor.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace ExceptionMonitor
			{
				ManagedExceptionInfo::ManagedExceptionInfo()
				{
				}
				
				void ManagedExceptionInfo::InitializeSpecial(__string* message, ClassID exceptionClassId)
				{
					_name = message;
					_exceptionClassId = exceptionClassId;
				}

				__string* ManagedExceptionInfo::GetMessage()
				{
					return _name;
				}

				ClassID ManagedExceptionInfo::GetExceptionClassId()
				{
					return _exceptionClassId;
				}

				__vector<FunctionID> ManagedExceptionInfo::GetStack()
				{
					return _stack;
				}
			}
		}
	}
}