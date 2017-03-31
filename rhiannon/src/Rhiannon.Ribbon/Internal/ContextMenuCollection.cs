using System.Collections.Generic;
using System.Linq;
using Rhiannon.Logging;
using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class ContextMenuCollection : IContextMenuCollection
	{
		private readonly IList<IContextMenu> _contextMenus;

		public ContextMenuCollection(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
		{
			_contextMenus = Initialize(node, callbacks, factory, nodeLocator);
		}

		private IList<IContextMenu> Initialize(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
		{
			if (node == null)
			{
				LoggingProvider.Current.Log("contextMenus node is null", Constants.Functionality, Policy.Core, LogEntryType.Warning);
				return new List<IContextMenu>();
			}
			IEnumerable<INode> contextMenuNodes = nodeLocator.FindContextMenuNodes(node);
			IList<IContextMenu> contextMenus = contextMenuNodes.Select(x => factory.CreateContextMenu(x, callbacks, nodeLocator)).ToList();
			return contextMenus;
		}

		public IEnumerator<IContextMenu> GetEnumerator()
		{
			return _contextMenus.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Invalidate()
		{
			foreach (IContextMenu contextMenu in this)
			{
				contextMenu.Invalidate();
			}
		}
	}
}
