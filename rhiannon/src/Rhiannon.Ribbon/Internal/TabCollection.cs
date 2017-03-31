using System.Collections.Generic;
using System.Linq;
using Rhiannon.Logging;
using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class TabCollection : ITabCollection
	{
		private readonly IList<ITab> _tabs;

		public TabCollection(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
		{
			_tabs = Initialize(node, callbacks, factory, nodeLocator);
		}

		private IList<ITab> Initialize(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
		{
			if (node == null)
			{
				LoggingProvider.Current.Log("tabs node is null", Constants.Functionality, Policy.Core, LogEntryType.Warning);
				return new List<ITab>();
			}
			IEnumerable<INode> tabNodes = nodeLocator.FindTabNodes(node);
			IList<ITab> tabs = tabNodes.Select(x => factory.CreateTab(x, callbacks, nodeLocator)).ToList();
			return tabs;
		}

		public IEnumerator<ITab> GetEnumerator()
		{
			return _tabs.GetEnumerator();
		}

		public void Invalidate()
		{
			foreach (ITab tab in this)
			{
				tab.Invalidate();
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
