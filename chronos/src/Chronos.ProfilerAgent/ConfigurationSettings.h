#pragma once
#include <string>
#include <vector>
#include "BaseStream.h"
#include "TypeSize.h"
#include "StringMarshaler.h"

class CConfigurationSettings
{
public:
	CConfigurationSettings(void);
	~CConfigurationSettings(void);
	__bool Initialize(CBaseStream* stream);
	__byte InitialState;
	std::wstring Name;
	std::wstring Token;
	std::wstring TargetProcessName;
	std::wstring ProcessTargetArguments;
	__bool ProfileChildProcess;
	std::vector<std::wstring>* FilterItems;
	__uint EventsMask;
	__bool IsExclusionType;
	__bool UseFastHooks;
	__bool ProfileSql;
};

