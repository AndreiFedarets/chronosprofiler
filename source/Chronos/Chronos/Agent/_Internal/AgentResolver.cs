using System;
using System.IO;
using System.Reflection;

namespace Chronos.Agent
{
    internal static class AgentResolver
    {
        public static void SetupAgentPath(ConfigurationSettings settings)
        {
            string agentPath32 = Assembly.GetExecutingAssembly().GetAssemblyPath();
            string agentPath64 = Path.Combine(agentPath32, "x64");
            settings.ProfilingTargetSettings.EnvironmentVariables[Constants.EnvironmentVariablesNames.AgentPath32] = agentPath32;
            settings.ProfilingTargetSettings.EnvironmentVariables[Constants.EnvironmentVariablesNames.AgentPath64] = agentPath64;

        }
    }
}
