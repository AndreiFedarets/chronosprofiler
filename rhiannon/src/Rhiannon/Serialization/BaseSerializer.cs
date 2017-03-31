using System.IO;
using System.Text;

namespace Rhiannon.Serialization
{
	internal abstract class BaseSerializer : ISerializer
	{
		public abstract T Deserialize<T>(Stream stream);

		public abstract void Serialize(Stream stream, object obj);

		public T Deserialize<T>(FileInfo fileInfo)
		{
			using (Stream reader = fileInfo.OpenRead())
			{
				return Deserialize<T>(reader);
			}
		}

		public void Serialize(FileInfo fileInfo, object obj)
		{
			using (Stream stream = fileInfo.OpenWrite())
			{
				Serialize(stream, obj);
			}
		}

		public string Serialize(object obj)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				Serialize(stream, obj);
				string value = Encoding.Default.GetString(stream.ToArray());
				return value;
			}
		}

		public T Deserialize<T>(string value)
		{
			byte[] bytes = Encoding.Default.GetBytes(value);
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				return Deserialize<T>(stream);
			}
		}
	}
}
