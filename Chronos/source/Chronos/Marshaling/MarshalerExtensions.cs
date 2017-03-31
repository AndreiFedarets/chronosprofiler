using System.IO;

namespace Chronos.Marshaling
{
    public static class MarshalerExtensions
    {
        public static byte[] MarshalObject(this ITypeMarshaler marshaler, object value)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                marshaler.MarshalObject(value, memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static object DemarshalObject(this ITypeMarshaler marshaler, byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                return marshaler.DemarshalObject(memoryStream);
            }
        }
    }
}
