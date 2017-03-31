using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public class Int64Marshaler : GenericMarshaler<long>
	{
		protected override void MarshalInternal(long value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override long DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		unsafe public static void Marshal(long value, Stream stream)
		{
			byte[] buffer = new byte[sizeof(long)];
			fixed (byte* pointer = buffer)
			{
				*((long*)pointer) = value;
			}
			stream.Write(buffer, 0, buffer.Length);
		}

		unsafe public static long Demarshal(Stream stream)
		{
			byte[] buffer = new byte[sizeof(long)];
			stream.Read(buffer, 0, buffer.Length);
			long value;
			fixed (byte* pointer = buffer)
			{
				value = *((long*)pointer);
			}
			return value;
		}
	}
}
