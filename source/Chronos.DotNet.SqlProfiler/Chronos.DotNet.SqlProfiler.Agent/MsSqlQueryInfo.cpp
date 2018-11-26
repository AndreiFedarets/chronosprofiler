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
				MsSqlQueryInfo::MsSqlQueryInfo()
				{

				}

				void MsSqlQueryInfo::InitializeName(__string* queryText)
				{
					_name = queryText;
				}
			}
		}
	}
}