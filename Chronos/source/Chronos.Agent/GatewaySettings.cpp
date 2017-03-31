#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		GatewaySettings::GatewaySettings()
		{
		}

		GatewaySettings::~GatewaySettings()
		{
		}
		
		__bool GatewaySettings::GetSyncStreamsCount(__uint* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(GatewaySettings::SyncStreamsCountIndex);
			if (i == _settings->end())
			{
				return false;
			}
			*value = i->second->AsUInt();
			return true;
		}
		
		__bool GatewaySettings::GetAsyncStreamsCount(__uint* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(GatewaySettings::AsyncStreamsCountIndex);
			if (i == _settings->end())
			{
				return false;
			}
			*value = i->second->AsUInt();
			return true;
		}

		const __guid GatewaySettings::SyncStreamsCountIndex = Converter::ConvertStringToGuid(L"{435FED7B-0F19-46E3-B10F-C8D7B676A3C9}");
		const __guid GatewaySettings::AsyncStreamsCountIndex = Converter::ConvertStringToGuid(L"{A7017CB0-965A-4F14-A524-5C0A6E3F53E4}");
		
	}
}