#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		__bool ProfilingTargetSettings::GetProfileChildProcess(__bool* value)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(ProfilingTargetSettings::ProfileChildProcessIndex);
			if (i == _settings->end())
			{
				return false;
			}
			*value = i->second->AsBool();
			return true;
		}
		
		const __guid ProfilingTargetSettings::ProfileChildProcessIndex = Converter::ConvertStringToGuid(L"{ADFF8AA5-C65F-4034-81E5-FE682574ED64}");
	}
}