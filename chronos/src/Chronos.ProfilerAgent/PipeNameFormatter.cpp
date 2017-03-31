#include "StdAfx.h"
#include "PipeNameFormatter.h"

std::wstring CPipeNameFormatter::GetHostServerPipeName()
{
	return L"\\\\.\\pipe\\chronosprofiler-host";
}

std::wstring CPipeNameFormatter::GetDaemonServerPipeName(std::wstring* daemonToken)
{
	std::wstring value(L"\\\\.\\pipe\\chronosprofiler-");
	value.append(*daemonToken);
	value.append(L"-agent");
	return value;
}

std::wstring CPipeNameFormatter::GetDaemonThreadPipeName(std::wstring* daemonToken, __uint threadStreamIndex)
{
	std::wstring value(L"\\\\.\\pipe\\chronosprofiler-");
	value.append(*daemonToken);
	value.append(L"-thread-");
	value.append(CConvert::ToString(threadStreamIndex));
	return value;
}

std::wstring CPipeNameFormatter::GetDaemonUnitPipeName(std::wstring* daemonToken, __uint unitType)
{
	std::wstring value(L"\\\\.\\pipe\\chronosprofiler-");
	value.append(*daemonToken);
	value.append(L"-unit-");
	value.append(CConvert::ToString(unitType));
	return value;
}

std::wstring CPipeNameFormatter::GetAgentServerPipeName(__uint pid)
{
	std::wstring value(L"\\\\.\\pipe\\chronosprofiler-agent-");
	value.append(CConvert::ToString(pid));
	return value;
}