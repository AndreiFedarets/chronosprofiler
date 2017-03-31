using System.IO;

namespace Rhiannon.Serialization
{
	public interface ISerializer
	{
		T Deserialize<T>(Stream stream);

		T Deserialize<T>(FileInfo fileInfo);

		T Deserialize<T>(string value);

		void Serialize(Stream stream, object @obj);

		void Serialize(FileInfo fileInfo, object @obj);

		string Serialize(object @obj);
	}
}
