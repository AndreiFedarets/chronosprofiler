using System.Xml.Serialization;

namespace Chronos.Configuration
{
	[XmlRoot("winClient")]
	public class WinClientConfiguration
	{
		[XmlArray("extensions")]
		[XmlArrayItem("extension")]
		public ExtensionDescriptionCollection Extensions { get; set; }
	}
}
