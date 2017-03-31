using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
    public class ExceptionInfoMarshaller : UnitBaseMarshaler<ExceptionInfo>
    {
        protected override void MarshalSpecificPart(ExceptionInfo value, Stream stream)
        {
            UInt64Marshaler.Marshal(value.ThreadId, stream);
            BoolMarshaler.Marshal(value.IsCatched, stream);
            MarshalingManager.Marshal(value.Stack, stream);
        }

        protected override void DemarshalSpecificPart(ExceptionInfo value, Stream stream)
        {
            value.ThreadId = UInt64Marshaler.Demarshal(stream);
            value.IsCatched = BoolMarshaler.Demarshal(stream);
            value.Message = StringMarshaler.Demarshal(stream);
            value.Stack = MarshalingManager.Demarshal<uint[]>(stream);
        }
    }
}
