using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chronos.Client.Win.Controls
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty SourceProperty;

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
            SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null));
        }

        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
    }
}
