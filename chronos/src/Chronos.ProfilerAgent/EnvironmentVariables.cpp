#include "StdAfx.h"
#include "EnvironmentVariables.h"

std::wstring CEnvironmentVariables::Get(std::wstring variableName)
{
	__wchar* nativeValue = new __wchar[ENVIRONMENT_VARIABLE_MAX_SIZE];
	memset(nativeValue, 0, ENVIRONMENT_VARIABLE_MAX_SIZE * sizeof(__wchar));
	GetEnvironmentVariableW(variableName.c_str(), nativeValue, ENVIRONMENT_VARIABLE_MAX_SIZE);
	return std::wstring(nativeValue);
}

void CEnvironmentVariables::Set(std::wstring variableName, std::wstring variableValue)
{
	SetEnvironmentVariableW(variableName.c_str(), variableValue.c_str());
}

void CEnvironmentVariables::Remove(std::wstring variableName)
{
	SetEnvironmentVariableW(variableName.c_str(), L"");
}
