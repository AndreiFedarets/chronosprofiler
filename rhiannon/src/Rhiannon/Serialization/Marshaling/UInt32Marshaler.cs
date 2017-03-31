using System.IO;

namespace Rhiannon.Serialization.Marshaling
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

		unsafe public static void Marshal(uint value, Stream stream)
		{
			byte[] buffer = new byte[sizeof(uint)];
			fixed (byte* pointer = buffer)
			{
				*((uint*)pointer) = value;
			}
			stream.Write(buffer, 0, buffer.Length);
		}

		unsafe public static uint Demarshal(Stream stream)
		{
			byte[] buffer = new byte[sizeof(uint)];
			stream.Read(buffer, 0, buffer.Length);
			uint value;
			fixed (byte* pointer = buffer)
			{
				value = *((uint*)pointer);
			}
			return value;
		}
	}
}
