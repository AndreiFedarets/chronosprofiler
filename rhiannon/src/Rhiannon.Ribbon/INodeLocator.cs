using System.Collections.Generic;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon
{
	public interface INodeLocator
	{
		INode FindTabCollectionNode(IDocument document);

		INode FindContextMenuCollectionNode(IDocument document);

		IEnumerable<INode> FindTabNodes(INode node);

		IEnumerable<INode> FindContextMenuNodes(INode node);

		IEnumerable<INode> FindGroupNodes(INode node);
	}
}
