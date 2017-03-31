using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class ModuleInfoMarshaller : UnitBaseMarshaler<ModuleInfo>
	{
		protected override void MarshalSpecificPart(ModuleInfo value, Stream stream)
		{
			UInt32Marshaler.Marshal((uint)value.LoadResult, stream);
			UInt64Marshaler.Marshal(value.AssemblyManagedId, stream);
		}

		protected override void DemarshalSpecificPart(ModuleInfo value, Stream stream)
		{
			value.LoadResult = (Result)UInt32Marshaler.Demarshal(stream);
			value.AssemblyManagedId = UInt64Marshaler.Demarshal(stream);
		}
	}
}
