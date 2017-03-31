using System.Resources;

namespace Rhiannon.Resources
{
	public class TextResource : IResource
	{
		private readonly ResourceManager _resourceManager;

		public TextResource(ResourceManager resourceManager)
		{
			_resourceManager = resourceManager;
		}

		public object this[string key]
		{
			get { return _resourceManager.GetString(key); }
		}

		public bool Contains(string key)
		{
			string value = (string) this[key];
			return !string.IsNullOrEmpty(value);
		}
	}
}
