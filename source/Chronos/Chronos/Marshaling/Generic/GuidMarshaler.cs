using System;
using System.Diagnostics;
using System.IO;

namespace Chronos.Marshaling
{
    public class GuidMarshaler : GenericMarshaler<Guid>
    {
        private const int GuidSize = 16;

        protected override void MarshalInternal(Guid value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override Guid DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static void Marshal(Guid value, Stream stream)
        {
            byte[] data = value.ToByteArray();
            Debug.Assert(data.Length == GuidSize, "Size of System.Guid should be 16");
            stream.Write(data, 0, GuidSize);
        }

        public static Guid Demarshal(Stream stream)
        {
            byte[] data = new byte[GuidSize];
            stream.Read(data, 0, GuidSize);
            Guid value = new Guid(data);
            return value;
        }
    }
}
