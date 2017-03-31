using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class ContextMenu : ContainerControlBase<IMenuItem>, IContextMenu
	{
		public ContextMenu(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
			: base(node, callbacks, factory, nodeLocator)
		{
		}
	}
}
