using System;
using Chronos.Configuration;
using Chronos.Core;
using Chronos.Daemon;

namespace Chronos.Host.Internal
{
	internal class Session : SingletonMarshalByRefObject, ISession
	{
		private readonly SessionCollection _sessions;
		private readonly ICommonConfiguration _commonConfiguration;
		private readonly ProcessInfo _agentProcessInfo;
		private IDaemonApplication _daemonApplication;

		public Session(ActivationSettings activationSettings, ConfigurationSettings configurationSettings, Guid sessionToken, ProcessInfo agentProcessInfo, ICommonConfiguration commonConfiguration, SessionCollection sessions)
		{
			Token = sessionToken;
			IsSaved = agentProcessInfo != null;
			_commonConfiguration = commonConfiguration;
			ActivationSettings = activationSettings;
			ConfigurationSettings = configurationSettings;
			_agentProcessInfo = agentProcessInfo;
			_sessions = sessions;
		}

		public Session(ActivationSettings activationSettings, ConfigurationSettings configurationSettings, Guid sessionToken, ICommonConfiguration commonConfiguration, SessionCollection sessions)
			: this(activationSettings, configurationSettings, sessionToken, null, commonConfiguration, sessions)
		{
		}

		public bool IsActive
		{
			get
			{
				SessionState state = State;
				return state == SessionState.Profiling || state == SessionState.Paused;
			}
		}

		public bool IsSaved { get; private set; }

		public SessionState State
		{
			get
			{
 				if (!IsLaunched)
 				{
 					return SessionState.Closed;
 				}
				return _daemonApplication.State;
			}
		}

		public ProcessInfo ProcessInfo
		{
			get
			{
				if (_agentProcessInfo == null)
				{
					SessionState sessionState = GetSessionState();
					if (sessionState == SessionState.Closed)
					{
						return ProcessInfo.Empty;
					}
					return _daemonApplication.GetProcessInfo();
				}
				return _agentProcessInfo;
			}
		}

		public ActivationSettings ActivationSettings { get; private set; }

		public ConfigurationSettings ConfigurationSettings { get; private set; }

		public Guid Token { get; private set; }

		public bool IsLaunched
		{
			get { return _daemonApplication != null; }
		}

        public void StartProfiling(int processId, uint syncTime)
		{
			LaunchSession();
            _daemonApplication.StartProfiling(processId, syncTime);
		}

		public IDaemonApplication StartDecoding()
		{
			if (State == SessionState.Closed)
			{
				LaunchSession();
				_daemonApplication.StartDecoding();
			}
			return _daemonApplication;
		}

		private void LaunchSession()
		{
			if (IsLaunched)
			{
				throw new InvalidOperationException();
			}
			if (!DaemonProvider.CheckConnection(ConfigurationSettings.Token, Token))
			{
				DaemonProvider.Run(_commonConfiguration, ConfigurationSettings.Token, Token);
			}
			_daemonApplication = DaemonProvider.Connect(ConfigurationSettings.Token, Token);
			_daemonApplication.StateChanged += OnDaemonApplicationStateChanged;
			_daemonApplication.Exited += OnDaemonProcessExited;
		}

		private void OnDaemonProcessExited()
		{
			_sessions.NotifySessionStateChanged(this);
			_daemonApplication = null;
		}

		private void OnDaemonApplicationStateChanged()
		{
			_sessions.NotifySessionStateChanged(this);
		}

		private SessionState GetSessionState()
		{
			if (!IsLaunched)
			{
				return SessionState.Closed;
			}
			return _daemonApplication.State;
		}

		public void Close(bool save)
		{
			if (IsLaunched)
			{
				_daemonApplication.StateChanged -= OnDaemonApplicationStateChanged;
				_daemonApplication.Close(save);
			}
		}

		public void Close()
		{
			Close(false);
		}

		public void Remove()
		{
			Close(false);
			if (IsSaved)
			{
				//remove data;
				_sessions.NotifySessionRemovedChanged(this);
			}
		}

		public void Dispose()
		{
			Close(false);
		}

        public void StopProfiling()
        {
            if (State == SessionState.Paused || State == SessionState.Profiling)
            {
                _daemonApplication.StopProfiling();
            }
        }
    }
}
