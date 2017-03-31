#pragma once
#include "stdafx.h"
#include <Windows.h>
#include <dbghelp.h>

HRESULT GenerateDump(DWORD processId, wchar_t* dumpFileFullName, EXCEPTION_POINTERS* exceptionPointers)
{
	HRESULT error = S_OK;
	HANDLE fileHandle = CreateFileW(dumpFileFullName, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	if(fileHandle == INVALID_HANDLE_VALUE)
	{
		error = GetLastError();
		error = HRESULT_FROM_WIN32(error);
		return error;
	}
	HANDLE processHandle = OpenProcess( PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, processId);
	MINIDUMP_TYPE flags = (MINIDUMP_TYPE)(MiniDumpWithFullMemory | MiniDumpWithHandleData | MiniDumpWithUnloadedModules | MiniDumpWithProcessThreadData | MiniDumpWithFullMemoryInfo | MiniDumpWithThreadInfo);
	MINIDUMP_EXCEPTION_INFORMATION* exceptionInfoPointer = NULL;
	if (exceptionPointers != NULL)
	{
		MINIDUMP_EXCEPTION_INFORMATION exceptionInfo;
		exceptionInfo.ThreadId = GetCurrentThreadId();
        exceptionInfo.ExceptionPointers = exceptionPointers;
        exceptionInfo.ClientPointers = FALSE;
		*exceptionInfoPointer = exceptionInfo;
	}
	BOOL result = MiniDumpWriteDump(processHandle, processId, fileHandle, flags, exceptionInfoPointer, NULL, NULL);
	if(!result)
	{
		error = (HRESULT)GetLastError();
	}
	CloseHandle(fileHandle);
	return error;
}
