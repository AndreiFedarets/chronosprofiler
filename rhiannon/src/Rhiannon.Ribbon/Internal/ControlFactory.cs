using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class ControlFactory : IControlFactory
	{
		public IControl CreateControl(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			switch (node.Name)
			{
				case Constants.Controls.Button:
					return CreateButton(node, callbacks, nodeLocator);
				case Constants.Controls.MenuItem:
					return CreateMenuItem(node, callbacks, nodeLocator);
				case Constants.Controls.Group:
					return CreateGroup(node, callbacks, nodeLocator);
			}
			return null;
		}

		public IButton CreateButton(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			return new Button(node, callbacks, this, nodeLocator);
		}

		public IMenuItem CreateMenuItem(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			return new MenuItem(node, callbacks, this, nodeLocator);
		}

		public ITabCollection CreateTabCollection(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			return new TabCollection(node, callbacks, this, nodeLocator);
		}

		public IContextMenuCollection CreateContextMenuCollection(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			return new ContextMenuCollection(node, callbacks, this, nodeLocator);
		}

		public ITab CreateTab(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			return new Tab(node, callbacks, this, nodeLocator);
		}

		public IContextMenu CreateContextMenu(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			return new ContextMenu(node, callbacks, this, nodeLocator);
		}

		public IGroup CreateGroup(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator)
		{
			return new Group(node, callbacks, this, nodeLocator);
		}
	}
}
