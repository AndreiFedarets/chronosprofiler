using System.Windows;
using System.Windows.Controls;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class TextBlockBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			TextBlock textBlock = element as TextBlock;
			if (textBlock != null)
			{
				ApplyText(textBlock, provider);
			}
		}

		private void ApplyText(TextBlock textBlock, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Text", textBlock.Uid)];
			if (resource != null)
			{
				textBlock.Text = (string)resource;
			}
		}
	}
}
