using System.IO;
using Chronos.Model;

namespace Chronos.DotNet.SqlProfiler.Marshaling
{
    internal class SqlQueryInfoMarshaler : NativeUnitMarshaler<SqlQueryNativeInfo>
    {
        protected override void MarshalInternal(SqlQueryNativeInfo value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override SqlQueryNativeInfo DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public new static void Marshal(SqlQueryNativeInfo value, Stream stream)
        {
            NativeUnitMarshaler<SqlQueryNativeInfo>.Marshal(value, stream);
        }

        public new static SqlQueryNativeInfo Demarshal(Stream stream)
        {
            SqlQueryNativeInfo unit = NativeUnitMarshaler<SqlQueryNativeInfo>.Demarshal(stream);
            return unit;
        }
    }
}
