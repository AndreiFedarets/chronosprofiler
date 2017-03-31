using System;
using System.Collections.Specialized;
using Chronos.Core;
using Chronos.Extensibility;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService.Host
{
	public class SessionActivator : ISessionActivator
	{
		private readonly IProfilerAgentSelector _agentSelector;
		private readonly InternetInformationService _internetInformationService;
		private readonly Guid _configurationToken;
	    private IApplicationPool _applicationPool;

		public SessionActivator(ActivationSettings activationSettings, Guid configurationToken)
		{
			_agentSelector = new ProfilerAgentSelector();
			_internetInformationService = new InternetInformationService();
			Settings = activationSettings;
			_configurationToken = configurationToken;
		}

		public Guid ConfigurationToken { get; private set; }

		public ActivationSettings Settings { get; private set; }

		public void Activate()
		{
			ConfigurationToken = _configurationToken;
			StringDictionary environmentVariables = SetupEnvironmentVariables(_configurationToken);
			_internetInformationService.AppendEnvironmentVariables(environmentVariables);
			_internetInformationService.CloseHostProcesses();
			_internetInformationService.Stop();
            _internetInformationService.Start();
            _applicationPool = _internetInformationService.ApplicationPools[Settings.AppPoolName];
            _applicationPool.Restart();
            _internetInformationService.RemoveEnvironmentVariables(environmentVariables);
		}

		public void Deactivate()
		{
			StringDictionary environmentVariables = SetupEnvironmentVariables(ConfigurationToken);
			_internetInformationService.RemoveEnvironmentVariables(environmentVariables);
			_internetInformationService.CloseHostProcesses();
			_internetInformationService.Stop();
			_internetInformationService.Start();
		}

		public bool Validate()
		{
			string appPoolName = Settings.AppPoolName;
			return !string.IsNullOrEmpty(appPoolName);
		}

		private StringDictionary SetupEnvironmentVariables(Guid configurationToken)
		{
			StringDictionary environmentVariables = new StringDictionary();
			string profilerGuid = _agentSelector.GetCurrentSystemProcessAgent();
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.EnableProfilingSettingName] = Core.Constants.EnvironmentVariablesValues.EnableProfiling;
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.ProfilerGuidSettingName] = profilerGuid;
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.CompatibilitySettingName] = Core.Constants.EnvironmentVariablesValues.CompatibilitySetting;
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.ConfigurationTokenSettingName] = configurationToken.ToString();
			return environmentVariables;
		}

		public ConfigurationState GetState()
		{
			StringDictionary serviceVariables = _internetInformationService.GetEnvironmentVariables();
			StringDictionary profilerVariables = SetupEnvironmentVariables(ConfigurationToken);
			bool contains = EnvironmentVariablesConverter.Contains(profilerVariables, serviceVariables);
			return contains ? ConfigurationState.Active : ConfigurationState.Inactive;
		}

        public void OnProcessConnected(int processId)
        {
            
        }
    }
}
