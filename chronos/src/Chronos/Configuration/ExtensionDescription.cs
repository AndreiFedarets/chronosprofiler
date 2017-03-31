using System;
using System.Xml.Serialization;

namespace Chronos.Configuration
{
	[XmlRoot("extension")]
	[Serializable]
	public class ExtensionDescription
	{
		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("name")]
		public string DisplayName { get; set; }

		[XmlAttribute("subpath")]
		public string SubPath { get; set; }
	}
}
