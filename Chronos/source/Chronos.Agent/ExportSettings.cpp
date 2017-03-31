#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		ExportSettings::ExportSettings()
		{
		}

		ExportSettings::~ExportSettings()
		{
		}
		
		__bool ExportSettings::GetAgentDll(__string* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(ExportSettings::AgentDllIndex);
			if (i == _settings->end())
			{
				return false;
			}
			value->assign(i->second->AsString());
			return true;
		}

		const __guid ExportSettings::AgentDllIndex = Converter::ConvertStringToGuid(L"{5EEB5AEB-20D7-436D-AB60-9DB07834BB72}");
	}
}