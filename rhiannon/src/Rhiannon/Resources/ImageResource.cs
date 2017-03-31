using System.Resources;

namespace Rhiannon.Resources
{
	public class ImageResource : IResource
	{
		private readonly ResourceManager _resourceManager;

		public ImageResource(ResourceManager resourceManager)
		{
			_resourceManager = resourceManager;
		}

		public object this[string key]
		{
			get { return _resourceManager.GetObject(key); }
		}

		public bool Contains(string key)
		{
			return this[key] != null;
		}
	}
}
