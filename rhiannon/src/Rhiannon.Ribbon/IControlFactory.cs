using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon
{
	public interface IControlFactory
	{
		IControl CreateControl(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator);

		ITabCollection CreateTabCollection(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator);

		IContextMenuCollection CreateContextMenuCollection(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator);

		ITab CreateTab(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator);

		IContextMenu CreateContextMenu(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator);

		IGroup CreateGroup(INode node, IControlCallbackCollection callbacks, INodeLocator nodeLocator);
	}
}
