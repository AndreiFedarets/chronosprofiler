using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chronos
{
    internal sealed class SessionCollection : RemoteBaseObject, ISessionCollection
    {
        private readonly Host.IApplication _application;
        private readonly Dictionary<Guid, Session> _collection;
        private readonly RemoteEventHandler<SessionEventArgs> _sessionCreatedEvent;
        private readonly RemoteEventHandler<SessionEventArgs> _sessionRemovedEvent;
        private readonly RemoteEventHandler<SessionEventArgs> _sessionStateChangedEvent;

        public SessionCollection(Host.IApplication application)
        {
            _application = application;
            _collection = new Dictionary<Guid, Session>();
            _sessionCreatedEvent = new RemoteEventHandler<SessionEventArgs>(this);
            _sessionRemovedEvent = new RemoteEventHandler<SessionEventArgs>(this);
            _sessionStateChangedEvent = new RemoteEventHandler<SessionEventArgs>(this);
        }

        public ISession this[Guid sessionUid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    Session session;
                    if (!_collection.TryGetValue(sessionUid, out session))
                    {
                        throw new SessionNotFoundException(sessionUid);
                    }
                    return session;
                }
            }
        }
        
        public int Count
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    return _collection.Count;
                }
            }
        }

        public event EventHandler<SessionEventArgs> SessionCreated
        {
            add { _sessionCreatedEvent.Add(value); }
            remove { _sessionCreatedEvent.Remove(value); }
        }

        public event EventHandler<SessionEventArgs> SessionRemoved
        {
            add { _sessionRemovedEvent.Add(value); }
            remove { _sessionRemovedEvent.Remove(value); }
        }

        public event EventHandler<SessionEventArgs> SessionStateChanged
        {
            add { _sessionStateChangedEvent.Add(value); }
            remove { _sessionStateChangedEvent.Remove(value); }
        }

        internal ISession Create(IConfiguration configuration)
        {
            lock (Lock)
            {
                VerifyDisposed();
                ConfigurationSettings configurationSettings = configuration.ConfigurationSettings;
                Session session = new Session(configurationSettings, this, _application);
                _collection.Add(session.Uid, session);
                _sessionCreatedEvent.Invoke(() => new SessionEventArgs(session));
                return session;
            }
        }

        public bool Contains(Guid sessionToken)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return _collection.ContainsKey(sessionToken);
            }
        }

        public IEnumerator<ISession> GetEnumerator()
        {
            List<ISession> sessions;
            lock (Lock)
            {
                VerifyDisposed();
                sessions = new List<ISession>(_collection.Values);
            }
            return sessions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                foreach (Session session in _collection.Values)
                {
                    session.Dispose();
                }
                _collection.Clear();
                _sessionCreatedEvent.Dispose();
                _sessionRemovedEvent.Dispose();
                _sessionStateChangedEvent.Dispose();
                base.Dispose();
            }
        }

        internal void OnSessionStateChanged(ISession session)
        {
            _sessionStateChangedEvent.Invoke(() => new SessionEventArgs(session));
        }

        internal void OnSessionRemovedChanged(ISession session)
        {
            bool removed;
            lock (Lock)
            {
                VerifyDisposed();
                removed = _collection.Remove(session.Uid);
            }
            if (removed)
            {
                _sessionRemovedEvent.Invoke(() => new SessionEventArgs(session));
            }
            else
            {
                LoggingProvider.Current.Log(TraceEventType.Warning, ErrorMessageFormatter.SessionAlreadyRemovedFromCollection(session.Uid));
            }
        }
    }
}
