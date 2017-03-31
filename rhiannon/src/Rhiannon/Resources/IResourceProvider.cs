using System.Resources;

namespace Rhiannon.Resources
{
	public interface IResourceProvider
	{
		object this[string key] { get; }

		void RegisterManager(ResourceManager resourceManager, ResourceType resourceType);
	}
}
