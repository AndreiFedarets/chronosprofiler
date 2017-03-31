using System.Xml.Serialization;

namespace Chronos.Configuration
{
	[XmlRoot("host")]
	public class HostConfiguration
	{
		[XmlElement("entryPoint")]
		public string EntryPoint { get; set; }

		[XmlElement("binaryPath")]
		public string BinaryPath { get; set; }

		[XmlElement("sessionsLocation")]
		public string SessionsLocation { get; set; }

		[XmlArray("extensions")]
		[XmlArrayItem("extension")]
		public ExtensionDescriptionCollection Extensions { get; set; }

		[XmlElement("runtype")]
		public string Runtype { get; set; }
	}
}
