#include "StdAfx.h"
#include "Chronos.Agent.Internal.h"
//#include <Windows.h>
#include <dbghelp.h>
#include <time.h>

namespace Chronos
{
	namespace Agent
	{
		LONG WINAPI ChronosUnhandledExceptionFilter(struct _EXCEPTION_POINTERS* exceptionPointers)
		{
			DWORD error = 0;
			DWORD processId = GetCurrentProcessId();
			
			time_t timeNow = time(0);
			struct tm localTimeNow;
			localtime_s(&localTimeNow, &timeNow);
			__string dumpFileName = Formatter::Format(L"%s_%d_%d-%d-%d.dmp", CurrentProcess::GetProcessName().c_str(), GetCurrentProcessId(), localTimeNow.tm_hour, localTimeNow.tm_min, localTimeNow.tm_sec);

			__string dumpFileFullName = Path::Combine(CrashDumpLogger::GetDumpsDirectoryPath(), dumpFileName);
			HANDLE fileHandle = CreateFileW(dumpFileFullName.c_str(), GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

			if (fileHandle != INVALID_HANDLE_VALUE)
			{
				HANDLE processHandle = OpenProcess( PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, processId);
				MINIDUMP_TYPE flags = (MINIDUMP_TYPE)(MiniDumpWithFullMemory | MiniDumpWithHandleData | MiniDumpWithUnloadedModules | MiniDumpWithProcessThreadData | MiniDumpWithFullMemoryInfo | MiniDumpWithThreadInfo);
				MINIDUMP_EXCEPTION_INFORMATION* exceptionInfoPointer = NULL;
				if (exceptionPointers != null)
				{
					MINIDUMP_EXCEPTION_INFORMATION* exceptionInfo = new MINIDUMP_EXCEPTION_INFORMATION();
					exceptionInfo->ThreadId = GetCurrentThreadId();
					exceptionInfo->ExceptionPointers = exceptionPointers;
					exceptionInfo->ClientPointers = FALSE;
					exceptionInfoPointer = exceptionInfo;
				}
				BOOL result = MiniDumpWriteDump(processHandle, processId, fileHandle, flags, exceptionInfoPointer, null, null);
				if (exceptionInfoPointer != null)
				{
					delete exceptionInfoPointer;
				}
				if(!result)
				{
					error = GetLastError();
					__ASSERT(true, L"Error generation crash dump");
				}
				CloseHandle(fileHandle);
			}
			else 
			{
				error = GetLastError();
				__ASSERT(true, L"Error generation crash dump");
			}
			return EXCEPTION_CONTINUE_SEARCH;
		}

		void CrashDumpLogger::Setup(__string dumpsDirectoryPath)
		{
			if (!_initialized)
			{
				_initialized = true;
				_dumpsDirectoryPath = dumpsDirectoryPath;
				SetUnhandledExceptionFilter(ChronosUnhandledExceptionFilter);
			}
		}

		__string CrashDumpLogger::GetDumpsDirectoryPath()
		{
			return _dumpsDirectoryPath;
		}

		__string CrashDumpLogger::_dumpsDirectoryPath;
		 __bool CrashDumpLogger::_initialized = false;
	}
}