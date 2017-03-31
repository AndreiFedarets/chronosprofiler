using System;
using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Storage
{
	public interface IStorage
	{
		IList<ConfigurationInfo> GetConfigurations();

		IList<SessionInfo> GetSessions(Guid configurationToken);

		void SaveConfiguration(ConfigurationInfo configuration);

		void RemoveConfiguration(Guid token);

		ISessionStorage CreateSessionStorage(Guid sessionToken);
	}
}
