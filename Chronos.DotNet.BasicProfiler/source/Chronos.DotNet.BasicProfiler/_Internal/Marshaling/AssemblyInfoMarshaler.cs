using System.IO;
using Chronos.Marshaling;
using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler.Marshaling
{
    internal class AssemblyInfoMarshaler : NativeUnitMarshaler<AssemblyNativeInfo>
    {
        protected override void MarshalInternal(AssemblyNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override AssemblyNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(AssemblyNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<AssemblyNativeInfo>.Marshal(value, stream);
            UInt64Marshaler.Marshal(value.AppDomainId, stream);
        }

        public new static AssemblyNativeInfo Demarshal(Stream stream)
        {
            AssemblyNativeInfo unit = NativeUnitMarshaler<AssemblyNativeInfo>.Demarshal(stream);
            unit.AppDomainId = UInt64Marshaler.Demarshal(stream);
            return unit;
        }
    }
}
