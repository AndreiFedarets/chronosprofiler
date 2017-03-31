using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Communication.Remoting;
using Chronos.Core;
using Chronos.Extensibility;

namespace Chronos.Host.Internal
{
	internal class ConfigurationCollection : SingletonMarshalByRefObject, IConfigurationCollection
	{
		private readonly ISessionActivatorProvider _sessionActivatorProvider;
		private readonly IDictionary<Guid, Configuration> _configurations;
		private readonly EventActionsHolder<IConfiguration> _configurationCreatedEvent;
		private readonly EventActionsHolder<IConfiguration> _configurationRemovedEvent;
		private bool _disposed;

		public ConfigurationCollection(ISessionActivatorProvider sessionActivatorProvider)
		{
			_configurations = new Dictionary<Guid, Configuration>();
			_sessionActivatorProvider = sessionActivatorProvider;
			_configurationCreatedEvent = new EventActionsHolder<IConfiguration>();
			_configurationRemovedEvent = new EventActionsHolder<IConfiguration>();
		}

		public IConfiguration this[Guid configurationToken]
		{
			get
			{
				Configuration configuration;
				_configurations.TryGetValue(configurationToken, out configuration);
				return configuration;
			}
		}

		public event Action<IConfiguration> ConfigurationCreated
		{
			add { _configurationCreatedEvent.Add(value); }
			remove { _configurationCreatedEvent.Remove(value);}
		}

		public event Action<IConfiguration> ConfigurationRemoved
		{
			add { _configurationRemovedEvent.Add(value); }
			remove { _configurationRemovedEvent.Remove(value); }
		}

		public IConfiguration Create(ConfigurationSettings configurationSettings, ActivationSettings activationSettings)
		{
			if (Contains(configurationSettings.Token))
			{
				throw new ArgumentException("token");
			}
			Configuration configuration = new Configuration(configurationSettings, activationSettings, _sessionActivatorProvider, this);
			_configurations.Add(configuration.Token, configuration);
			_configurationCreatedEvent.Invoke(configuration);
			return configuration;
		}

		public bool Contains(Guid configurationToken)
		{
			return _configurations.ContainsKey(configurationToken);
		}

		public IConfiguration GetConfiguration(Guid configurationToken)
		{
			return _configurations[configurationToken];
		}

		public void OnConfigurationRemove(Configuration configuration)
		{
			_configurations.Remove(configuration.Token);
			_configurationRemovedEvent.Invoke(configuration);
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
            foreach (IConfiguration configuration in this)
            {
                configuration.Deactivate();
            }
			_disposed = true;
		}

		public IEnumerator<IConfiguration> GetEnumerator()
		{
			IList<IConfiguration> configurations = _configurations.Values.Cast<IConfiguration>().ToList();
			return configurations.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
