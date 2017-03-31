using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Rhiannon.Windows.Presentation.Converters
{
    public class MessageBoxButtonToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            if (value is MessageBoxButton && parameter is MessageBoxResult)
			{
			    MessageBoxButton button = (MessageBoxButton) value;
			    MessageBoxResult result = (MessageBoxResult) parameter;
			    switch (result)
			    {
                    case MessageBoxResult.OK:
			            return button == MessageBoxButton.OK || button == MessageBoxButton.OKCancel;
                    case MessageBoxResult.Cancel:
			            return button == MessageBoxButton.OKCancel || button == MessageBoxButton.YesNoCancel;
                    case MessageBoxResult.Yes:
			            return button == MessageBoxButton.YesNo || button == MessageBoxButton.YesNoCancel;
                    case MessageBoxResult.No:
			            return button == MessageBoxButton.YesNo || button == MessageBoxButton.YesNoCancel;
			    }
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
		    throw new NotSupportedException();
		}
	}
}
