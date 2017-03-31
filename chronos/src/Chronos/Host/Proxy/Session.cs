using System;
using Chronos.Communication.Remoting;
using Chronos.Core;
using Chronos.Daemon;

namespace Chronos.Host.Proxy
{
	internal class Session : ISession
	{
		private readonly IRemotingExecutor _executor;
		private readonly ISession _session;
		private ConfigurationSettings _configurationSettings;
		private ActivationSettings _activationSettings;
		private Guid _token;

		public Session(ISession session, IRemotingExecutor executor)
		{
			_executor = executor;
			_session = session;
		}

		public Guid Token
		{
			get
			{
				if (_token == Guid.Empty)
				{
					_token = _executor.Execute(() => _session.Token);
				}
				return _token;
			}
		}

		public SessionState State
		{
			get { return _executor.Execute(() => _session.State); }
		}

		public ProcessInfo ProcessInfo
		{
			get { return _executor.Execute(() => _session.ProcessInfo); }
		}

		public bool IsActive
		{
			get { return _executor.Execute(() => _session.IsActive); }
		}

		public bool IsSaved
		{
			get { return _executor.Execute(() => _session.IsSaved); }
		}

		public ConfigurationSettings ConfigurationSettings
		{
			get
			{
				if (_configurationSettings == null)
				{
					_configurationSettings = _executor.Execute(() => _session.ConfigurationSettings);
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
					_activationSettings = _executor.Execute(() => _session.ActivationSettings);
				}
				return _activationSettings;
			}
		}

		public void Close(bool save)
		{
			_executor.Execute(() => _session.Close(save));
		}

		public void Close()
		{
			Close(false);
		}

		public IDaemonApplication StartDecoding()
		{
			_executor.Execute(() => _session.StartDecoding());
			IDaemonApplication daemonApplication = DaemonProvider.Connect(ConfigurationSettings.Token, Token);
			return daemonApplication;
		}

		public void Remove()
		{
			_executor.Execute(() => _session.Remove());
		}

		public void Dispose()
		{
			
		}

        public void StopProfiling()
        {
            _executor.Execute(() => _session.StopProfiling());
        }
    }
}
