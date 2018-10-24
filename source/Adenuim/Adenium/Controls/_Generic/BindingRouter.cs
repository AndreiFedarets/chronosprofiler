using System.Windows;

namespace Adenium.Controls
{
    public class BindingRouter : FrameworkElement
    {
        public static readonly DependencyProperty SourceProperty;
        public static readonly DependencyProperty TargetProperty;

        static BindingRouter()
        {
            SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(BindingRouter), new FrameworkPropertyMetadata(OnSourcePropertyChanged));
            TargetProperty = DependencyProperty.Register("Target", typeof(object), typeof(BindingRouter), new FrameworkPropertyMetadata(null));
        }

        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public object Target
        {
            get { return GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            BindingRouter control = (BindingRouter) sender;
            control.Target = eventArgs.NewValue;
        }
    }
}
