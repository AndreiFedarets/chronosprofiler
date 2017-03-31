using System.Xml.Serialization;

namespace Chronos.Configuration
{
	[XmlRoot("agent")]
	public class AgentConfiguration
	{
		[XmlElement("entryPoint32")]
		public string EntryPoint32 { get; set; }

		[XmlElement("binaryPath")]
		public string BinaryPath { get; set; }

		[XmlElement("entryPoint64")]
		public string EntryPoint64 { get; set; }
	}
}
