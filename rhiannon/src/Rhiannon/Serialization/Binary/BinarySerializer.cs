using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Rhiannon.Serialization.Binary
{
	internal class BinarySerializer : BaseSerializer
	{
		private readonly BinaryFormatter _binaryFormatter;

		public BinarySerializer()
		{
			_binaryFormatter = new BinaryFormatter();
		}

		public override T Deserialize<T>(Stream stream)
		{
			T result = (T)_binaryFormatter.Deserialize(stream);
			return result;
		}

		public override void Serialize(Stream stream, object obj)
		{
			_binaryFormatter.Serialize(stream, obj);
		}
	}
}
