using System.Collections.Generic;
using System.Linq;
using Rhiannon.Logging;
using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon.Internal
{
	internal class GroupCollection : IGroupCollection
	{
		private readonly IList<IGroup> _groups;

		public GroupCollection(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
		{
			_groups = Initialize(node, callbacks, factory, nodeLocator);
		}

		private IList<IGroup> Initialize(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
		{
			if (node == null)
			{
				LoggingProvider.Current.Log("groups node is null", Constants.Functionality, Policy.Core, LogEntryType.Warning);
				return new List<IGroup>();
			}
			IEnumerable<INode> groupNodes = nodeLocator.FindGroupNodes(node);
			IList<IGroup> groups = groupNodes.Select(x => factory.CreateGroup(x, callbacks, nodeLocator)).ToList();
			return groups;
		}

		public IEnumerator<IGroup> GetEnumerator()
		{
			return _groups.GetEnumerator();
		}

		public void Invalidate()
		{
			foreach (IGroup group in this)
			{
				group.Invalidate();
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
