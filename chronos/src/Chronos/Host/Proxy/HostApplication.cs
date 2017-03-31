using Chronos.Communication.Remoting;

namespace Chronos.Host.Proxy
{
	internal class HostApplication : IHostApplication
	{
		private readonly IRemotingExecutor _executor;
		private readonly IHostApplication _application;
		private IConfigurationCollection _configurations;
		private ISessionCollection _sessions;

		public HostApplication(IHostApplication application, IRemotingExecutor executor)
		{
			_executor = executor;
			_application = application;
		}

		public IConfigurationCollection Configurations
		{
			get
			{
				if (_configurations == null)
				{
					IConfigurationCollection configurations = _executor.Execute(() => _application.Configurations);
					_configurations = ProxyFactory.Proxy(configurations, _executor);
				}
				return _configurations;
			}
		}

		public string MachineName
		{
			get { return _executor.Execute(() => _application.MachineName); }
		}

		public ISessionCollection Sessions
		{
			get
			{
				if (_sessions == null)
				{
					ISessionCollection sessions = _executor.Execute(() => _application.Sessions);
					_sessions = ProxyFactory.Proxy(sessions, _executor);
				}
				return _sessions;
			}
		}

		public void Quit()
		{
			Dispose();
			_executor.Execute(() =>_application.Quit());
		}

		public string Ping(string message)
		{
			return _executor.Execute(() => _application.Ping(message));
		}

		public void Dispose()
		{
			if (_configurations != null)
			{
				_configurations.Dispose();
			}
			if (_sessions != null)
			{
				_sessions.Dispose();
			}
		}
	}
}
