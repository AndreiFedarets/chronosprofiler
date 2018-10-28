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
            SourceProperty = DependencyProperty.Register("Source", typeof(Layouting.IMenu), typeof(ContextMenu), new PropertyMetadata(SourcePropertyChanged));
        }

        public Layouting.IMenu Source
        {
            get { return (Layouting.IMenu)GetValue(SourceProperty); }
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
            foreach (Layouting.IMenuControl viewModel in Source)
            {
                BindViewModelRecurcive(this, viewModel);
                Visibility = Visibility.Visible;
            }
        }

        private void BindViewModelRecurcive(Control parent, Layouting.IMenuControl childViewModel)
        {
            ItemsControl itemsControl = parent as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }
            Control control = MenuControlConverter.Convert(childViewModel);
            itemsControl.Items.Add(control);
            Layouting.IMenuControlCollection viewModelCollection = childViewModel as Layouting.IMenuControlCollection;
            if (viewModelCollection == null)
            {
                return;
            }
            foreach (Layouting.IMenuControl viewModel in viewModelCollection)
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
