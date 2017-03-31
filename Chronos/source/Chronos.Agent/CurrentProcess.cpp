#include "stdafx.h"
#include "Chronos.Agent.h"
#include <Psapi.h>

namespace Chronos
{
	namespace Agent
	{
		__uint CurrentProcess::GetProcessId()
		{
			return GetCurrentProcessId();
		}

		__string CurrentProcess::GetArguments()
		{
			__wchar* commandLineBuffer = GetCommandLineW();
			__string commandLine(commandLineBuffer);
			__FREEARR(commandLineBuffer);
			return commandLine;
		}

		SYSTEMTIME CurrentProcess::GetCreationTime()
		{
			FILETIME creationTime;
			FILETIME exitTime;
			FILETIME kernelTime;
			FILETIME userTime;
			FILETIME localCreationTime;
			SYSTEMTIME systemTime;
			GetProcessTimes(GetProcessHandle(), &creationTime, &exitTime, &kernelTime, &userTime);
			FileTimeToLocalFileTime(&creationTime, &localCreationTime);
			FileTimeToSystemTime(&localCreationTime, &systemTime);
			return systemTime;
		}

		HANDLE CurrentProcess::GetProcessHandle()
		{
			if (_processHandle == 0)
			{
				_processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, GetProcessId());
			}
			return _processHandle;
		}

		__int CurrentProcess::GetProcessPlatform()
		{
			switch (sizeof(UINT_PTR))
			{
				case 4:
					return CurrentProcess::I386;
				case 8:
					return CurrentProcess::X64;
				default:
					return CurrentProcess::Unknown;
			}
		}

		__string CurrentProcess::GetProcessName()
		{
			HANDLE processHandle = GetProcessHandle();
			__string name;
			if (processHandle) 
			{
				LPWSTR buffer = new wchar_t[MAX_PATH];
				if (GetProcessImageFileNameW(processHandle, buffer, MAX_PATH))
				{
					name = Path::GetFileName(buffer);
				}
			}
			return name;
		}

		HANDLE CurrentProcess::_processHandle = 0;
	}
}