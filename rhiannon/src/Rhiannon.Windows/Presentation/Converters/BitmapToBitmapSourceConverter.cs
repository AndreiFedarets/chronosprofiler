using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using Rhiannon.Extensions;

namespace Rhiannon.Windows.Presentation.Converters
{
	public class BitmapToBitmapSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Bitmap bitmap = value as Bitmap;
			if (bitmap != null)
			{
				return bitmap.ToBitmapSource();
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
