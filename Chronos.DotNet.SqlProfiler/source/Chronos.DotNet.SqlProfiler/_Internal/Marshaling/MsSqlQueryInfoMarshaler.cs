using System.IO;
using Chronos.Model;

namespace Chronos.DotNet.SqlProfiler.Marshaling
{
    internal class MsSqlQueryInfoMarshaler : NativeUnitMarshaler<MsSqlQueryNativeInfo>
    {
        protected override void MarshalInternal(MsSqlQueryNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override MsSqlQueryNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(MsSqlQueryNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<MsSqlQueryNativeInfo>.Marshal(value, stream);
        }

        public new static MsSqlQueryNativeInfo Demarshal(Stream stream)
        {
            MsSqlQueryNativeInfo unit = NativeUnitMarshaler<MsSqlQueryNativeInfo>.Demarshal(stream);
            return unit;
        }
    }
}
