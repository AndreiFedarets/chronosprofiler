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
				MsSqlQueryCollection::MsSqlQueryCollection()
				{
					_lastUid = 0;
				}

				MsSqlQueryInfo* MsSqlQueryCollection::CreateUnit()
				{
					__uint currentUid = InterlockedIncrement(&_lastUid);
					return DotNetUnitCollectionBase<MsSqlQueryInfo>::CreateUnit(currentUid);
				}

				MsSqlQueryInfo* MsSqlQueryCollection::FindQuery(__string* queryText, __bool create)
				{
					return CreateUnit();
				}

				HRESULT MsSqlQueryCollection::InitializeUnitSpecial(MsSqlQueryInfo* unit)
				{
					return S_OK;
				}
	
				const __guid MsSqlQueryCollection::ServiceToken = Converter::ConvertStringToGuid(L"{06B185D8-B4E0-4B89-8BE5-47E07C39F319}");
			}
		}
	}
}