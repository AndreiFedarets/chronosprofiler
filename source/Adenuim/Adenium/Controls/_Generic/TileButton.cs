using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Adenium.Controls
{
    public class TileButton : Button
    {
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty IconProperty;

        static TileButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (TileButton), new FrameworkPropertyMetadata(typeof (TileButton)));
            IconProperty = DependencyProperty.Register("Icon", typeof (ImageSource), typeof (TileButton), new FrameworkPropertyMetadata(null));
            TextProperty = DependencyProperty.Register("Text", typeof (string), typeof (TileButton), new FrameworkPropertyMetadata(null));
        }

        public ImageSource Icon
        {
            get { return (ImageSource) GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
