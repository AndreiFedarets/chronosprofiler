#pragma once
#include <string>
#include <vector>
#include "BaseStream.h"
#include "TypeSize.h"
#include "StringMarshaler.h"

class CAgentSettings
{
public:
	CAgentSettings(void);
	~CAgentSettings(void);
	__bool Initialize(CBaseStream* stream);
	std::wstring* SessionToken;
	__uint CallPageSize;
	__uint ThreadStreamsCount;
};

