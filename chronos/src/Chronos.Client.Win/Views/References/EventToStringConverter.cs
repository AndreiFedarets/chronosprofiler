using System;
using System.Windows.Data;
using Chronos.Core;

namespace Chronos.Client.Win.Views.References
{
    public class EventToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEvent @event = (IEvent)values[0];
            IEventNameFormatter formatter = (IEventNameFormatter) values[1];
            return formatter.FormatName(@event);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
