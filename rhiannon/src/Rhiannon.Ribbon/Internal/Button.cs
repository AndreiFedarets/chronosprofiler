using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class Button : ControlBase, IButton
	{
		public Button(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
			: base(node, callbacks, factory, nodeLocator)
		{
		}

		protected IButtonCallback ButtonCallback
		{
			get { return (IButtonCallback) ControlCallback; }
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
