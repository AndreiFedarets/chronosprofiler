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
				HRESULT MsSqlQueryCollection::InitializeUnitSpecial(MsSqlQueryInfo* unit)
				{
					//HRESULT result = _managedProvider->GetClassInfo(id, (ModuleID*)&(unit->ModuleId), &(unit->Name));
					//return result;
					return S_OK;
				}
	
				const __guid MsSqlQueryCollection::ServiceToken = Converter::ConvertStringToGuid(L"{06B185D8-B4E0-4B89-8BE5-47E07C39F319}");
			}
		}
	}
}