using System.Windows;
using System.Windows.Controls;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class HeaderedContentControlBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			HeaderedContentControl headeredContentControl = element as HeaderedContentControl;
			if (headeredContentControl != null)
			{
				ApplyContent(headeredContentControl, provider);
			}
		}

		private void ApplyContent(HeaderedContentControl headeredContentControl, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Header", headeredContentControl.Uid)];
			if (resource != null)
			{
				headeredContentControl.Header = resource;
			}
		}
	}
}
