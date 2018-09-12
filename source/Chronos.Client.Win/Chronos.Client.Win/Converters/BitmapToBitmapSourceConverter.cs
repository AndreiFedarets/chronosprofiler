using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace Chronos.Client.Win.Converters
{
    public class BitmapToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Bitmap bitmap = (Bitmap) value;
            return bitmap.ToBitmapSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
            //BitmapSource bitmapSource = (BitmapSource) value;
            //return value.ToBitmap();
        }
    }
}
