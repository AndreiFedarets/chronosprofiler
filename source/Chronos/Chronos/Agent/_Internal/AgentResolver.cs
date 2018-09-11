using System;
using System.Reflection;

namespace Chronos.Agent
{
    internal static class AgentResolver
    {
        public static void SetupAgentPath(ConfigurationSettings settings)
        {
            string variableValue = Environment.GetEnvironmentVariable(Constants.EnvironmentVariablesNames.Path);
            string chronosPath = Assembly.GetExecutingAssembly().GetAssemblyPath();
            variableValue = EnvironmentExtensions.AppendEnvironmentPath(variableValue, chronosPath);
            settings.ProfilingTargetSettings.EnvironmentVariables[Constants.EnvironmentVariablesNames.Path] = variableValue;
        }
    }
}
