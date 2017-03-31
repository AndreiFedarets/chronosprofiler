using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public class ByteMarshaler : GenericMarshaler<byte>
	{
		protected override void MarshalInternal(byte value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override byte DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		public static void Marshal(byte value, Stream stream)
		{
			stream.WriteByte(value);
		}

		public static byte Demarshal(Stream stream)
		{
			return (byte)stream.ReadByte();
		}
	}
}
