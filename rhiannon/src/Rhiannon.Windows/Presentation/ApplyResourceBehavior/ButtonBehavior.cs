using System.Windows;
using System.Windows.Controls;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class ButtonBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			Button button = element as Button;
			if (button != null)
			{
				ApplyContent(button, provider);
			}
		}

		private void ApplyContent(Button button, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Content", button.Uid)];
			if (resource != null)
			{
				button.Content = resource;
			}
			resource = provider[string.Format("{0}_ToolTip", button.Uid)];
			if (resource != null)
			{
				button.ToolTip = resource;
			}
		}
	}
}
