using System;

namespace Rhiannon.Resources
{
	public class LocalizedPropertyAttribute : Attribute
	{
		public LocalizedPropertyAttribute(string resourceKey)
		{
			ResourceKey = resourceKey;
		}

		public string ResourceKey { get; private set; }
	}
}
