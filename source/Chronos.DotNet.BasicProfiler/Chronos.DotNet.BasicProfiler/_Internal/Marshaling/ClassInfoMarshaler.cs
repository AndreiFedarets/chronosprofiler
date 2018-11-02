using System.IO;
using Chronos.Common;
using Chronos.Marshaling;

namespace Chronos.DotNet.BasicProfiler.Marshaling
{
    internal class ClassInfoMarshaler : NativeUnitMarshaler<ClassNativeInfo>
    {
        protected override void MarshalInternal(ClassNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override ClassNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(ClassNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<ClassNativeInfo>.Marshal(value, stream);
            UInt32Marshaler.Marshal(value.TypeToken, stream);
            UInt64Marshaler.Marshal(value.ModuleId, stream);
        }

        public new static ClassNativeInfo Demarshal(Stream stream)
        {
            ClassNativeInfo unit = NativeUnitMarshaler<ClassNativeInfo>.Demarshal(stream);
            unit.TypeToken = UInt32Marshaler.Demarshal(stream);
            unit.ModuleId = UInt64Marshaler.Demarshal(stream);
            return unit;
        }
    }
}
