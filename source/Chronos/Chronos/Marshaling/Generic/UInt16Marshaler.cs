using System.IO;

namespace Chronos.Marshaling
{
    public class UInt16Marshaler : GenericMarshaler<ushort>
    {
        protected override void MarshalInternal(ushort value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override ushort DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static unsafe void Marshal(ushort value, Stream stream)
        {
            byte[] buffer = new byte[sizeof(ushort)];
            fixed (byte* pointer = buffer)
            {
                *((ushort*)pointer) = value;
            }
            stream.Write(buffer, 0, buffer.Length);
        }

        public static unsafe ushort Demarshal(Stream stream)
        {
            byte[] buffer = new byte[sizeof(ushort)];
            stream.Read(buffer, 0, buffer.Length);
            ushort value;
            fixed (byte* pointer = buffer)
            {
                value = *((ushort*)pointer);
            }
            return value;
        }
    }
}
