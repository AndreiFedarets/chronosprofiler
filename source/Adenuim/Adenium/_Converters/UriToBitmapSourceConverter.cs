using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Adenium
{
    public class UriToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapSource bitmapSource = null;
            string uri = value == null ? string.Empty : value.ToString();
            if (!string.IsNullOrEmpty(uri))
            {
                bitmapSource = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
            }
            return bitmapSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
