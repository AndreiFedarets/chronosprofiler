#pragma once
#include "ClientBase.h"
#include "PipeNameFormatter.h"
#include "ConfigurationSettings.h"
#include "AgentSettings.h"
#include "ByteMarshaler.h"
#include "HostOperationCode.h"
#include "ResultCode.h"
#include "Exceptions.h"
#include "Int64Marshaler.h"
#include "CurrentProcess.h"
#include "Int32Marshaler.h"
#include "Timer.h"
#include "UInt32Marshaler.h"

class CHostClient : public CClientBase
{
public:
	CHostClient();
	__bool GetConfigurationSettings(std::wstring* configurationToken, CConfigurationSettings* settings);
	__bool StartProfilingSession(std::wstring* configurationToken, CAgentSettings* settings);
};

