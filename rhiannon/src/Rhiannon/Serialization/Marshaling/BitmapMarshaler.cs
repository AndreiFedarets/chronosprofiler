using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Rhiannon.Serialization.Marshaling
{
	public class BitmapMarshaler : GenericMarshaler<Bitmap>
	{
		protected override void MarshalInternal(Bitmap value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override Bitmap DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		public static void Marshal(Bitmap value, Stream stream)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				value.Save(memoryStream, ImageFormat.Png);
				byte[] data = memoryStream.ToArray();
				ByteArrayMarshaler.Marshal(data, stream);
			}
		}

		public static Bitmap Demarshal(Stream stream)
		{
			byte[] data = ByteArrayMarshaler.Demarshal(stream);
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				Bitmap bitmap = new Bitmap(memoryStream);
				return bitmap;
			}
		}
	}
}
