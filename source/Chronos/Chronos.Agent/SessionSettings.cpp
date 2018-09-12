#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		SessionSettings::SessionSettings(void)
		{
			_initialized = false;
			_frameworksSettings = null;
			_profilingTypesSettings = null;
			_gatewaySettings = null;
			_profilingTargetSettings = null;
		}

		SessionSettings::~SessionSettings(void)
		{
			__FREEOBJ(_frameworksSettings);
			__FREEOBJ(_profilingTypesSettings);
			__FREEOBJ(_gatewaySettings);
		}
		
		__bool SessionSettings::GetProfilingTypesSettings(ProfilingTypeSettingsCollection** profilingTypesSettings)
		{
			if (_profilingTypesSettings == null)
			{
				std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(SessionSettings::ProfilingTypesSettingsIndex);
				if (i == _settings->end())
				{
					return false;
				}
				DynamicSettingBlock* settingBlock = i->second;
				IStreamReader* stream = settingBlock->OpenRead();
				ProfilingTypeSettingsCollection* temp = new ProfilingTypeSettingsCollection();
				if (temp->Initialize(stream))
				{
					_profilingTypesSettings = temp;
				}
				else
				{
					__FREEOBJ(temp);
				}
				__FREEOBJ(stream);
			}
			*profilingTypesSettings = _profilingTypesSettings;
			return _profilingTypesSettings != null;
		}

		__bool SessionSettings::GetFrameworksSettings(FrameworkSettingsCollection** frameworksSettings)
		{
			if (_frameworksSettings == null)
			{
				std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(SessionSettings::FrameworksSettingsSettingsIndex);
				if (i == _settings->end())
				{
					return false;
				}
				DynamicSettingBlock* settingBlock = i->second;
				IStreamReader* stream = settingBlock->OpenRead();
				FrameworkSettingsCollection* temp = new FrameworkSettingsCollection();
				if (temp->Initialize(stream))
				{
					_frameworksSettings = temp;
				}
				else
				{
					__FREEOBJ(temp);
				}
				__FREEOBJ(stream);
			}
			*frameworksSettings = _frameworksSettings;
			return _frameworksSettings != null;
		}

		__bool SessionSettings::GetGatewaySettings(GatewaySettings** gatewaySettings)
		{
			if (_gatewaySettings == null)
			{
				std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(SessionSettings::GatewaySettingsIndex);
				if (i == _settings->end())
				{
					return false;
				}
				DynamicSettingBlock* settingBlock = i->second;
				IStreamReader* stream = settingBlock->OpenRead();
				GatewaySettings* temp = new GatewaySettings();
				if (temp->Initialize(stream))
				{
					_gatewaySettings = temp;
				}
				else
				{
					__FREEOBJ(temp);
				}
				__FREEOBJ(stream);
			}
			*gatewaySettings = _gatewaySettings;
			return _gatewaySettings != null;
		}

		__bool SessionSettings::GetProfilingTargetSettings(ProfilingTargetSettings** profilingTargetSettings)
		{
			if (_profilingTargetSettings == null)
			{
				std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(SessionSettings::ProfilingTargetSettingsIndex);
				if (i == _settings->end())
				{
					return false;
				}
				DynamicSettingBlock* settingBlock = i->second;
				IStreamReader* stream = settingBlock->OpenRead();
				ProfilingTargetSettings* temp = new ProfilingTargetSettings();
				if (temp->Initialize(stream))
				{
					_profilingTargetSettings = temp;
				}
				else
				{
					__FREEOBJ(temp);
				}
				__FREEOBJ(stream);
			}
			*profilingTargetSettings = _profilingTargetSettings;
			return _profilingTargetSettings != null;
		}

		__bool SessionSettings::GetSessionUid(__guid* uid)
		{
			return GetUid(uid);
		}
		
		HRESULT SessionSettings::ValidateAndSort()
		{
			ProfilingTypeSettingsCollection* profilingTypesSettings = null;
			if (!GetProfilingTypesSettings(&profilingTypesSettings))
			{
				return E_FAIL;
			}
			__RETURN_IF_FAILED(profilingTypesSettings->ValidateAndSort());

			FrameworkSettingsCollection* frameworksSettings = null;
			if (!GetFrameworksSettings(&frameworksSettings))
			{
				return E_FAIL;
			}
			__RETURN_IF_FAILED(frameworksSettings->ValidateAndSort());

			return S_OK;
		}
		
		const __guid SessionSettings::ProfilingTypesSettingsIndex = Converter::ConvertStringToGuid(L"{71477AC3-3114-43B5-8C06-91889837C6A0}");
		const __guid SessionSettings::FrameworksSettingsSettingsIndex =  Converter::ConvertStringToGuid(L"{ABC75C78-E219-4652-ACD7-5A3749E0CC7E}");
		const __guid SessionSettings::GatewaySettingsIndex =  Converter::ConvertStringToGuid(L"{EF63A005-A7FC-4386-A8B1-68418C01671A}");
		const __guid SessionSettings::ProfilingTargetSettingsIndex =  Converter::ConvertStringToGuid(L"{71438E30-4BB7-4A10-BE9F-B2531B690930}");
	}
}