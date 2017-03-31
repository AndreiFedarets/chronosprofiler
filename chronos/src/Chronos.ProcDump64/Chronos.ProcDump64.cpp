// Chronos.ProcDump64.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "ProcDump.h"
#include "Convert.h"

EXTERN_C __declspec(dllexport) HRESULT GenerateDump64(DWORD processId, wchar_t* dumpFileFullName)
{
	return GenerateDump(processId, dumpFileFullName, NULL);
}

int _tmain(int argc, _TCHAR* argv[])
{
	if (argc < 3)
	{
		return 0;
	}
	wchar_t* processIdArgument = argv[1];
	wchar_t* dumpFileFullName = argv[2];
	DWORD processId = CConvert::ToInt(std::wstring(processIdArgument));
	GenerateDump64(processId, dumpFileFullName);
	return 0;
}
