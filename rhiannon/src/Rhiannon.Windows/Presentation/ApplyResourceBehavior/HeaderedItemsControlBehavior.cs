using System.Windows;
using System.Windows.Controls;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class HeaderedItemsControlBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			HeaderedItemsControl headeredItemsControl = element as HeaderedItemsControl;
			if (headeredItemsControl != null)
			{
				ApplyHeader(headeredItemsControl, provider);
			}
		}

		private void ApplyHeader(HeaderedItemsControl headeredItemsControl, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Header", headeredItemsControl.Uid)];
			if (resource != null)
			{
				headeredItemsControl.Header = resource;
			}
		}
	}
}
