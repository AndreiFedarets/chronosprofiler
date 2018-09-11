using System.IO;

namespace Chronos.Marshaling
{
    public class DoubleMarshaler : GenericMarshaler<double>
    {
        protected override void MarshalInternal(double value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override double DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static unsafe void Marshal(double value, Stream stream)
        {
            byte[] buffer = new byte[sizeof (double)];
            fixed (byte* pointer = buffer)
            {
                *((double*) pointer) = value;
            }
            stream.Write(buffer, 0, buffer.Length);
        }

        public static unsafe double Demarshal(Stream stream)
        {
            byte[] buffer = new byte[sizeof (double)];
            stream.Read(buffer, 0, buffer.Length);
            double value;
            fixed (byte* pointer = buffer)
            {
                value = *((double*) pointer);
            }
            return value;
        }
    }
}
