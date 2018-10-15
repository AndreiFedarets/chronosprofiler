using System.Windows;

namespace Chronos.Client.Win.Controls
{
    public class BindingForward : FrameworkElement
    {
        public static readonly DependencyProperty SourceProperty;
        public static readonly DependencyProperty TargetProperty;

        static BindingForward()
        {
            SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(BindingForward), new FrameworkPropertyMetadata(OnSourcePropertyChanged));
            TargetProperty = DependencyProperty.Register("Target", typeof(object), typeof(BindingForward), new FrameworkPropertyMetadata(null));
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
            BindingForward control = (BindingForward) sender;
            control.Target = eventArgs.NewValue;
        }
    }
}
