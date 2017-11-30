using Chronos.Marshaling;
using Chronos.Model;
using System.IO;

namespace Chronos.Java.BasicProfiler.Marshaling
{
    internal class ThreadInfoMarshaler : NativeUnitMarshaler<ThreadNativeInfo>
    {
        protected override void MarshalInternal(ThreadNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override ThreadNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(ThreadNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<ThreadNativeInfo>.Marshal(value, stream);
            UInt32Marshaler.Marshal(value.OsThreadId, stream);
        }

        public new static ThreadNativeInfo Demarshal(Stream stream)
        {
            ThreadNativeInfo unit = NativeUnitMarshaler<ThreadNativeInfo>.Demarshal(stream);
            unit.OsThreadId = UInt32Marshaler.Demarshal(stream);
            return unit;
        }
    }
}
