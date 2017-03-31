namespace Rhiannon.Resources
{
	public class EmptyTextResource : IResource
	{
		public object this[string key]
		{
			get { return string.Empty; }
		}

		public bool Contains(string key)
		{
			return false;
		}
	}
}
