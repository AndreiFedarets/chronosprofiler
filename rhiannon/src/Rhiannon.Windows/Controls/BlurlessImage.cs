using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rhiannon.Windows.Controls
{
	public class BlurlessImage : Image
	{
		public BlurlessImage()
		{
			SnapsToDevicePixels = true;
			Stretch = Stretch.None;
			SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.NearestNeighbor);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			Size measureSize = new Size();
			BitmapSource bitmapSource = (BitmapSource)Source;
			if (bitmapSource != null)
			{
				measureSize = new Size(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
			}
			return measureSize;
		}
	}
}
