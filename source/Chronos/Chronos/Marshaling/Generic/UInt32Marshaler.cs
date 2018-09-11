using System.IO;

namespace Chronos.Marshaling
{
    public class UInt32Marshaler : GenericMarshaler<uint>
    {
        protected override void MarshalInternal(uint value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override uint DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static unsafe void Marshal(uint value, Stream stream)
        {
            byte[] buffer = new byte[sizeof (uint)];
            fixed (byte* pointer = buffer)
            {
                *((uint*) pointer) = value;
            }
            stream.Write(buffer, 0, buffer.Length);
        }

        public static unsafe uint Demarshal(Stream stream)
        {
            byte[] buffer = new byte[sizeof (uint)];
            stream.Read(buffer, 0, buffer.Length);
            uint value;
            fixed (byte* pointer = buffer)
            {
                value = *((uint*) pointer);
            }
            return value;
        }
    }
}
