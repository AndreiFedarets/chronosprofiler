using System.Collections.Generic;
using System.Resources;

namespace Rhiannon.Resources
{
	public class ResourceProvider : IResourceProvider
	{
		private readonly IList<IResource> _resources;

		public ResourceProvider()
		{
			_resources = new List<IResource>();
		}

		public ResourceProvider(ResourceManager textResourceManager, ResourceManager imageResourceManager)
			: this()
		{
			RegisterManager(textResourceManager, ResourceType.Text);
			RegisterManager(imageResourceManager, ResourceType.Image);
		}

		public void RegisterManager(ResourceManager resourceManager, ResourceType resourceType)
		{
			IResource resource = null;
			switch (resourceType)
			{
				case ResourceType.Text:
					resource = new TextResource(resourceManager);
					break;
				case ResourceType.Image:
					resource = new ImageResource(resourceManager);
					break;
			}
			if (resource != null)
			{
				_resources.Add(resource);
			}
		}

		public object this[string key]
		{
			get
			{
				foreach (IResource resource in _resources)
				{
					if (resource.Contains(key))
					{
						return resource[key];
					}
				}
				return null;
			}
		}
	}
}
