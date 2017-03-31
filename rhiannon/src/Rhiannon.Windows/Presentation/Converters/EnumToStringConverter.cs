using System;
using System.Globalization;
using System.Windows.Data;

namespace Rhiannon.Windows.Presentation.Converters
{
    public class EnumToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (!value.GetType().IsEnum)
            {
                throw new ArgumentException("value");
            }
		    return value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
