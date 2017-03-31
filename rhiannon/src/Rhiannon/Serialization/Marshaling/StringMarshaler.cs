using System.IO;
using System.Text;

namespace Rhiannon.Serialization.Marshaling
{
	public class StringMarshaler : GenericMarshaler<string>
	{
		protected override void MarshalInternal(string value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override string DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		public static void Marshal(string value, Stream stream)
		{
			byte[] data = Encoding.Unicode.GetBytes(value);
			Int32Marshaler.Marshal(data.Length, stream);
			stream.Write(data, 0, data.Length);
		}

		public static string Demarshal(Stream stream)
		{
			int size = Int32Marshaler.Demarshal(stream);
			byte[] data = new byte[size];
			stream.Read(data, 0, data.Length);
			return Encoding.Unicode.GetString(data);
		}
	}
}
