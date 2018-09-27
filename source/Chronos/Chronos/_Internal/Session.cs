using System;

namespace Chronos
{
    internal sealed class Session : RemoteBaseObject, ISession
    {
        private readonly Guid _uid;
        private readonly Host.IApplication _hostApplication;
        private readonly SessionCollection _sessions;
        private readonly ConfigurationSettings _configurationSettings;
        private readonly Daemon.WeakApplication _daemonApplication;

        public Session(ConfigurationSettings configurationSettings, SessionCollection sessions, Host.IApplication application)
            : this(Guid.NewGuid(), configurationSettings, sessions, application)
        {
        }

        public Session(Guid sessionUid, ConfigurationSettings configurationSettings, SessionCollection sessions, Host.IApplication application)
        {
            _uid = sessionUid;
            _configurationSettings = configurationSettings;
            _sessions = sessions;
            _hostApplication = application;
            _daemonApplication = new Daemon.WeakApplication(sessionUid);
            _daemonApplication.SessionStateChanged += OnSessionStateChanged;
            //_daemonApplication.ApplicationStateChanged += OnApplicationStateChanged;
        }

        public Guid Uid
        {
            get
            {
                VerifyDisposed();
                return _uid;
            }
        }

        public IProfilingTimer ProfilingTimer
        {
            get { return _daemonApplication.ProfilingTimer; }
        }

        public Guid ConfigurationUid
        {
            get
            {
                VerifyDisposed();
                return _configurationSettings.ConfigurationUid;
            }
        }

        public SessionState State
        {
            get
            {
                VerifyDisposed();
                return _daemonApplication.SessionState;
            }
        }

        public bool IsActive
        {
            get
            {
                SessionState state = State;
                return state == SessionState.Profiling || state == SessionState.Decoding;
            }
        }

        public bool SaveOnClose
        {
            get
            {
                VerifyDisposed();
                return _daemonApplication.SaveOnClose;
            } 
            set
            {
                VerifyDisposed();
                _daemonApplication.SaveOnClose = value;
            }
        }

        public IServiceContainer ServiceContainer
        {
            get
            {
                VerifyDisposed();
                return _daemonApplication.ServiceContainer;
            }
        }

        public Host.IApplication Application
        {
            get
            {
                VerifyDisposed();
                return _hostApplication;
            }
        }

        public event EventHandler<SessionStateEventArgs> SessionStateChanged
        {
            add { _daemonApplication.SessionStateChanged += value; }
            remove { _daemonApplication.SessionStateChanged -= value; }
        }

        public ConfigurationSettings GetConfigurationSettings()
        {
            VerifyDisposed();
            return _configurationSettings.Clone();
        }

        public ProcessInformation GetProfiledProcessInformation()
        {
            VerifyDisposed();
            return _daemonApplication.GetProcessInformation();
        }

        internal void StartProfiling(int profiledProcessId, Guid agentApplicationUid, uint profilingBeginTime)
        {
            VerifyDisposed();
            _daemonApplication.StartProfiling(profiledProcessId, agentApplicationUid, profilingBeginTime);
        }

        public void StartDecoding(ILifetimeSponsor sponsor)
        {
            VerifyDisposed();
            _daemonApplication.StartDecoding(sponsor);
        }

        public void StopProfiling()
        {
            VerifyDisposed();
            _daemonApplication.StopProfiling();
        }

        public void CloseSession()
        {
            VerifyDisposed();
            _daemonApplication.Close();
        }

        public void SaveSession()
        {
            VerifyDisposed();
            _daemonApplication.SaveSession();
        }

        public void RemoveSession()
        {
            VerifyDisposed();
            _daemonApplication.RemoveSession();
        }

        public void FlushData()
        {
            VerifyDisposed();
            _daemonApplication.ReloadData();
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _daemonApplication.Close();
                _daemonApplication.SessionStateChanged -= OnSessionStateChanged;
                //_daemonApplication.ApplicationStateChanged -= OnApplicationStateChanged;
                _daemonApplication.Dispose();
                base.Dispose();
            }
        }

        //private void OnApplicationStateChanged(object sender, ApplicationStateEventArgs e)
        //{
        //    //Application is closed, so 
        //    if (e.CurrentState == ApplicationState.Closed)
        //    {
        //        _sessions.OnSessionStateChanged(this);
        //    }
        //}

        private void OnSessionStateChanged(object sender, SessionStateEventArgs e)
        {
            VerifyDisposed();
            _sessions.OnSessionStateChanged(this);
        }
    }
}
