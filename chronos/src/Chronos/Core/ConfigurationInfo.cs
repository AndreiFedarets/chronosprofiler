using System;

namespace Chronos.Core
{
	[Serializable]
	public class ConfigurationInfo
	{
		public ConfigurationInfo(ConfigurationSettings configurationSettings, ActivationSettings activationSettings)
		{
			ConfigurationSettings = configurationSettings;
			ActivationSettings = activationSettings;
		}

		public ConfigurationInfo()
		{

		}

		public ConfigurationSettings ConfigurationSettings { get; private set; }

		public ActivationSettings ActivationSettings { get; private set; }
	}
}
