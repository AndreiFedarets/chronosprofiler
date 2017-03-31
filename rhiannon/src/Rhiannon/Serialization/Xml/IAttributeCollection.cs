using System.Collections.Generic;

namespace Rhiannon.Serialization.Xml
{
	public interface IAttributeCollection : IEnumerable<IAttribute>
	{
		IAttribute Add(string name);

		IAttribute this[string name] { get; }

		bool Exists(string name);
	}
}
