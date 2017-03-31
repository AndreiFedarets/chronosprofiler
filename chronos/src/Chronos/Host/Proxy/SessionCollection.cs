using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Communication.Remoting;
using Rhiannon.Extensions;

namespace Chronos.Host.Proxy
{
	internal class SessionCollection : List<ISession>, ISessionCollection
	{
		private readonly IRemotingExecutor _executor;
		private readonly ISessionCollection _sessions;
		private SessionCollectionEventsRouter _eventsRouter;

		public SessionCollection(ISessionCollection sessions, IRemotingExecutor executor)
		{
			_executor = executor;
			_sessions = sessions;
			_executor.Execute(Initialize);
		}

		public ISession this[Guid sessionToken]
		{
			get
			{
				ISession session = this.FirstOrDefault(x => x.Token == sessionToken);
				return session;
			}
		}

		public event Action<ISession> SessionCreated;

		public event Action<ISession> SessionRemoved;

		public event Action<ISession> SessionStateChanged;

		public bool Contains(Guid sessionToken)
		{
			return this.Any(x => x.Token == sessionToken);
		}

		public void Dispose()
		{
			_executor.Execute(() => _eventsRouter.Dispose());
		}

		private void Initialize()
		{
			_eventsRouter = new SessionCollectionEventsRouter(_sessions, OnSessionCreated, OnSessionRemoved, OnSessionStateChanged);
			foreach (ISession session in _sessions)
			{
				ISession proxy = ProxyFactory.Proxy(session, _executor);
				Add(proxy);
			}
		}

		private void OnSessionCreated(ISession session)
		{
			ISession proxy = ProxyFactory.Proxy(session, _executor);
			Add(proxy);
			SessionCreated.SafeInvoke(proxy);
		}

		private void OnSessionRemoved(ISession session)
		{
			ISession proxy = this[session.Token];
			Remove(proxy);
			SessionRemoved.SafeInvoke(proxy);
		}

		private void OnSessionStateChanged(ISession session)
		{
			Session proxy = (Session)this[session.Token];
			SessionStateChanged.SafeInvoke(proxy);
		}
	}
}
