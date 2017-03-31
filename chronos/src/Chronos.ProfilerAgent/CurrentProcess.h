#pragma once
#include "PathResolver.h"

class CCurrentProcess
{
public:
	static DWORD GetProcessId();
	static std::wstring GetName();
	static std::wstring GetFullName();
	static std::wstring GetArguments();
	static SYSTEMTIME GetCreationTime();
private:
	static void Initialize();
	static std::wstring _fullName;
	static std::wstring _arguments;
};

