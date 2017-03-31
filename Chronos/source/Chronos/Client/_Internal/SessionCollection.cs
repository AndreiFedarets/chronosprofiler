using System;

namespace Chronos.Client
{
    internal sealed class SessionCollection : DictionaryChangedBase<Guid, ISession>, ISessionCollection
    {
        private readonly Host.IApplicationCollection _hostApplications;
        private Delegate _sessionCreated;
        private Delegate _sessionRemoved;
        private Delegate _sessionStateChanged;

        public SessionCollection(Host.IApplicationCollection hostApplications)
        {
            _hostApplications = hostApplications;
            InitializeCollection();
        }

        public ISession this[Guid sessionUid]
        {
            get
            {
                lock (Lock)
                {
                    ISession session;
                    if (!TryGetValue(sessionUid, out session))
                    {
                        throw new SessionNotFoundException(sessionUid);
                    }
                    return session;
                }
            }
        }

        public event EventHandler<SessionEventArgs> SessionCreated
        {
            add
            {
                VerifyDisposed();
                _sessionCreated = Delegate.Combine(_sessionCreated, value);
            }
            remove
            {
                VerifyDisposed();
                _sessionCreated = Delegate.Remove(_sessionCreated, value);
            }
        }

        public event EventHandler<SessionEventArgs> SessionRemoved
        {
            add
            {
                VerifyDisposed();
                _sessionRemoved = Delegate.Combine(_sessionRemoved, value);
            }
            remove
            {
                VerifyDisposed();
                _sessionRemoved = Delegate.Remove(_sessionRemoved, value);
            }
        }

        public event EventHandler<SessionEventArgs> SessionStateChanged
        {
            add
            {
                VerifyDisposed();
                _sessionStateChanged = Delegate.Combine(_sessionStateChanged, value);
            }
            remove
            {
                VerifyDisposed();
                _sessionStateChanged = Delegate.Remove(_sessionStateChanged, value);
            }
        }

        public bool Contains(Guid sessionToken)
        {
            lock (Lock)
            {
                return ContainsKey(sessionToken);
            }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _hostApplications.ApplicationConnected -= OnHostApplicationConnected;
                _hostApplications.ApplicationDisconnected -= OnHostApplicationDisconnected;
                foreach (Host.IApplication application in _hostApplications)
                {
                    DisposeApplication(application);
                }
                base.Dispose();
            }
        }

        private void InitializeCollection()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _hostApplications.ApplicationConnected += OnHostApplicationConnected;
                _hostApplications.ApplicationDisconnected += OnHostApplicationDisconnected;
                foreach (Host.IApplication hostApplication in _hostApplications)
                {
                    InitializeApplication(hostApplication);
                }
            }
        }

        private void OnHostApplicationConnected(object sender, Host.ApplicationEventArgs e)
        {
            Host.IApplication hostApplication = e.Application;
            lock (Lock)
            {
                VerifyDisposed();
                InitializeApplication(hostApplication);
            }
        }

        private void InitializeApplication(Host.IApplication hostApplication)
        {
            ISessionCollection hostSessions = hostApplication.Sessions;
            hostSessions.SessionCreated += OnHostSessionCreated;
            hostSessions.SessionRemoved += OnHostSessionRemoved;
            hostSessions.SessionStateChanged += OnHostSessionStateChanged;
            foreach (ISession hostSession in hostSessions)
            {
                Add(hostSession.Uid, hostSession);
            }
        }

        private void OnHostApplicationDisconnected(object sender, Host.ApplicationEventArgs e)
        {
            Host.IApplication hostApplication = e.Application;
            lock (Lock)
            {
                VerifyDisposed();
                DisposeApplication(hostApplication);
            }
        }

        private void DisposeApplication(Host.IApplication hostApplication)
        {
            ISessionCollection hostSessions = hostApplication.Sessions;
            hostSessions.SessionCreated -= OnHostSessionCreated;
            hostSessions.SessionRemoved -= OnHostSessionRemoved;
            hostSessions.SessionStateChanged -= OnHostSessionStateChanged;
            foreach (ISession session in this)
            {
                if (session.Application == hostApplication)
                {
                    Remove(session.Uid);
                }
            }
        }

        private void OnHostSessionCreated(object sender, SessionEventArgs e)
        {
            ISession hostSession = e.Session;
            lock (Lock)
            {
                VerifyDisposed();
                if (!ContainsKey(hostSession.Uid))
                {
                    Add(hostSession.Uid, hostSession);
                }
            }
            SessionEventArgs.RaiseEvent((EventHandler<SessionEventArgs>)_sessionCreated, this, hostSession);
            //DispatcherHolder.BeginInvoke(action);
        }

        private void OnHostSessionRemoved(object sender, SessionEventArgs e)
        {
            ISession hostSession = e.Session;
            bool configurationRemoved;
            lock (Lock)
            {
                VerifyDisposed();
                configurationRemoved = Remove(hostSession.Uid);
            }
            if (configurationRemoved)
            {
                SessionEventArgs.RaiseEvent((EventHandler<SessionEventArgs>)_sessionRemoved, this, hostSession);
                //DispatcherHolder.BeginInvoke(action);
            }
        }

        public void OnHostSessionStateChanged(object sender, SessionEventArgs e)
        {
            ISession session;
            ISession hostSession = e.Session;
            lock (Lock)
            {
                session = GetItem(hostSession.Uid);
            }
            SessionEventArgs.RaiseEvent((EventHandler<SessionEventArgs>)_sessionStateChanged, this, session);
            //DispatcherHolder.BeginInvoke(action);
        }
    }
}
