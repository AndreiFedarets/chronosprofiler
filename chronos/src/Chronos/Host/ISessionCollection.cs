using System;
using System.Collections.Generic;

namespace Chronos.Host
{
	public interface ISessionCollection : IEnumerable<ISession>, IDisposable
	{
		ISession this[Guid sessionToken] { get; }

		event Action<ISession> SessionCreated;

		event Action<ISession> SessionRemoved;

		event Action<ISession> SessionStateChanged;

		bool Contains(Guid sessionToken);
	}
}
