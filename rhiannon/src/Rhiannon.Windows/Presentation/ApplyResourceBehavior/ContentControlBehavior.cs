using System.Windows;
using System.Windows.Controls;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class ContentControlBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			ContentControl contentControl = element as ContentControl;
			if (contentControl != null)
			{
				ApplyContent(contentControl, provider);
			}
		}

		private void ApplyContent(ContentControl contentControl, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Content", contentControl.Uid)];
			if (resource != null)
			{
				contentControl.Content = resource;
			}
		}
	}
}
