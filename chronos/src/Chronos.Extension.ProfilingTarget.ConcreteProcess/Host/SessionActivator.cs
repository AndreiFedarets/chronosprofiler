using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Chronos.Core;
using Chronos.Extensibility;
using Rhiannon.Win32;

namespace Chronos.Extension.ProfilingTarget.ConcreteProcess.Host
{
	public class SessionActivator : ISessionActivator
	{
		private readonly IProfilerAgentSelector _agentSelector;
		private readonly IList<System.Diagnostics.Process> _processes;
		private readonly Guid _configurationToken;

		public SessionActivator(ActivationSettings activationSettings, Guid configurationToken)
		{
			_agentSelector = new ProfilerAgentSelector();
			_processes = new List<System.Diagnostics.Process>();
			Settings = activationSettings;
			_configurationToken = configurationToken;
		}

		public ActivationSettings Settings { get; private set; }

		public void Activate()
		{
			string fileName = Settings.ProcessFullName;
			string args = Settings.ProcessArguments;
			StringDictionary environmentVariables = SetupEnvironmentVariables(_configurationToken);
			System.Diagnostics.Process process = Launcher.Launch(Settings.ConsoleSession, fileName, args, environmentVariables);
			_processes.Add(process);
		}

		public void Deactivate()
		{
			foreach (System.Diagnostics.Process process in _processes)
			{
				if (!process.HasExited)
				{
					process.CloseMainWindow();
				}
			}
		}

		public bool Validate()
		{
			string processFullName = Settings.ProcessFullName;
			return !string.IsNullOrEmpty(processFullName) && File.Exists(processFullName);
		}

		private StringDictionary SetupEnvironmentVariables(Guid configurationToken)
		{
			StringDictionary environmentVariables = new StringDictionary();
			string profilerGuid = _agentSelector.SelectProcessAgent(Settings.ProcessFullName);
			environmentVariables[Core.Constants.EnvironmentVariablesNames.EnableProfilingSettingName] = Core.Constants.EnvironmentVariablesValues.EnableProfiling;
			environmentVariables[Core.Constants.EnvironmentVariablesNames.ProfilerGuidSettingName] = profilerGuid;
			environmentVariables[Core.Constants.EnvironmentVariablesNames.CompatibilitySettingName] = Core.Constants.EnvironmentVariablesValues.CompatibilitySetting;
			environmentVariables[Core.Constants.EnvironmentVariablesNames.ConfigurationTokenSettingName] = configurationToken.ToString();
			return environmentVariables;
		}

		public ConfigurationState GetState()
		{
			return _processes.Any(x => !x.HasExited) ? ConfigurationState.Active : ConfigurationState.Inactive;
		}

        public void OnProcessConnected(int processId)
        {
            
        }
    }
}
