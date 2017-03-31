using System.Collections.Generic;

namespace Rhiannon.Serialization.Xml
{
	public interface INodeCollection : IEnumerable<INode>
	{
		INode Add(string nodeName);

		INode FindFirst(string nodeName);
	}
}
