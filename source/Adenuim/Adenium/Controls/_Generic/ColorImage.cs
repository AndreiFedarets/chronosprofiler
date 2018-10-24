using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Adenium.Controls
{
    public class ColorImage : Control
    {
        public static readonly DependencyProperty MaskSourceProperty;

        static ColorImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorImage), new FrameworkPropertyMetadata(typeof(ColorImage)));
            MaskSourceProperty = DependencyProperty.Register("MaskSource", typeof(ImageSource), typeof(ColorImage));
        }

        public ColorImage()
        {
            SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.NearestNeighbor);
        }

        public ImageSource MaskSource
        {
            get { return (ImageSource) GetValue(MaskSourceProperty); }
            set { SetValue(MaskSourceProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size measureSize;
            BitmapSource bitmapSource = (BitmapSource)MaskSource;
            if (bitmapSource != null)
            {
                measureSize = new Size(bitmapSource.PixelWidth + Padding.Left + Padding.Right, bitmapSource.PixelHeight + Padding.Top + Padding.Bottom);
            }
            else
            {
                measureSize = base.MeasureOverride(constraint);
            }
            return measureSize;
        }
    }
}
