using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;

namespace Chronos.Proxy
{
    internal sealed class SessionCollection : ProxyBaseObject<ISessionCollection>, ISessionCollection
    {
        private readonly Chronos.Host.IApplication _application;
        private readonly Dictionary<Guid, Session> _collection;
        private readonly RemoteEventRouter<SessionEventArgs> _sessionCreatedEventSink;
        private readonly RemoteEventRouter<SessionEventArgs> _sessionRemovedEventSink;
        private readonly RemoteEventRouter<SessionEventArgs> _sessionStateChangedEventSink;
        private volatile bool _initialized;

        public SessionCollection(ISessionCollection sessions, Chronos.Host.IApplication application)
            : base(sessions)
        {
            _initialized = false;
            _application = application;
            _collection = new Dictionary<Guid, Session>();
            _sessionCreatedEventSink = new RemoteEventRouter<SessionEventArgs>(RemoteObject, "SessionCreated", this, OnRemoteSessionCreated);
            _sessionRemovedEventSink = new RemoteEventRouter<SessionEventArgs>(RemoteObject, "SessionRemoved", this, OnRemoteSessionRemoved, OnRemoteSessionRemovedExecuted);
            _sessionStateChangedEventSink = new RemoteEventRouter<SessionEventArgs>(RemoteObject, "SessionStateChanged", this, OnRemoteSessionStateChanged);
        }

        public ISession this[Guid uid]
        {
            get
            {
                lock (_collection)
                {
                    VerifyDisposed();
                    Session session;
                    if (_collection.TryGetValue(uid, out session))
                    {
                        return session;
                    }
                    ISession remoteSession = Execute(() => RemoteObject[uid]);
                    session = new Session(remoteSession, _application);
                    _collection.Add(uid, session);
                    return session;
                }
            }
        }

        public int Count
        {
            get
            {
                VerifyDisposed();
                if (_initialized)
                {
                    lock (_collection)
                    {
                        return _collection.Count;
                    }
                }
                return Execute(() => RemoteObject.Count);
            }
        }

        public event EventHandler<SessionEventArgs> SessionCreated
        {
            add { _sessionCreatedEventSink.Event += value; }
            remove { _sessionCreatedEventSink.Event -= value; }
        }

        public event EventHandler<SessionEventArgs> SessionRemoved
        {
            add { _sessionRemovedEventSink.Event += value; }
            remove { _sessionRemovedEventSink.Event -= value; }
        }

        public event EventHandler<SessionEventArgs> SessionStateChanged
        {
            add { _sessionStateChangedEventSink.Event += value; }
            remove { _sessionStateChangedEventSink.Event -= value; }
        }

        public bool Contains(Guid uid)
        {
            VerifyDisposed();
            lock (_collection)
            {
                if (_collection.ContainsKey(uid))
                {
                    return true;
                }
            }
            return Execute(() => RemoteObject.Contains(uid));
        }

        public IEnumerator<ISession> GetEnumerator()
        {
            List<ISession> collection;
            lock (_collection)
            {
                VerifyDisposed();
                Initialize();
                collection = new List<ISession>(_collection.Values);
            }
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            lock (_collection)
            {
                VerifyDisposed();
                _sessionCreatedEventSink.Dispose();
                _sessionRemovedEventSink.Dispose();
                _sessionStateChangedEventSink.Dispose();
                foreach (Session session in _collection.Values)
                {
                    session.Dispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            List<ISession> remoteSessions = Execute(() => RemoteObject.ToList());
            if (remoteSessions.Count == _collection.Count)
            {
                return;
            }
            foreach (ISession remoteSession in remoteSessions)
            {
                Session session = new Session(remoteSession, _application);
                if (!_collection.ContainsKey(session.Uid))
                {
                    _collection.Add(session.Uid, session);
                }
            }
            _initialized = true;
        }

        private SessionEventArgs OnRemoteSessionCreated(SessionEventArgs eventArgs)
        {
            Session session;
            ISession remoteSession = eventArgs.Session;
            Guid sessionUid;
            try
            {
                sessionUid = remoteSession.Uid;
            }
            catch(RemotingException remotingException)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, remotingException);
                return null;
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                throw;
            }
            lock (_collection)
            {
                VerifyDisposed();
                if (!_collection.TryGetValue(sessionUid, out session))
                {
                    session = new Session(remoteSession, _application);
                    _collection.Add(sessionUid, session);
                }
            }
            return new SessionEventArgs(session);
        }

        private SessionEventArgs OnRemoteSessionRemoved(SessionEventArgs eventArgs)
        {
            Session session;
            ISession remoteSession = eventArgs.Session;
            Guid sessionUid;
            try
            {
                //TODO: to think how to remote session if we don't know it's uid
                sessionUid = remoteSession.Uid;
            }
            catch (RemotingException remotingException)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, remotingException);
                return null;
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                throw;
            }
            lock (_collection)
            {
                VerifyDisposed();
                if (_collection.TryGetValue(sessionUid, out session))
                {
                    _collection.Remove(sessionUid);
                }
            }
            if (session == null)
            {
                session = new Session(remoteSession, _application);
            }
            return new SessionEventArgs(session);
        }


        private void OnRemoteSessionRemovedExecuted(SessionEventArgs eventArgs)
        {
            eventArgs.Session.TryDispose();
        }

        private SessionEventArgs OnRemoteSessionStateChanged(SessionEventArgs eventArgs)
        {
            Session session;
            ISession remoteSession = eventArgs.Session;
            Guid sessionUid;
            try
            {
                sessionUid = remoteSession.Uid;
            }
            catch (RemotingException remotingException)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, remotingException);
                return null;
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                throw;
            }
            lock (_collection)
            {
                VerifyDisposed();
                if (!_collection.TryGetValue(sessionUid, out session))
                {
                    session = new Session(remoteSession, _application);
                    _collection.Add(sessionUid, session);
                }
            }
            return new SessionEventArgs(session);
        }
    }
}
