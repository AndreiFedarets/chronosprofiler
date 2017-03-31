#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		__string EnvironmentVariables::Get(__string variableName)
		{
			__wchar nativeValue[ENVIRONMENT_VARIABLE_MAX_SIZE];
			memset(nativeValue, 0, ENVIRONMENT_VARIABLE_MAX_SIZE * sizeof(__wchar));
			GetEnvironmentVariableW(variableName.c_str(), nativeValue, ENVIRONMENT_VARIABLE_MAX_SIZE);
			return __string(nativeValue);
		}

		void EnvironmentVariables::Set(__string variableName, __string variableValue)
		{
			SetEnvironmentVariableW(variableName.c_str(), variableValue.c_str());
		}

		void EnvironmentVariables::Remove(__string variableName)
		{
			SetEnvironmentVariableW(variableName.c_str(), L"");
		}
	}
}
