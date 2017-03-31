using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public class BoolMarshaler : GenericMarshaler<bool>
	{
		protected override void MarshalInternal(bool value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override bool DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		public static void Marshal(bool value, Stream stream)
		{
			byte buffer = (byte)(value ? 1 : 0);
			stream.WriteByte(buffer);
		}

		public static bool Demarshal(Stream stream)
		{
			int buffer = stream.ReadByte();
			return buffer != 0;
		}
	}
}
