#pragma once
#include "Chronos.DotNet.SqlProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace SqlProfiler
			{
// ==================================================================================================================================================
				class QueryContext
				{
					public:
						__declspec(thread) static MsSqlQueryInfo* CurrentMsSqlQuery;
				};
			}
		}
	}
}