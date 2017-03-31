using System.IO;

namespace Chronos.Marshaling
{
    public class UInt64Marshaler : GenericMarshaler<ulong>
    {
        protected override void MarshalInternal(ulong value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override ulong DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static unsafe void Marshal(ulong value, Stream stream)
        {
            byte[] buffer = new byte[sizeof (ulong)];
            fixed (byte* pointer = buffer)
            {
                *((ulong*) pointer) = value;
            }
            stream.Write(buffer, 0, buffer.Length);
        }

        public static unsafe ulong Demarshal(Stream stream)
        {
            byte[] buffer = new byte[sizeof (ulong)];
            stream.Read(buffer, 0, buffer.Length);
            ulong value;
            fixed (byte* pointer = buffer)
            {
                value = *((ulong*) pointer);
            }
            return value;
        }
    }
}
