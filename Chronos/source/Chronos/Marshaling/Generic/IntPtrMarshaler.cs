using System;
using System.IO;

namespace Chronos.Marshaling
{
    public class IntPtrMarshaler : GenericMarshaler<IntPtr>
    {
        protected override void MarshalInternal(IntPtr value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override IntPtr DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static void Marshal(IntPtr value, Stream stream)
        {
            Int64Marshaler.Marshal(value.ToInt64(), stream);
        }

        public static IntPtr Demarshal(Stream stream)
        {
            return new IntPtr(Int64Marshaler.Demarshal(stream));
        }
    }
}
