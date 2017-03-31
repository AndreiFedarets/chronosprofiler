using System;
using System.Globalization;
using System.Windows.Data;

namespace Rhiannon.Windows.Presentation.Converters
{
	public class EqualityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool equals = Equals(value, parameter);
			return !equals;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
