using System;

namespace Chronos
{
    public static class GuidExtensions
    {
        public static Guid ReverseBits(this Guid guid)
        {
            byte[] data = guid.ToByteArray();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ 255);
            }
            Guid result = new Guid(data);
            return result;
        }
    }
}
