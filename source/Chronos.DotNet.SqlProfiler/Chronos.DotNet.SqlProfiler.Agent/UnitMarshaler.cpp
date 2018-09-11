#include "stdafx.h"
#include "Chronos.DotNet.SqlProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace SqlProfiler
			{
				void UnitMarshaler::MarshalMsSqlQuery(MsSqlQueryInfo* sqlQueryInfo, IStreamWriter* stream)
				{
					//Base
					Chronos::Agent::UnitMarshaler::MarshalUnit(sqlQueryInfo, stream);
				}
			}
		}
	}
}