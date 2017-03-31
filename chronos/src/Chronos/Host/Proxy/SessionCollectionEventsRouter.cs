using System;

namespace Chronos.Host.Proxy
{
	public class SessionCollectionEventsRouter : MarshalByRefObject, IDisposable
	{
		private readonly ISessionCollection _sessions;
		private readonly Action<ISession> _sessionCreated;
		private readonly Action<ISession> _sessionRemoved;
		private readonly Action<ISession> _sessionStateChanged;

		public SessionCollectionEventsRouter(ISessionCollection sessions, Action<ISession> sessionCreated,
			Action<ISession> sessionRemoved, Action<ISession> sessionStateChanged)
		{
			_sessions = sessions;
			_sessionCreated = sessionCreated;
			_sessionRemoved = sessionRemoved;
			_sessionStateChanged = sessionStateChanged;
			_sessions.SessionCreated += OnSessionCreated;
			_sessions.SessionRemoved += OnSessionRemoved;
			_sessions.SessionStateChanged += OnSessionStateChanged;
		}

		public void Dispose()
		{
			_sessions.SessionCreated -= OnSessionCreated;
			_sessions.SessionRemoved -= OnSessionRemoved;
			_sessions.SessionStateChanged -= OnSessionStateChanged;
		}

		public void OnSessionCreated(ISession session)
		{
			_sessionCreated.BeginInvoke(session, null, null);
		}

		public void OnSessionRemoved(ISession session)
		{
			_sessionRemoved.BeginInvoke(session, null, null);
		}

		public void OnSessionStateChanged(ISession session)
		{
			_sessionStateChanged.BeginInvoke(session, null, null);
		}
	}
}
