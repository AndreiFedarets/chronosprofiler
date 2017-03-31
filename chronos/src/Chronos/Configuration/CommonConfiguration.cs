using System.Xml.Serialization;

namespace Chronos.Configuration
{
	[XmlRoot("common")]
	public class CommonConfiguration : ICommonConfiguration
	{
		[XmlElement("daemon")]
		public DaemonConfiguration Daemon { get; set; }

		[XmlElement("host")]
		public HostConfiguration Host { get; set; }

		[XmlElement("agent")]
		public AgentConfiguration Agent { get; set; }

		[XmlElement("winClient")]
		public WinClientConfiguration WinClient { get; set; }

		[XmlElement("webClient")]
		public WebClientConfiguration WebClient { get; set; }
	}
}
