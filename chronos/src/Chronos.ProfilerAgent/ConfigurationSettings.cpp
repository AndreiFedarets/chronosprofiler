#include "StdAfx.h"
#include "ConfigurationSettings.h"


CConfigurationSettings::CConfigurationSettings(void)
{
}

CConfigurationSettings::~CConfigurationSettings(void)
{
	
}

__bool CConfigurationSettings::Initialize(CBaseStream* stream)
{
	//Name
	Name = *CStringMarshaler::Demarshal(stream);
	//Token
	Token = *CStringMarshaler::Demarshal(stream);
	//InitialState
	stream->Read(&InitialState, TypeSize::_BYTE);
	//EventsMask
	stream->Read(&EventsMask, TypeSize::_INT32);
	//TargetProcessName
	TargetProcessName = *CStringMarshaler::Demarshal(stream);
	//ProcessTargetArguments
	ProcessTargetArguments = *CStringMarshaler::Demarshal(stream);
	//ProfileChildProcess
	stream->Read(&ProfileChildProcess, TypeSize::_BOOL);
	//ExclusionsType
	__byte filterType = 0;
	stream->Read(&filterType, TypeSize::_BYTE);
	IsExclusionType = filterType == 1;
	//UseFastHooks
	stream->Read(&UseFastHooks, TypeSize::_BOOL);
	//ProfileSql
	stream->Read(&ProfileSql, TypeSize::_BOOL);
	//Exclusions
	__uint size = 0;
	stream->Read(&size, TypeSize::_INT32);
	FilterItems = new std::vector<std::wstring>();
	for (__uint i = 0; i < size; i++)
	{
		std::wstring filterItem = *CStringMarshaler::Demarshal(stream);
		FilterItems->push_back(filterItem);
	}
	return true;
}

