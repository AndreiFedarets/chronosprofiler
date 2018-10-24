using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Adenium
{
    public sealed class TabViewBehaviorExtension : IViewBehaviorExtension
    {
        private readonly View _view;
        private readonly TabViewModel _viewModel;

        public TabViewBehaviorExtension(View view, TabViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            BuildLayout();
        }

        public void Dispose()
        {
            ClearLayout();
        }

        private void ClearLayout()
        {
            ContentControl contentControl = ViewsManager.FindViewContent(_view);
            if (contentControl.Content == null)
            {
                return;
            }
            TabControl control = (TabControl)contentControl.Content;
            contentControl.Content = null;
            control.Items.Clear();
        }

        private void BuildLayout()
        {
            TabControl tabControl = new TabControl();
            tabControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            tabControl.VerticalAlignment = VerticalAlignment.Stretch;

            //Set templates
            tabControl.ItemTemplate = (DataTemplate)Application.Current.Resources["TabViewHeaderDataTempate"];
            tabControl.ContentTemplate = (DataTemplate)Application.Current.Resources["TabViewContentDataTempate"];

            //Set DataContext
            tabControl.DataContext = new TabViewControlDataContext(_viewModel);

            //Set ItemsSource binding
            Binding itemsSourceBinding = new Binding("ViewModel");
            itemsSourceBinding.Mode = BindingMode.OneWay;
            tabControl.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);

            //Set TabControl.SelectedItem <-> TabViewModel.ActiveItem binding
            Binding selectedItemBinding = new Binding("ViewModel.ActiveItem");
            selectedItemBinding.Mode = BindingMode.TwoWay;
            tabControl.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);

            //Put TabControl on view
            ContentControl contentControl = ViewsManager.FindViewContent(_view);
            contentControl.Content = tabControl;
        }
    }
}
