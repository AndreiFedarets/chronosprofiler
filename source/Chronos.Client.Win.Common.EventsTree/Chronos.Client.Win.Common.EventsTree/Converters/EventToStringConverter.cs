using Chronos.Common.EventsTree;
using System;
using System.Windows.Data;

namespace Chronos.Client.Win.Converters.Common.EventsTree
{
    public class EventToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEvent @event = (IEvent)values[0];
            IEventMessageBuilder formatter = (IEventMessageBuilder)values[1];
            return formatter.BuildMessage(@event);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
