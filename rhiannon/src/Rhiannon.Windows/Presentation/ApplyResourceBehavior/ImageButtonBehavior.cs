using System.Windows;
using Rhiannon.Extensions;
using Rhiannon.Resources;
using Rhiannon.Windows.Controls;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal class ImageButtonBehavior : BehaviorBase
	{
		protected override void ApplyElement(DependencyObject element, IResourceProvider provider)
		{
			ImageButton button = element as ImageButton;
			if (button != null)
			{
				ApplyContent(button, provider);
				ApplyImage(button, provider);
			}
		}

		private void ApplyContent(ImageButton button, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Content", button.Uid)];
			if (resource != null)
			{
				button.Content = resource;
			}
		}

		private void ApplyImage(ImageButton button, IResourceProvider provider)
		{
			object resource = provider[string.Format("{0}_Image", button.Uid)];
			System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)resource;
			button.ImageSource = bitmap.ToBitmapSource();
		}
	}
}
