using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace Chronos.Java
{
    public class FrameworkAdapter : IFrameworkAdapter, IInitializable, IDisposable
    {
        public FrameworkAdapter()
        {
        }

        public void Initialize(object application)
        {
        }

        public void Dispose()
        {
        }

        public void ConfigureForProfiling(ConfigurationSettings configurationSettings)
        {
            StringDictionary dictionary = configurationSettings.ProfilingTargetSettings.EnvironmentVariables;
            dictionary[Constants.EnvironmentVariablesNames.ProfilerSettingName] = string.Format(Constants.EnvironmentVariablesValues.ProfilerAgent, GetAgentLibraryFullName());
            configurationSettings.ProfilingTargetSettings.EnvironmentVariables = dictionary;
        }

        private string GetAgentLibraryFullName()
        {
            string fullName = Assembly.GetCallingAssembly().GetAssemblyPath();
            fullName = Path.Combine(fullName, Constants.AgentLibraryName);
            return fullName;
        }
    }
}
