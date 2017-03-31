using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chronos.Client.Win.Controls
{
    public class BlurlessImage : Image
    {
        public BlurlessImage()
        {
            SnapsToDevicePixels = true;
            Stretch = Stretch.Fill;
            SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.NearestNeighbor);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size measureSize;
            BitmapSource bitmapSource = (BitmapSource)Source;
            if (bitmapSource != null)
            {
                measureSize = new Size(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
            }
            else
            {
                measureSize = base.MeasureOverride(constraint);
            }
            return measureSize;
        }
    }
}
