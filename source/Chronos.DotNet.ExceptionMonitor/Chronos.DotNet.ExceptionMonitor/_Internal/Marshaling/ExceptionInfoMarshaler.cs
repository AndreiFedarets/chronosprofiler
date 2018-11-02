using System.IO;
using Chronos.Marshaling;
using Chronos.Model;

namespace Chronos.DotNet.ExceptionMonitor.Marshaling
{
    internal class ExceptionInfoMarshaler : NativeUnitMarshaler<ExceptionNativeInfo>
    {
        protected override void MarshalInternal(ExceptionNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override ExceptionNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(ExceptionNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<ExceptionNativeInfo>.Marshal(value, stream);
            UInt64Marshaler.Marshal(value.ClassId, stream);
            StringMarshaler.Marshal(value.Message, stream);
            MarshalingManager.Marshal(value.Stack, stream);
        }

        public new static ExceptionNativeInfo Demarshal(Stream stream)
        {
            ExceptionNativeInfo unit = NativeUnitMarshaler<ExceptionNativeInfo>.Demarshal(stream);
            unit.ClassId = UInt64Marshaler.Demarshal(stream);
            unit.Message = StringMarshaler.Demarshal(stream);
            unit.Stack = MarshalingManager.Demarshal<ulong[]>(stream);
            return unit;
        }
    }
}
