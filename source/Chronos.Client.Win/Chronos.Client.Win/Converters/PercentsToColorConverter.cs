using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Chronos.Client.Win.Converters
{
    public class PercentsToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double percents = (double) value;
            return Convert(percents);
        }

        public static Color Convert(double percents)
        {
            byte green;
            byte red;
            const byte blue = 20;
            if (percents < 50)
            {
                red = (byte)(percents * 255 * 2 / 100);
                green = 255;
            }
            else
            {
                red = 255;
                green = (byte)((100 - percents * 2) * 255 / 100);
            }
            return Color.FromRgb(red, green, blue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
