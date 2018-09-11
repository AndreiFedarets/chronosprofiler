using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using Chronos.Communication.Managed;

namespace Chronos.Daemon
{
    internal sealed class WeakApplication : RemoteBaseObject, IApplication
    {
        private volatile IApplication _application;
        private readonly Guid _daemonApplicationUid;
        private bool _saveOnClose;

        public WeakApplication(Guid daemonApplicationUid)
        {
            _daemonApplicationUid = daemonApplicationUid;
        }

        public EnvironmentInformation EnvironmentInformation
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.EnvironmentInformation;
                }
                return null;
            }
        }

        public Guid Uid
        {
            get { return _daemonApplicationUid; }
        }


        public IProfilingTimer ProfilingTimer
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.ProfilingTimer;
                }
                return null;
            }
        }

        public bool SaveOnClose
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.SaveOnClose;
                }
                return _saveOnClose;
            }
            set
            {
                IApplication application;
                _saveOnClose = value;
                if (VerifyDaemonLaunched(false, out application))
                {
                    application.SaveOnClose = _saveOnClose;
                }
            }
        }

        public ApplicationState ApplicationState
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.ApplicationState;
                }
                return ApplicationState.Closed;
            }
        }

        public TimeSpan StartupTime
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.StartupTime;
                }
                return TimeSpan.MinValue;
            }
        }

        public SessionState SessionState
        {
            get
            {
                SessionState sessionState = SessionState.Closed;
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    try
                    {
                        sessionState = application.SessionState;
                    }
                    catch (RemotingException remotingException)
                    {
                        sessionState = SessionState.Closed;
                        LoggingProvider.Current.Log(TraceEventType.Warning, remotingException);
                    }
                }
                return sessionState;
            }
        }

        public IServiceContainer ServiceContainer
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.ServiceContainer;
                }
                return null;
            }
        }
        
        public Communication.Native.IRequestClient AgentClient
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.AgentClient;
                }
                return null;
            }
        }

        public ProcessInformation ProfiledProcess
        {
            get
            {
                IApplication application;
                if (VerifyDaemonLaunched(false, out application))
                {
                    return application.ProfiledProcess;
                }
                return null;
            }
        }

        public event EventHandler<ApplicationStateEventArgs> ApplicationStateChanged;

        public event EventHandler<SessionStateEventArgs> SessionStateChanged;

        //public event EventHandler<ApplicationStateEventArgs> ApplicationStateChanged;

        public ProcessInformation GetProcessInformation()
        {
            IApplication application;
            if (VerifyDaemonLaunched(false, out application))
            {
                return application.ProfiledProcess;
            }
            return null;
        }

        public void StartProfiling(int profiledProcessId, Guid agentApplicationUid, uint profilingBeginTime)
        {
            IApplication application;
            VerifyDaemonLaunched(true, out application);
            application.StartProfiling(profiledProcessId, agentApplicationUid, profilingBeginTime);
        }

        public void StartDecoding(ILifetimeSponsor sponsor)
        {
            IApplication application;
            VerifyDaemonLaunched(true, out application);
            application.StartDecoding(sponsor);
        }

        public void StopProfiling()
        {
            IApplication application;
            if (VerifyDaemonLaunched(false, out application))
            {
                application.StopProfiling();
            }
        }

        public void Close()
        {
            IApplication application;
            if (VerifyDaemonLaunched(false, out application))
            {
                application.SessionStateChanged -= OnSessionStateChanged;
                application.Close();
            }
        }

        public void SaveSession()
        {
            IApplication application;
            if (VerifyDaemonLaunched(false, out application))
            {
                application.SaveSession();
            }
        }

        public void RemoveSession()
        {
            IApplication application;
            if (VerifyDaemonLaunched(false, out application))
            {
                application.RemoveSession();
            }
        }

        public void ReloadData()
        {
            IApplication application;
            if (VerifyDaemonLaunched(false, out application))
            {
                if (SessionState == SessionState.Profiling || SessionState == SessionState.Decoding)
                {
                    application.ReloadData();
                }
            }
        }

        public void Run()
        {
            IApplication application;
            VerifyDaemonLaunched(true, out application);
        }

        public string Ping(string message)
        {
            IApplication application;
            if (VerifyDaemonLaunched(false, out application))
            {
                return application.Ping(message);
            }
            return message == null ? GetType().ToString() : message + GetType();
        }

        private bool VerifyDaemonLaunched(bool launchIfNotLaunched, out IApplication application)
        {
            lock (Lock)
            {
                if (_application != null)
                {
                    try
                    {
                        _application.Ping(string.Empty);
                        application = _application;
                        return true;
                    }
                    catch (RemoteApplicationUnavailableException remotingException)
                    {
                        _application = null;
                        LoggingProvider.Current.Log(TraceEventType.Information, remotingException);
                    }
                }
                if (launchIfNotLaunched)
                {
                    ConnectionSettings connectionSettings = ApplicationManager.CreateLocalConnectionSettings(_daemonApplicationUid);
                    if (!ApplicationManager.CheckConnection(connectionSettings))
                    {
                        ApplicationManager.Run(_daemonApplicationUid);
                    }
                    _application = ApplicationManager.Connect(connectionSettings);
                    _application.SessionStateChanged += OnSessionStateChanged;
                    _application.SaveOnClose = _saveOnClose;
                    //_application.ApplicationStateChanged += OnDaemonApplicationStateChanged;
                    application = _application;
                    return true;
                }
                application = _application;
                return false;
            }
        }

        public void OnSessionStateChanged(object sender, SessionStateEventArgs e)
        {
            SessionStateEventArgs.RaiseEvent(SessionStateChanged, _application, e.PreviousState, e.CurrentState);
        }

        //public void OnDaemonApplicationStateChanged(object sender, ApplicationStateEventArgs e)
        //{
        //    try
        //    {
        //        ApplicationStateEventArgs.RaiseEvent(ApplicationStateChanged, _application, e.PreviousState, e.CurrentState);
        //    }
        //    finally
        //    {
        //        if (e.CurrentState == ApplicationState.Closed)
        //        {
        //            _application.SessionStateChanged += OnSessionStateChanged;
        //            _application.ApplicationStateChanged += OnDaemonApplicationStateChanged;
        //        }
        //    }
        //}

        public override void Dispose()
        {
            _application.TryDispose();
            base.Dispose();
        }
    }
}
