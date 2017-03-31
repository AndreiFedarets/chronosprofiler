using System;
using Chronos.Communication.Remoting;
using Chronos.Core;

namespace Chronos.Host.Proxy
{
	internal class Configuration : IConfiguration
	{
		private readonly IRemotingExecutor _executor;
		private readonly IConfiguration _configuration;
		private ConfigurationSettings _configurationSettings;
		private ActivationSettings _activationSettings;
		private Guid _token;
		private string _name;

		public Configuration(IConfiguration configuration, IRemotingExecutor executor)
		{
			_executor = executor;
			_configuration = configuration;
		}

		public Guid Token
		{
			get
			{
				if (_token == Guid.Empty)
				{
					_token = _executor.Execute(() =>  _configuration.Token);
				}
				return _token;
			}
		}

		public string Name
		{
			get
			{
				if (_name == null)
				{
					_name = _executor.Execute(() =>  _configuration.Name);
				}
				return _name;
			}
		}

		public ConfigurationSettings ConfigurationSettings
		{
			get
			{
				if (_configurationSettings == null)
				{
					_configurationSettings = _executor.Execute(() =>  _configuration.ConfigurationSettings);
				}
				return _configurationSettings;
			}
		}

		public ActivationSettings ActivationSettings
		{
			get
			{
				if (_activationSettings == null)
				{
					_activationSettings = _executor.Execute(() =>  _configuration.ActivationSettings);
				}
				return _activationSettings;
			}
		}

		public ConfigurationState State
		{
			get { return _executor.Execute(() =>  _configuration.State); }
		}

		public void Activate()
		{
			_executor.Execute(() =>  _configuration.Activate());
		}

		public void Deactivate()
		{
			_executor.Execute(() =>  _configuration.Deactivate());
		}

		public void Remove()
		{
			_executor.Execute(() =>  _configuration.Remove());
		}
	}
}
