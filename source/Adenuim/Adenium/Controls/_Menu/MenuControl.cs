using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class MenuControl : System.Windows.Controls.Menu
    {
        public static readonly DependencyProperty SourceProperty;

        static MenuControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuControl), new FrameworkPropertyMetadata(typeof(System.Windows.Controls.Menu)));
            SourceProperty = DependencyProperty.Register("Source", typeof(Menu.IMenu), typeof(MenuControl), new PropertyMetadata(SourcePropertyChanged));
        }

        public Menu.IMenu Source
        {
            get { return (Menu.IMenu)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private void BindViewModel()
        {
            Items.Clear();
            if (Source == null)
            {
                return;
            }
            Visibility = Visibility.Collapsed;
            foreach (Menu.IMenuControl viewModel in Source)
            {
                BindViewModelRecurcive(this, viewModel);
                Visibility = Visibility.Visible;
            }
        }

        private void BindViewModelRecurcive(Control parent, Menu.IMenuControl childViewModel)
        {
            ItemsControl itemsControl = parent as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }
            Control control = MenuControlConverter.Convert(childViewModel);
            itemsControl.Items.Add(control);
            Menu.IMenuControlCollection viewModelCollection = childViewModel as Adenium.Menu.IMenuControlCollection;
            if (viewModelCollection == null)
            {
                return;
            }
            foreach (Menu.IMenuControl viewModel in viewModelCollection)
            {
                BindViewModelRecurcive(control, viewModel);
            }
        }

        private static void SourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            MenuControl menu = (MenuControl)source;
            menu.BindViewModel();
        }
    }
}
