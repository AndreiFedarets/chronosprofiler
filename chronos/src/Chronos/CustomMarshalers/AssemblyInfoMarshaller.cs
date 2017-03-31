using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class AssemblyInfoMarshaller : UnitBaseMarshaler<AssemblyInfo>
	{
		protected override void MarshalSpecificPart(AssemblyInfo value, Stream stream)
		{
			UInt32Marshaler.Marshal((uint)value.LoadResult, stream);
			UInt64Marshaler.Marshal(value.AppDomainManagedId, stream);
		}

		protected override void DemarshalSpecificPart(AssemblyInfo value, Stream stream)
		{
			value.LoadResult = (Result)UInt32Marshaler.Demarshal(stream);
			value.AppDomainManagedId = UInt64Marshaler.Demarshal(stream);
		}
	}
}
