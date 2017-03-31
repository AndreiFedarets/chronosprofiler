using System.IO;
using Chronos.Marshaling;
using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler.Marshaling
{
    internal class ModuleInfoMarshaler : NativeUnitMarshaler<ModuleNativeInfo>
    {
        protected override void MarshalInternal(ModuleNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override ModuleNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(ModuleNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<ModuleNativeInfo>.Marshal(value, stream);
            UInt64Marshaler.Marshal(value.AssemblyId, stream);
        }

        public new static ModuleNativeInfo Demarshal(Stream stream)
        {
            ModuleNativeInfo unit = NativeUnitMarshaler<ModuleNativeInfo>.Demarshal(stream);
            unit.AssemblyId = UInt64Marshaler.Demarshal(stream);
            return unit;
        }
    }
}
