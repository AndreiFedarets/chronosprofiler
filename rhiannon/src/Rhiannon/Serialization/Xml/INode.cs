namespace Rhiannon.Serialization.Xml
{
	public interface INode
	{
		string Value { get; set; }

		string Name { get; }

		IDocument Owner { get; }

		IAttributeCollection Attributes { get; }

		INodeCollection Children { get; }

		void Delete();

		T GetValueAs<T>();

		void SetValueAs<T>(T value);
	}
}
