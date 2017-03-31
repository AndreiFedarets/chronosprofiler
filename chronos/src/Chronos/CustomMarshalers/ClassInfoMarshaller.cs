using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class ClassInfoMarshaller : UnitBaseMarshaler<ClassInfo>
	{
		protected override void MarshalSpecificPart(ClassInfo value, Stream stream)
		{
			UInt32Marshaler.Marshal((uint)value.LoadResult, stream);
			UInt64Marshaler.Marshal(value.ModuleManangedId, stream);
		}

		protected override void DemarshalSpecificPart(ClassInfo value, Stream stream)
		{
			value.LoadResult = (Result)UInt32Marshaler.Demarshal(stream);
			value.ModuleManangedId = UInt64Marshaler.Demarshal(stream);
		}
	}
}
