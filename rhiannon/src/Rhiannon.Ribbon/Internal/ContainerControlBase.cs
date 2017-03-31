using System.Collections.Generic;
using System.Linq;
using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class ContainerControlBase<T> : ControlBase, IContainerControl<T> where T : IControl
	{
		private readonly IList<T> _children;

		public ContainerControlBase(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
			: base(node, callbacks, factory, nodeLocator)
		{
			_children = Node.Children.Select(x => Factory.CreateControl(x, Callbacks, NodeLocator)).Where(x => x is T).Cast<T>().ToList();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _children.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
