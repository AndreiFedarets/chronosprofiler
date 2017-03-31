using System.IO;
using Chronos.Marshaling;
using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler.Marshaling
{
    internal class FunctionInfoMarshaler : NativeUnitMarshaler<FunctionNativeInfo>
    {
        protected override void MarshalInternal(FunctionNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override FunctionNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(FunctionNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<FunctionNativeInfo>.Marshal(value, stream);
            UInt32Marshaler.Marshal(value.TypeToken, stream);
            UInt64Marshaler.Marshal(value.ClassId, stream);
            UInt64Marshaler.Marshal(value.ModuleId, stream);
            UInt64Marshaler.Marshal(value.AssemblyId, stream);
        }

        public new static FunctionNativeInfo Demarshal(Stream stream)
        {
            FunctionNativeInfo unit = NativeUnitMarshaler<FunctionNativeInfo>.Demarshal(stream);
            unit.TypeToken = UInt32Marshaler.Demarshal(stream);
            unit.ClassId = UInt64Marshaler.Demarshal(stream);
            unit.ModuleId = UInt64Marshaler.Demarshal(stream);
            unit.AssemblyId = UInt64Marshaler.Demarshal(stream);
            return unit;
        }
    }
}
