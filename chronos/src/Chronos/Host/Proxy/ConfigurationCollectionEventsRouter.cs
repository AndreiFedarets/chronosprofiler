using System;

namespace Chronos.Host.Proxy
{
	internal class ConfigurationCollectionEventsRouter : MarshalByRefObject, IDisposable
	{
		private readonly IConfigurationCollection _configurations;
		private readonly Action<IConfiguration> _configurationCreated;
		private readonly Action<IConfiguration> _configurationRemoved;

		public ConfigurationCollectionEventsRouter(IConfigurationCollection configurations, Action<IConfiguration> configurationCreated,
			Action<IConfiguration> configurationRemoved)
		{
			_configurations = configurations;
			_configurationCreated = configurationCreated;
			_configurationRemoved = configurationRemoved;
			_configurations.ConfigurationCreated += OnConfigurationCreated;
			_configurations.ConfigurationRemoved += OnConfigurationRemoved;
		}

		public void Dispose()
		{
			_configurations.ConfigurationCreated -= OnConfigurationCreated;
			_configurations.ConfigurationRemoved -= OnConfigurationRemoved;
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public void OnConfigurationCreated(IConfiguration configuration)
		{
			_configurationCreated.BeginInvoke(configuration, null, null);
		}

		public void OnConfigurationRemoved(IConfiguration configuration)
		{
			_configurationRemoved.BeginInvoke(configuration, null, null);
		}
	}
}
