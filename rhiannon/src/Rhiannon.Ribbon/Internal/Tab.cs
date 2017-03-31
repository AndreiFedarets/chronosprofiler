using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class Tab : ContainerControlBase<IGroup>, ITab
	{
		public Tab(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
			: base(node, callbacks, factory, nodeLocator)
		{
		}

		public override void Invalidate()
		{
			base.Invalidate();
			foreach (IGroup group in this)
			{
				group.Invalidate();
			}
		}
	}
}
