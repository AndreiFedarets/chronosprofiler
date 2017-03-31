using System;
using Rhiannon.Ribbon.Client;
using Rhiannon.Ribbon.Internal;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon
{
	public class Ribbon : IRibbon
	{
		public Ribbon(IDocument document, IControlCallbackCollection callbacks)
		{
			IControlFactory factory = new ControlFactory();
			INodeLocator nodesLocator = new NodeLocator();
			Tabs = factory.CreateTabCollection(nodesLocator.FindTabCollectionNode(document), callbacks, nodesLocator);
			ContextMenus = factory.CreateContextMenuCollection(nodesLocator.FindContextMenuCollectionNode(document), callbacks, nodesLocator);
		}

		public ITabCollection Tabs { get; private set; }

		public IContextMenuCollection ContextMenus { get; private set; }

		public void Invalidate()
		{
			InvokeBeforeInvalidate();
			Tabs.Invalidate();
			ContextMenus.Invalidate();
			InvokeAfterInvalidate();
		}
		
		public event EventHandler BeforeInvalidate;

		private void InvokeBeforeInvalidate()
		{
			EventHandler handler = BeforeInvalidate;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public event EventHandler AfterInvalidate;

		private void InvokeAfterInvalidate()
		{
			EventHandler handler = AfterInvalidate;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}
