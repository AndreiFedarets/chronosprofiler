using System.Drawing;
using System.Windows;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class WindowViewBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			WindowView windowView = element as WindowView;
			if (windowView != null)
			{
				ApplyTitle(windowView, provider);
				ApplyIcon(windowView, provider);
			}
		}

		private void ApplyIcon(WindowView windowView, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Icon", windowView.Uid)];
			if (resource is Bitmap)
			{
				//windowView.Icon = (resource as Bitmap).ToIcon();
			}
		}

		private void ApplyTitle(WindowView windowView, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Title", windowView.Uid)];
			if (resource is string)
			{
				windowView.Title = (string)resource;
			}
		}
	}
}
