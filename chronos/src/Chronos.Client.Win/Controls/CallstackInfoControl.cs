using System.Windows;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
    public class CallstackInfoControl : UnitInfoControl<CallstackInfo>
    {
        private static readonly DependencyProperty EventNameFormatterProperty;

        static CallstackInfoControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CallstackInfoControl), new FrameworkPropertyMetadata(typeof(CallstackInfoControl)));
            EventNameFormatterProperty = DependencyProperty.Register("EventNameFormatter", typeof(IEventNameFormatter), typeof(CallstackInfoControl), new PropertyMetadata(OnEventNameFormatterPropertyChanged));
        }

        public IEventNameFormatter EventNameFormatter
        {
            get { return (IEventNameFormatter) GetValue(EventNameFormatterProperty); }
            set { SetValue(EventNameFormatterProperty, value); }
        }

        private static void OnEventNameFormatterPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            CallstackInfoControl control = (CallstackInfoControl) sender;
            control.OnUnitPropertyChanged(control.Unit);
        }

        protected override void OnUnitPropertyChanged(CallstackInfo unit)
        {
            if (EventNameFormatter != null && string.IsNullOrEmpty(UnitName))
            {
                UnitName = EventNameFormatter.FormatName(unit.RootEvent);
            }
        }
    }
}
