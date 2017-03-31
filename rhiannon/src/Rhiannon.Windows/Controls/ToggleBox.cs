using System.Windows;
using System.Windows.Controls;

namespace Rhiannon.Windows.Controls
{
	public class ToggleBox : ComboBox
	{
		public static readonly DependencyProperty IsCheckedProperty;

		static ToggleBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleBox), new FrameworkPropertyMetadata(typeof(ToggleBox)));
			IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof (bool), typeof (ToggleBox));
		}

		public bool IsChecked
		{
			get { return (bool) GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}
	}
}
