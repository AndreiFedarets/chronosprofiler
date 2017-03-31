using System;
using System.IO;

namespace Chronos.Marshaling
{
    public class ByteArrayMarshaler : GenericMarshaler<byte[]>
    {
        protected override void MarshalInternal(byte[] value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override byte[] DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static void Marshal(byte[] value, Stream stream)
        {
            Int32Marshaler.Marshal(value.Length, stream);
            if (value.Length > 0)
            {
                stream.Write(value, 0, value.Length);
            }
        }

        public static byte[] Demarshal(Stream stream)
        {
            int length = Int32Marshaler.Demarshal(stream);
            byte[] array = new byte[length];
            if (length > 0)
            {
                stream.Read(array, 0, array.Length);
            }
            return array;
        }
    }
}
