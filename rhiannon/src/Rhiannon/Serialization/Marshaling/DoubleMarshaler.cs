using System.IO;

namespace Rhiannon.Serialization.Marshaling
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

		unsafe public static void Marshal(double value, Stream stream)
		{
			byte[] buffer = new byte[sizeof(double)];
			fixed (byte* pointer = buffer)
			{
				*((double*)pointer) = value;
			}
			stream.Write(buffer, 0, buffer.Length);
		}

		unsafe public static double Demarshal(Stream stream)
		{
			byte[] buffer = new byte[sizeof(double)];
			stream.Read(buffer, 0, buffer.Length);
			double value;
			fixed (byte* pointer = buffer)
			{
				value = *((double*)pointer);
			}
			return value;
		}
	}
}
