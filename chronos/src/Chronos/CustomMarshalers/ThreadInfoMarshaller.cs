using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class ThreadInfoMarshaller : UnitBaseMarshaler<ThreadInfo>
	{
		protected override void MarshalSpecificPart(ThreadInfo value, Stream stream)
		{
			Int32Marshaler.Marshal(value.OSThreadId, stream);
		}

		protected override void DemarshalSpecificPart(ThreadInfo value, Stream stream)
		{
			value.OSThreadId = Int32Marshaler.Demarshal(stream);
		}
	}
}
