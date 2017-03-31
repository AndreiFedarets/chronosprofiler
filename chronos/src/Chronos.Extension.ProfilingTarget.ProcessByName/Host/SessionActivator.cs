using System;
using System.Collections;
using System.Collections.Specialized;
using Chronos.Core;
using Chronos.Extensibility;

namespace Chronos.Extension.ProfilingTarget.ProcessByName.Host
{
	public class SessionActivator : ISessionActivator
	{
		private readonly IProfilerAgentSelector _agentSelector;
		private readonly Guid _configurationToken;

		public SessionActivator(ActivationSettings activationSettings, Guid configurationToken)
		{
			_agentSelector = new ProfilerAgentSelector();
			Settings = activationSettings;
			_configurationToken = configurationToken;
		}

		public ActivationSettings Settings { get; private set; }

		public void Activate()
		{
			StringDictionary environmentVariables = SetupEnvironmentVariables(_configurationToken);
			foreach (DictionaryEntry entry in environmentVariables)
			{
				Environment.SetEnvironmentVariable(entry.Key.ToString(), entry.Value.ToString(), EnvironmentVariableTarget.Machine);
			}
		}

		public void Deactivate()
		{
			StringDictionary environmentVariables = SetupEnvironmentVariables(Guid.Empty);
			foreach (DictionaryEntry entry in environmentVariables)
			{
				Environment.SetEnvironmentVariable(entry.Key.ToString(), string.Empty, EnvironmentVariableTarget.Machine);
			}
		}

		public bool Validate()
		{
			return Settings.ProcessPlatform != ProcessPlatform.Native;
		}

		private StringDictionary SetupEnvironmentVariables(Guid configurationToken)
		{
			StringDictionary environmentVariables = new StringDictionary();
			string profilerGuid = _agentSelector.SelectProcessAgent(Settings.ProcessPlatform);
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.EnableProfilingSettingName] = Chronos.Core.Constants.EnvironmentVariablesValues.EnableProfiling;
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.ProfilerGuidSettingName] = profilerGuid;
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.CompatibilitySettingName] = Chronos.Core.Constants.EnvironmentVariablesValues.CompatibilitySetting;
			environmentVariables[Chronos.Core.Constants.EnvironmentVariablesNames.ConfigurationTokenSettingName] = configurationToken.ToString();
			return environmentVariables;
		}

		public ConfigurationState GetState()
		{
			string variable = Environment.GetEnvironmentVariable(Chronos.Core.Constants.EnvironmentVariablesNames.ConfigurationTokenSettingName, EnvironmentVariableTarget.Machine);
			return string.IsNullOrEmpty(variable) ? ConfigurationState.Inactive : ConfigurationState.Active;
		}

        public void OnProcessConnected(int processId)
        {
            Deactivate();
        }
    }
}
