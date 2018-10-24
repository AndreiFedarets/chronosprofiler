using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class ContextMenu : System.Windows.Controls.ContextMenu
    {
        public static readonly DependencyProperty SourceProperty;

        static ContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(typeof(System.Windows.Controls.ContextMenu)));
            SourceProperty = DependencyProperty.Register("Source", typeof(Adenium.Menu.IMenu), typeof(ContextMenu), new PropertyMetadata(SourcePropertyChanged));
        }

        public Adenium.Menu.IMenu Source
        {
            get { return (Adenium.Menu.IMenu)GetValue(SourceProperty); }
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
            foreach (Adenium.Menu.IMenuControl viewModel in Source)
            {
                BindViewModelRecurcive(this, viewModel);
                Visibility = Visibility.Visible;
            }
        }

        private void BindViewModelRecurcive(Control parent, Adenium.Menu.IMenuControl childViewModel)
        {
            ItemsControl itemsControl = parent as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }
            Control control = MenuControlConverter.Convert(childViewModel);
            itemsControl.Items.Add(control);
            Adenium.Menu.IMenuControlCollection viewModelCollection = childViewModel as Adenium.Menu.IMenuControlCollection;
            if (viewModelCollection == null)
            {
                return;
            }
            foreach (Adenium.Menu.IMenuControl viewModel in viewModelCollection)
            {
                BindViewModelRecurcive(control, viewModel);
            }
        }

        private static void SourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ContextMenu menu = (ContextMenu)source;
            menu.BindViewModel();
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    //SEARCH_BUTTON_PART
        //    if (_browseButton != null)
        //    {
        //        _browseButton.Click -= OnBrowseButtonClick;
        //    }
        //    _browseButton = GetTemplateChild(BrowseButtonPartName) as Button;
        //    if (_browseButton != null)
        //    {
        //        _browseButton.Click += OnBrowseButtonClick;
        //    }
        //}
    }
}
