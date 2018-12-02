using System.IO;

namespace Chronos.Marshaling
{
    public class Int16Marshaler : GenericMarshaler<short>
    {
        protected override void MarshalInternal(short value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override short DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static unsafe void Marshal(short value, Stream stream)
        {
            byte[] buffer = new byte[sizeof(short)];
            fixed (byte* pointer = buffer)
            {
                *((short*)pointer) = value;
            }
            stream.Write(buffer, 0, buffer.Length);
        }

        public static unsafe short Demarshal(Stream stream)
        {
            byte[] buffer = new byte[sizeof(short)];
            stream.Read(buffer, 0, buffer.Length);
            short value;
            fixed (byte* pointer = buffer)
            {
                value = *((short*)pointer);
            }
            return value;
        }
    }
}
