using Chronos.Communication.Remoting;

namespace Chronos.Host.Proxy
{
	internal static class ProxyFactory
	{
		public static ConfigurationCollection Proxy(IConfigurationCollection realObject, IRemotingExecutor executor)
		{
			if (realObject == null)
			{
				return null;
			}
			return new ConfigurationCollection(realObject, executor);
		}

		public static SessionCollection Proxy(ISessionCollection realObject, IRemotingExecutor executor)
		{
			if (realObject == null)
			{
				return null;
			}
			return new SessionCollection(realObject, executor);
		}

		public static Session Proxy(ISession realObject, IRemotingExecutor executor)
		{
			if (realObject == null)
			{
				return null;
			}
			return new Session(realObject, executor);
		}

		public static Configuration Proxy(IConfiguration realObject, IRemotingExecutor executor)
		{
			if (realObject == null)
			{
				return null;
			}
			return new Configuration(realObject, executor);
		}
	}
}
