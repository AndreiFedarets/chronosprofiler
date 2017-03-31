namespace Rhiannon.Serialization
{
	public interface ISerializerFactory
	{
		ISerializer Create<T>(SerializerType serializerType);

		ISerializer CreateXml<T>();

		ISerializer CreateBinary();
	}
}
