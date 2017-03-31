using System;
using System.Collections.Generic;
using Chronos.Communication.Remoting;
using Chronos.Configuration;

namespace Chronos.Host.Internal
{
	internal class SessionCollection : SingletonMarshalByRefObject, ISessionCollection
	{
		private readonly ICommonConfiguration _commonConfiguration;
		private readonly IDictionary<Guid, ISession> _sessions;
		private readonly EventActionsHolder<ISession> _sessionCreatedEvent;
		private readonly EventActionsHolder<ISession> _sessionRemovedEvent;
		private readonly EventActionsHolder<ISession> _sessionStateChangedEvent;

		public SessionCollection(ICommonConfiguration commonConfiguration)
		{
			_sessions = new Dictionary<Guid, ISession>();
			_commonConfiguration = commonConfiguration;
			_sessionCreatedEvent = new EventActionsHolder<ISession>();
			_sessionRemovedEvent = new EventActionsHolder<ISession>();
			_sessionStateChangedEvent = new EventActionsHolder<ISession>();
		}

		public ISession this[Guid sessionToken]
		{
			get
			{
				ISession controller;
				_sessions.TryGetValue(sessionToken, out controller);
				return controller;
			}
		}

		public event Action<ISession> SessionCreated
		{
			add { _sessionCreatedEvent.Add(value); }
			remove { _sessionCreatedEvent.Remove(value); }
		}

		public event Action<ISession> SessionRemoved
		{
			add { _sessionRemovedEvent.Add(value); }
			remove { _sessionRemovedEvent.Remove(value); }
		}

		public event Action<ISession> SessionStateChanged
		{
			add { _sessionStateChangedEvent.Add(value); }
			remove { _sessionStateChangedEvent.Remove(value); }
		}

		public ISession Create(IConfiguration configuration)
		{
			Guid sessionToken = Guid.NewGuid();
			return Create(sessionToken, configuration);
		}

		public ISession Create(Guid sessionToken, IConfiguration configuration)
		{
			Session session = new Session(configuration.ActivationSettings, configuration.ConfigurationSettings, sessionToken, _commonConfiguration, this);
			_sessions.Add(session.Token, session);
			_sessionCreatedEvent.Invoke(session);
			return session;
		}

		public bool Contains(Guid sessionToken)
		{
			return _sessions.ContainsKey(sessionToken);
		}

		public IEnumerator<ISession> GetEnumerator()
		{
			return _sessions.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			foreach (ISession session in this)
			{
				session.Dispose();
			}
			_sessions.Clear();
		}

		public void NotifySessionStateChanged(ISession session)
		{
			_sessionStateChangedEvent.Invoke(session);
		}

		public void NotifySessionRemovedChanged(ISession session)
		{
			_sessions.Remove(session.Token);
			_sessionRemovedEvent.Invoke(session);
		}
	}
}
