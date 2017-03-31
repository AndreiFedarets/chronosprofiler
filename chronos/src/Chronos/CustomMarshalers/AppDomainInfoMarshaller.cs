using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class AppDomainInfoMarshaller : UnitBaseMarshaler<AppDomainInfo>
	{
		protected override void MarshalSpecificPart(AppDomainInfo value, Stream stream)
		{
			UInt32Marshaler.Marshal((uint)value.LoadResult, stream);
		}

		protected override void DemarshalSpecificPart(AppDomainInfo value, Stream stream)
		{
			value.LoadResult = (Result)UInt32Marshaler.Demarshal(stream);
		}
	}
}
