#pragma once
#define ENVIRONMENT_VARIABLE_MAX_SIZE 32767
class CEnvironmentVariables
{
public:
	static std::wstring Get(std::wstring variableName);
	static void Set(std::wstring variableName, std::wstring variableValue);
	static void Remove(std::wstring variableName);
};

