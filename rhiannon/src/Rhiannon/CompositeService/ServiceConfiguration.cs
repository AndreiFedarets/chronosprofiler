using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rhiannon.CompositeService
{
	[Serializable]
	[XmlRoot("service")]
	public class ServiceConfiguration
	{
		[XmlAttribute("type")]
		public Type ServiceType { get; set; }

		[XmlArray("operations")]
		[XmlArrayItem("operation")]
		public List<ServiceOperationConfiguration> Operations { get; set; }
	}
}
