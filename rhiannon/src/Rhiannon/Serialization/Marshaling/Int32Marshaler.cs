using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public class Int32Marshaler : GenericMarshaler<int>
	{
		protected override void MarshalInternal(int value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override int DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		unsafe public static void Marshal(int value, Stream stream)
		{
			byte[] buffer = new byte[sizeof(int)];
			fixed (byte* pointer = buffer)
			{
				*((int*)pointer) = value;
			}
			stream.Write(buffer, 0, buffer.Length);
		}

		unsafe public static int Demarshal(Stream stream)
		{
			byte[] buffer = new byte[sizeof(int)];
			stream.Read(buffer, 0, buffer.Length);
			int value;
			fixed (byte* pointer = buffer)
			{
				value = *((int*)pointer);
			}
			return value;
		}
	}
}
