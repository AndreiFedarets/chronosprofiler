using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rhiannon.Windows.Controls
{
    [TemplatePart(Name = ImagePartName, Type = typeof(Image))]
	public class ImageButton : Button
    {
        private const string ImagePartName = "Image";

        public static readonly DependencyProperty ImageSourceProperty;

        private Image _image;

		static ImageButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
            ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null, OnImagePropertyChanged));
		}

        public ImageSource ImageSource
		{
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _image = GetTemplateChild(ImagePartName) as Image;
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (_image != null)
            {
                _image.Source = ImageSource;
            }
        }

        public static void OnImagePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ImageButton imageButton = sender as ImageButton;
            if (imageButton != null)
            {
                imageButton.UpdateImage();
            }
        }

	}
}
