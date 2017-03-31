#include "StdAfx.h"
#include <Psapi.h>
#include "CurrentProcess.h"


DWORD CCurrentProcess::GetProcessId()
{
	return GetCurrentProcessId();
}

std::wstring CCurrentProcess::GetName()
{
	std::wstring fullName = GetFullName();
	return CPathResolver::GetFileName(fullName);
}

std::wstring CCurrentProcess::GetFullName()
{
	HANDLE processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, GetProcessId());
	const size_t nameBufferLength = 2048;
	__wchar* nameBuffer = new __wchar[nameBufferLength];
	GetProcessImageFileNameW(processHandle, nameBuffer, nameBufferLength);
	return nameBuffer;
}

std::wstring CCurrentProcess::GetArguments()
{
	__wchar* commandLine = GetCommandLineW();
	return commandLine;
}

SYSTEMTIME CCurrentProcess::GetCreationTime()
{
	FILETIME creationTime;
	FILETIME exitTime;
	FILETIME kernelTime;
	FILETIME userTime;
	FILETIME localCreationTime;
	SYSTEMTIME systemTime;
	HANDLE processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, GetProcessId());
	GetProcessTimes(processHandle, &creationTime, &exitTime, &kernelTime, &userTime);
	FileTimeToLocalFileTime(&creationTime, &localCreationTime);
	FileTimeToSystemTime(&localCreationTime, &systemTime);
	return systemTime;
}
