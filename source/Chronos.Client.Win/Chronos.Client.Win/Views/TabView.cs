using Chronos.Client.Win.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Chronos.Client.Win.Views
{
    public class TabView : PageView
    {
        private readonly TabControlContext _context;

        public TabView()
        {
            _context = new TabControlContext();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            CreateTabControl();
        }

        protected override void OnViewModelChanged(ViewModel oldValue, ViewModel newValue)
        {
            TabViewModel newViewModel = (TabViewModel)newValue;
            _context.Initialize(newViewModel);
        }

        private void CreateTabControl()
        {
            System.Windows.Controls.TabControl tabControl = new System.Windows.Controls.TabControl();
            tabControl.DataContext = _context;

            //Set templates
            tabControl.ItemTemplate = (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["TabViewHeaderDataTempate"];
            tabControl.ContentTemplate = (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["TabViewContentDataTempate"];

            //Set ItemsSource binding
            Binding itemsSourceBinding = new Binding(".");
            tabControl.SetBinding(System.Windows.Controls.ItemsControl.ItemsSourceProperty, itemsSourceBinding);

            //Set SelectedItem binding
            Binding selectedItemBinding = new Binding("ActiveItem");
            selectedItemBinding.Mode = BindingMode.TwoWay;
            tabControl.SetBinding(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, selectedItemBinding);

            //Put TabControl on view
            System.Windows.Controls.ContentControl contentControl = ViewsManager.FindViewContent(this);
            contentControl.Content = tabControl;
        }

        public class TabControlContext : ObservableCollection<TabItemViewModel>
        {
            private TabViewModel _viewModel;
            private TabItemViewModel _activeItem;

            public TabItemViewModel ActiveItem
            {
                get { return _activeItem; }
                set 
                {
                    _activeItem = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ActiveItem"));
                }
            }

            internal void Initialize(TabViewModel viewModel)
            {
                if (_viewModel != null)
                {
                    _viewModel.CollectionChanged -= OnViewModelCollectionChanged;
                    _viewModel.ActiveViewModelChanged -= OnViewModelActiveViewModelChanged;
                }
                _viewModel = viewModel;
                if (_viewModel != null)
                {
                    _viewModel.CollectionChanged += OnViewModelCollectionChanged;
                    _viewModel.ActiveViewModelChanged += OnViewModelActiveViewModelChanged;
                    foreach (ViewModel item in _viewModel)
                    {
                        AddItem(item);
                    }
                }
            }

            private void OnViewModelActiveViewModelChanged(object sender, EventArgs e)
            {
                ViewModel activeViewModel = _viewModel.ActiveViewModel;
                TabItemViewModel item = FindTabItemViewModel(activeViewModel);
                ActiveItem = item;
            }

            private void OnViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (ViewModel item in e.NewItems)
                    {
                        AddItem(item);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (ViewModel item in e.OldItems)
                    {
                        RemoveItem(item);
                    }
                }
            }

            private void AddItem(ViewModel viewModel)
            {
                TabItemViewModel item = new TabItemViewModel(viewModel, _viewModel);
                Add(item);
            }

            private void RemoveItem(ViewModel viewModel)
            {
                TabItemViewModel item = FindTabItemViewModel(viewModel);
                if (item != null)
                {
                    Remove(item);
                }
            }

            private TabItemViewModel FindTabItemViewModel(ViewModel viewModel)
            {
                TabItemViewModel item = this.FirstOrDefault(x => x.UnderlyingViewModel == viewModel);
                return item;
            }
        }
    }
}
