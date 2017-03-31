using System.Windows;
using System.Windows.Controls;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class MenuItemBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			MenuItem menuItem = element as MenuItem;
			if (menuItem != null)
			{
				ApplyHeader(menuItem, provider);
			}
		}

		private void ApplyHeader(MenuItem menuItem, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Header", menuItem.Uid)];
			if (resource != null)
			{
				menuItem.Header = resource;
			}
		}
	}
}
