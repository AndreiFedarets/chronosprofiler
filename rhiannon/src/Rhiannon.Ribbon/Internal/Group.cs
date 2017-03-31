using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class Group : ContainerControlBase<IControl>, IGroup
	{
		public Group(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
			: base(node, callbacks, factory, nodeLocator)
		{
		}

		public override void Invalidate()
		{
			base.Invalidate();
			foreach (IControl control in this)
			{
				control.Invalidate();
			}
		}
	}
}
