using System;
using System.Collections.Specialized;

namespace Chronos.DotNet
{
    public class FrameworkAdapter : IFrameworkAdapter, IInitializable, IDisposable
    {
        private readonly AgentInstaller _installer;

        public FrameworkAdapter()
        {
            _installer = new AgentInstaller();
        }

        public void Initialize(object application)
        {
            _installer.Install();
        }

        public void Dispose()
        {
            _installer.Uninstall();
        }

        public void ConfigureForProfiling(ConfigurationSettings configurationSettings)
        {
            StringDictionary dictionary = configurationSettings.ProfilingTargetSettings.EnvironmentVariables;
            dictionary[Constants.EnvironmentVariablesNames.EnableProfilingSettingName] = Constants.EnvironmentVariablesValues.EnableProfiling;
            dictionary[Constants.EnvironmentVariablesNames.ProfilerGuidSettingName] = Constants.EnvironmentVariablesValues.ProfilerAgentGuid;
            configurationSettings.ProfilingTargetSettings.EnvironmentVariables = dictionary;
        }
    }
}
