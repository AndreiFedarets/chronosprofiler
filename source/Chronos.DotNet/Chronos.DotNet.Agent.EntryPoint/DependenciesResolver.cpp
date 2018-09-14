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
				DependenciesResolver::DependenciesResolver()
				{
					LoadDependencies();
				}

				DependenciesResolver::~DependenciesResolver()
				{
				}

				__string DependenciesResolver::GetAgentPath()
				{
					__string variableName;
					if (sizeof(UINT_PTR) == 4)
					{
						variableName = L"CHRONOS_PROFILER_AGENT_PATH_32";
					}
					else if (sizeof(UINT_PTR) == 8)
					{
						variableName = L"CHRONOS_PROFILER_AGENT_PATH_64";
					}
					__wchar* variableValue = new __wchar[ENVIRONMENT_VARIABLE_MAX_SIZE];
					memset(variableValue, 0, ENVIRONMENT_VARIABLE_MAX_SIZE * sizeof(__wchar));
					GetEnvironmentVariableW(variableName.c_str(), variableValue, ENVIRONMENT_VARIABLE_MAX_SIZE);
					__string agentPath(variableValue);
					agentPath.append(L"\\Chronos.Agent.dll");
					__FREEOBJ(variableValue);
					return agentPath;
				}

				__string DependenciesResolver::GetDotNetAgentPath()
				{
					wchar_t path[MAX_PATH];
					HMODULE moduleHandle = NULL;
					if (GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS |	GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT, L"DllMain", &moduleHandle) == 0)
					{
						return __string();
					}
					if (GetModuleFileName(moduleHandle, path, sizeof(path)) == 0)
					{
						return __string();
					}
					PathRemoveFileSpecW(path);
					__string agentPath(path);
					agentPath.append(L"\\Chronos.DotNet.Agent.dll");
					return agentPath;
				}

				void DependenciesResolver::LoadDependencies()
				{
					__string agentPath = GetAgentPath();
					HMODULE agentModule = LoadLibrary(agentPath.c_str());
					__string dotNetAgentPath = GetDotNetAgentPath();
					HMODULE dothNetAgentModule = LoadLibrary(dotNetAgentPath.c_str());
				}
			}
		}
	}
}