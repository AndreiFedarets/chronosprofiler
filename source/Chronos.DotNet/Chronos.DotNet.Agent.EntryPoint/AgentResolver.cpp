#include "stdafx.h"
#include "Chronos.DotNet.Agent.EntryPoint.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace EntryPoint
			{
				AgentResolver::AgentResolver()
				{
					MessageBox(NULL, L"S", L"S", MB_OK);
					SetupAgentPath();
				}

				AgentResolver::~AgentResolver()
				{
				}

				void AgentResolver::SetupAgentPath()
				{
					__string agentPath;
					if (sizeof(UINT_PTR) == 4)
					{
						agentPath = GetVariable(L"CHRONOS_PROFILER_AGENT_PATH_32");
					}
					else if (sizeof(UINT_PTR) == 8)
					{
						agentPath = GetVariable(L"CHRONOS_PROFILER_AGENT_PATH_64");
					}
					if (!agentPath.empty())
					{
						DLL_DIRECTORY_COOKIE cookie = AddDllDirectory(agentPath.c_str());
						/*__string path = GetVariable(L"PATH");
						path.append(L";");
						path.append(agentPath);
						SetVariable(L"PATH", path);*/
					}
				}


				__string AgentResolver::GetVariable(__string variableName)
				{
					__wchar* nativeValue = new __wchar[ENVIRONMENT_VARIABLE_MAX_SIZE];
					memset(nativeValue, 0, ENVIRONMENT_VARIABLE_MAX_SIZE * sizeof(__wchar));
					GetEnvironmentVariableW(variableName.c_str(), nativeValue, ENVIRONMENT_VARIABLE_MAX_SIZE);
					__string value(nativeValue);
					__FREEOBJ(nativeValue);
					return value;
				}

				void AgentResolver::SetVariable(__string variableName, __string variableValue)
				{
					SetEnvironmentVariableW(variableName.c_str(), variableValue.c_str());
				}
			}
		}
	}
}