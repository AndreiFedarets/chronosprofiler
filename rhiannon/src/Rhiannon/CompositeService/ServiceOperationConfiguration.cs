using System;
using System.Xml.Serialization;

namespace Rhiannon.CompositeService
{
	[Serializable]
	[XmlRoot("operation")]
	public class ServiceOperationConfiguration
	{
		public ServiceOperationConfiguration()
		{
			OperationOrder = int.MaxValue;
		}

		[XmlAttribute("type")]
		public Type OperationType { get; set; }

		[XmlAttribute("order")]
		public int OperationOrder { get; set; }

	}
}
