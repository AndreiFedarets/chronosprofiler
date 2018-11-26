#include "stdafx.h"
#include "Chronos.DotNet.SqlProfiler.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace SqlProfiler
			{
				MsSqlQueryInfo* QueryContext::CurrentMsSqlQuery = null;
			}
		}
	}
}