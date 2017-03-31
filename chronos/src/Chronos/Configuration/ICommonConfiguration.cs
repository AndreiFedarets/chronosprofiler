namespace Chronos.Configuration
{
	public interface ICommonConfiguration
	{
		DaemonConfiguration Daemon { get; }

		HostConfiguration Host { get; }

		AgentConfiguration Agent { get; }

		WinClientConfiguration WinClient { get; }

		WebClientConfiguration WebClient { get; }
	}
}
