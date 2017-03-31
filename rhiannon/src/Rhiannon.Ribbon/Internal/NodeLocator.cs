using System.Collections.Generic;
using System.Linq;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class NodeLocator : INodeLocator
	{
		public INode FindTabCollectionNode(IDocument document)
		{
			return document.Children.FindFirst(Constants.Controls.TabCollection);
		}

		public INode FindContextMenuCollectionNode(IDocument document)
		{
			return document.Children.FindFirst(Constants.Controls.ContextMenuCollection);
		}

		public IEnumerable<INode> FindTabNodes(INode node)
		{
			return node.Children.Where(x => string.Equals(x.Name, Constants.Controls.Tab));
		}

		public IEnumerable<INode> FindContextMenuNodes(INode node)
		{
			return node.Children.Where(x => string.Equals(x.Name, Constants.Controls.ContextMenu));
		}

		public IEnumerable<INode> FindGroupNodes(INode node)
		{
			return node.Children.Where(x => string.Equals(x.Name, Constants.Controls.Group));
		}
	}
}
