using System.Collections.Generic;
using System.Xml.Serialization;

namespace Chronos.Configuration
{
	[XmlRoot("extensions")]
	public class ExtensionDescriptionCollection : List<ExtensionDescription>
	{
	}
}
