using System.Windows;
using System.Windows.Controls;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
    public abstract class UnitInfoControl<T> : Control where T : UnitBase
    {
        private static readonly DependencyPropertyKey UnitNamePropertyKey;
        public static readonly DependencyProperty UnitProperty;
        public static readonly DependencyProperty UnitNameProperty;

        static UnitInfoControl()
        {
            UnitProperty = DependencyProperty.Register("Unit", typeof(T), typeof(UnitInfoControl<T>), new PropertyMetadata(default(T), OnUnitPropertyChanged));
            UnitNamePropertyKey = DependencyProperty.RegisterReadOnly("UnitName", typeof(string), typeof(UnitInfoControl<T>), new PropertyMetadata(string.Empty));
            UnitNameProperty = UnitNamePropertyKey.DependencyProperty;
        }

        public T Unit
        {
            get { return (T)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public string UnitName
        {
            get { return (string)GetValue(UnitNameProperty); }
            protected set { SetValue(UnitNamePropertyKey, value); }
        }

        private static void OnUnitPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            UnitInfoControl<T> control = (UnitInfoControl<T>) sender;
            T unit = (T) eventArgs.NewValue;
            control.OnUnitPropertyChanged(unit);
        }

        protected virtual void OnUnitPropertyChanged(T unit)
        {
            UnitName = unit.Name;
        }
    }
}
