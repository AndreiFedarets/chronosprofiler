namespace Rhiannon.Serialization.Xml
{
	public interface IAttribute
	{
		string Value { get; set; }

		string Name { get; }

		T GetValueAs<T>();

		void SetValueAs<T>(T value);
	}
}
