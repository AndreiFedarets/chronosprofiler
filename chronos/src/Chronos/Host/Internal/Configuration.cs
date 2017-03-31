using System;
using Chronos.Core;
using Chronos.Extensibility;

namespace Chronos.Host.Internal
{
	internal class Configuration : SingletonMarshalByRefObject, IConfiguration
	{
		private readonly ISessionActivator _sessionActivator;
		private readonly ConfigurationCollection _configurations;

		public Configuration(ConfigurationSettings configurationSettings, ActivationSettings activationSettings,
			ISessionActivatorProvider activatorProvider, ConfigurationCollection configurations)
		{
			Token = configurationSettings.Token;
			Name = configurationSettings.Name;
			ISessionActivator sessionActivator;
			if (!activatorProvider.TryCreate(activationSettings, Token, out sessionActivator))
			{
				throw new ArgumentException("activationSettings");
			}
			_sessionActivator = sessionActivator;
			_configurations = configurations;
			ConfigurationSettings = configurationSettings;
		}

		public ConfigurationState State
		{
			get { return _sessionActivator.GetState(); }
		}

		public Guid Token { get; private set; }

		public ConfigurationSettings ConfigurationSettings { get; private set; }

		public ActivationSettings ActivationSettings
		{
			get { return _sessionActivator.Settings; }
		}

		public string Name { get; private set; }

		public void Activate()
		{
			_sessionActivator.Activate();
		}

		public void Deactivate()
		{
			_sessionActivator.Deactivate();
		}

		public void Remove()
		{
			_configurations.OnConfigurationRemove(this);
			//TODO: Remove ConfigurationSettings
		}

        public void OnProcessConnected(int processId)
        {
            _sessionActivator.OnProcessConnected(processId);
        }
	}
}
