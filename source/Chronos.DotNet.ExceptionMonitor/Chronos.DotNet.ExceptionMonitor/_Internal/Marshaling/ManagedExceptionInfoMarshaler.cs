using System.IO;
using Chronos.Marshaling;
using Chronos.Model;

namespace Chronos.DotNet.ExceptionMonitor.Marshaling
{
    internal class ManagedExceptionInfoMarshaler : NativeUnitMarshaler<ManagedExceptionNativeInfo>
    {
        protected override void MarshalInternal(ManagedExceptionNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override ManagedExceptionNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(ManagedExceptionNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<ManagedExceptionNativeInfo>.Marshal(value, stream);
            UInt64Marshaler.Marshal(value.ClassId, stream);
            StringMarshaler.Marshal(value.Message, stream);
            MarshalingManager.Marshal(value.Stack, stream);
        }

        public new static ManagedExceptionNativeInfo Demarshal(Stream stream)
        {
            ManagedExceptionNativeInfo unit = NativeUnitMarshaler<ManagedExceptionNativeInfo>.Demarshal(stream);
            unit.ClassId = UInt64Marshaler.Demarshal(stream);
            unit.Message = StringMarshaler.Demarshal(stream);
            unit.Stack = MarshalingManager.Demarshal<ulong[]>(stream);
            return unit;
        }
    }
}
