using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using Rhiannon.Extensions;

namespace Rhiannon.Windows.Presentation.Converters
{
	public class ByteArrayToBitmapSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			byte[] data = value as byte[];
			if (data == null || data.Length == 0)
			{
				return null;
			}
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				Bitmap bitmap = new Bitmap(memoryStream);
				return bitmap.ToBitmapSource();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
