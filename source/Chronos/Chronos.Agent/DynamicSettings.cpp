#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		DynamicSettings::DynamicSettings()
		{
			_initialized = false;
			_settings = new std::map<__guid, DynamicSettingBlock*>();
		}

		DynamicSettings::~DynamicSettings()
		{
			if (_settings == null)
			{
				return;
			}
			while (!_settings->empty())
			{
				std::map<__guid, DynamicSettingBlock*>::iterator i = _settings->begin();
				__guid settingToken = i->first;
				DynamicSettingBlock* block = i->second;
				_settings->erase(settingToken);
				__FREEOBJ(block);
			}
			__FREEOBJ(_settings);
		}
		
		bool DynamicSettings::Initialize(IStreamReader* stream)
		{
			if (_initialized)
			{
				return true;
			}
			_initialized = true;
			__int count = Marshaler::DemarshalInt(stream);
			for (__int i = 0; i < count; i++)
			{
				__guid key = Marshaler::DemarshalGuid(stream);
				__int blockSize = Marshaler::DemarshalInt(stream);
				__byte* block = new __byte[blockSize];
				stream->Read(block, blockSize);
				_settings->insert(std::pair<__guid, DynamicSettingBlock*>(key, new DynamicSettingBlock(blockSize, block)));
			}
			return true;
		}

		DynamicSettingBlock* DynamicSettings::GetSettingBlock(__guid settingToken)
		{
			std::map<GUID, DynamicSettingBlock*>::iterator i = _settings->find(settingToken);
			DynamicSettingBlock* result = null;
			if (i != _settings->end())
			{
				result = i->second;
			}
			return result;
		}
	}
}