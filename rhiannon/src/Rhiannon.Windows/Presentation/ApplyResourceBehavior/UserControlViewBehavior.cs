using System.Drawing;
using System.Windows;
using Rhiannon.Extensions;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class UserControlViewBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			UserControlView userControlView = element as UserControlView;
			if (userControlView != null)
			{
				ApplyTitle(userControlView, provider);
				ApplyIcon(userControlView, provider);
			}
		}

		private void ApplyIcon(UserControlView userControlView, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Icon", userControlView.Uid)];
			if (resource is Bitmap)
			{
				userControlView.Icon = (resource as Bitmap).ToBitmapSource();
			}
		}

		private void ApplyTitle(UserControlView userControlView, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Title", userControlView.Uid)];
			if (resource is string)
			{
				userControlView.Title = (string)resource;
			}
		}
	}
}
