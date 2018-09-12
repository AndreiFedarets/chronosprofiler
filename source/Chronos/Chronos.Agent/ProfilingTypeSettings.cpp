#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		__bool ProfilingTypeSettings::GetDataMarker(__byte* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(ProfilingTypeSettings::DataMarkerIndex);
			if (i == _settings->end())
			{
				return false;
			}
			*value = i->second->AsByte();
			return true;
		}

		__bool ProfilingTypeSettings::GetFrameworkUid(__guid* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(ProfilingTypeSettings::FrameworkUidIndex);
			if (i == _settings->end())
			{
				return false;
			}
			*value = i->second->AsGuid();
			return true;
		}
		
		__bool ProfilingTypeSettings::GetDependencies(__vector<__guid>* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(ProfilingTypeSettings::DependenciesIndex);
			if (i != _settings->end())
			{
				IStreamReader* stream = i->second->OpenRead();
				__int count = Marshaler::DemarshalUInt(stream);
				for (__int i = 0; i < count; ++i)
				{
					__guid uid = Marshaler::DemarshalGuid(stream);
					value->push_back(uid);
				}
				__FREEOBJ(stream);
			}
			return true;
		}

		const __guid ProfilingTypeSettings::DataMarkerIndex = Converter::ConvertStringToGuid(L"{60B683C6-1686-4CCB-B965-856D72E12662}");
		const __guid ProfilingTypeSettings::FrameworkUidIndex = Converter::ConvertStringToGuid(L"{FF216060-30AA-4E54-8DF0-BAB4BEB2CF6F}");
		const __guid ProfilingTypeSettings::DependenciesIndex = Converter::ConvertStringToGuid(L"{DEABC098-FD0C-486F-BDEC-2FB369C29058}");
	}
}