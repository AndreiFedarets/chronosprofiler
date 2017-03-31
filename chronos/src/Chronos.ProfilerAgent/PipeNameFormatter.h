#pragma once
#include "Convert.h"

class CPipeNameFormatter
{
public:
	static std::wstring GetHostServerPipeName();
	static std::wstring GetDaemonServerPipeName(std::wstring* daemonToken);
	static std::wstring GetDaemonThreadPipeName(std::wstring* daemonToken, __uint threadStreamIndex);
	static std::wstring GetDaemonUnitPipeName(std::wstring* daemonToken, __uint unitType);
	static std::wstring GetAgentServerPipeName(__uint pid);
};

