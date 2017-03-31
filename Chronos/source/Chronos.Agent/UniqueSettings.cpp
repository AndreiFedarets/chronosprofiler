#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		UniqueSettings::UniqueSettings()
		{
		}

		UniqueSettings::~UniqueSettings()
		{
		}
		
		__bool UniqueSettings::GetUid(__guid* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(UniqueSettings::UidIndex);
			if (i == _settings->end())
			{
				return false;
			}
			*value = i->second->AsGuid();
			return true;
		}

		const __guid UniqueSettings::UidIndex = Converter::ConvertStringToGuid(L"{E36CFA00-0712-45B4-8423-17846C318208}");
		
	}
}