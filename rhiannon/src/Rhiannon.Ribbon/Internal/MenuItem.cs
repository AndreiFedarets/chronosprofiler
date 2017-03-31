using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class MenuItem : ContainerControlBase<IMenuItem>, IMenuItem
	{
		public MenuItem(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
			: base(node, callbacks, factory, nodeLocator)
		{
		}

		protected IButtonCallback ButtonCallback
		{
			get { return (IButtonCallback)ControlCallback; }
		}

		public void OnAction()
		{
			if (ButtonCallback != null)
			{
				ButtonCallback.OnAction();
			}
		}
	}
}
