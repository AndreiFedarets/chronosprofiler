namespace Rhiannon.Resources
{
	public interface IResource
	{
		object this[string key] { get; }

		bool Contains(string key);
	}
}
