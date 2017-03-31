using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Communication.Remoting;
using Chronos.Core;
using Rhiannon.Extensions;

namespace Chronos.Host.Proxy
{
	internal class ConfigurationCollection : List<IConfiguration>, IConfigurationCollection
	{
		private readonly IRemotingExecutor _executor;
		private readonly IConfigurationCollection _configurations;
		private readonly ConfigurationCollectionEventsRouter _eventsRouter;

		public ConfigurationCollection(IConfigurationCollection configurations, IRemotingExecutor executor)
		{
			_executor = executor;
			_configurations = configurations;
			_eventsRouter = new ConfigurationCollectionEventsRouter(_configurations, OnConfigurationCreated, OnConfigurationRemoved);
			_executor.Execute(Initialize);
		}

		public IConfiguration this[Guid configurationToken]
		{
			get
			{
				IConfiguration configuration = this.FirstOrDefault(x => x.Token == configurationToken);
				return configuration;
			}
		}

		public event Action<IConfiguration> ConfigurationCreated;

		public event Action<IConfiguration> ConfigurationRemoved;

		public IConfiguration Create(ConfigurationSettings configurationSettings, ActivationSettings activationSettings)
		{
			IConfiguration configuration = _executor.Execute(() => _configurations.Create(configurationSettings, activationSettings));
			return ProxyFactory.Proxy(configuration, _executor);
		}

		public bool Contains(Guid configurationToken)
		{
			return this.Any(x => x.Token == configurationToken);
		}

		public void Dispose()
		{
			_executor.Execute(() => _eventsRouter.Dispose());
		}

		private void Initialize()
		{
			foreach (IConfiguration configuration in _configurations)
			{
				IConfiguration proxy = ProxyFactory.Proxy(configuration, _executor);
				Add(proxy);
			}
		}

		private void OnConfigurationCreated(IConfiguration configuration)
		{
			IConfiguration proxy = ProxyFactory.Proxy(configuration, _executor);
			Add(proxy);
			ConfigurationCreated.SafeInvoke(proxy);
		}

		private void OnConfigurationRemoved(IConfiguration configuration)
		{
			IConfiguration proxy = this[configuration.Token];
			Remove(proxy);
			ConfigurationRemoved.SafeInvoke(proxy);
		}
	}
}
