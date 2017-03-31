using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class FunctionInfoMarshaller : UnitBaseMarshaler<FunctionInfo>
	{
		protected override void MarshalSpecificPart(FunctionInfo value, Stream stream)
		{
			UInt64Marshaler.Marshal(value.ClassManangedId, stream);
			UInt32Marshaler.Marshal(value.Hits, stream);
			UInt32Marshaler.Marshal(value.TotalTime, stream);
		}

		protected override void DemarshalSpecificPart(FunctionInfo value, Stream stream)
		{
			value.ClassManangedId = UInt64Marshaler.Demarshal(stream);
			value.Hits = UInt32Marshaler.Demarshal(stream);
			value.TotalTime = UInt32Marshaler.Demarshal(stream);
		}
	}
}
