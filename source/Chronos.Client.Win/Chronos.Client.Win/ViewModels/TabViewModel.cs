using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Caliburn.Micro;
using Chronos.Client.Win.Contracts;
using Chronos.Client.Win.Menu;

namespace Chronos.Client.Win.ViewModels
{
    public class TabViewModel : Conductor<IViewModel>.Collection.OneActive, IContainerViewModel
    {
        private readonly ViewModelContext _context;

        public TabViewModel()
        {
            _context = new ViewModelContext(this);
        }

        public Guid TypeId
        {
            get { return _context.TypeId; }
        }

        public Guid InstanceId
        {
            get { return _context.InstanceId; }
        }

        public new IContainerViewModel Parent
        {
            get { return base.Parent as IContainerViewModel; }
        }

        public IMenu ContextMenu
        {
            get { return _context.ContextMenu; }
        }

        protected ContractCollection Contracts
        {
            get { return _context.Contracts; } 
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { Items.CollectionChanged += value; }
            remove { Items.CollectionChanged -= value; }
        }

        public event EventHandler<ViewModelEventArgs> ViewModelAttached;

        public event EventHandler<ViewModelEventArgs> ViewModelDeattached;

        public override void ActivateItem(IViewModel viewModel)
        {
            TabItemViewModel tabItemViewModel = viewModel as TabItemViewModel;
            if (tabItemViewModel != null)
            {
                viewModel = tabItemViewModel.MainViewModel;
            }
            else
            {
                tabItemViewModel = FindTabItemViewModel(viewModel);   
            }
            bool contains = tabItemViewModel != null;
            if (!contains)
            {
                tabItemViewModel = new TabItemViewModel(viewModel);
                tabItemViewModel.ViewModelAttached += OnChildViewModelActivated;
                tabItemViewModel.ViewModelDeattached += OnChildViewModelDeactivated;
                //Register viewModel in TabViewModel contracts scope
                //It will also be registered in scope of TabItemViewModel automatically, later
                Contracts.RegisterItem(viewModel);
            }
            base.ActivateItem(tabItemViewModel);
            if (!contains)
            {
                ViewModelEventArgs.RaiseEvent(ViewModelAttached, this, tabItemViewModel);
                //Activate viewModel on TabItemViewModel level
                tabItemViewModel.ActivateMainViewModel();
            }
        }

        public override void DeactivateItem(IViewModel viewModel, bool close)
        {
            TabItemViewModel tabItemViewModel = viewModel as TabItemViewModel;
            if (tabItemViewModel != null)
            {
                viewModel = tabItemViewModel.MainViewModel;
            }
            else
            {
                tabItemViewModel = FindTabItemViewModel(viewModel);
            }
            bool contains = tabItemViewModel != null;
            if (!contains)
            {
                return;
            }
            if (close)
            {
                //Deactivate viewModel on TabItemViewModel level
                tabItemViewModel.DeactivateMainViewModel();
            }
            base.DeactivateItem(tabItemViewModel, close);
            if (close)
            {
                tabItemViewModel.ViewModelAttached -= OnChildViewModelActivated;
                tabItemViewModel.ViewModelDeattached -= OnChildViewModelDeactivated;
                Contracts.UnregisterItem(viewModel);
            }
            if (close)
            {
                ViewModelEventArgs.RaiseEvent(ViewModelDeattached, this, tabItemViewModel);   
            }
        }

        public IEnumerator<IViewModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private TabItemViewModel FindTabItemViewModel(IViewModel mainViewModel)
        {
            foreach (IViewModel viewModel in Items)
            {
                TabItemViewModel tabItemViewModel = viewModel as TabItemViewModel;
                if (tabItemViewModel != null && Equals(tabItemViewModel.MainViewModel, mainViewModel))
                {
                    return tabItemViewModel;
                }
            }
            return null;
        }

        private void OnChildViewModelActivated(object sender, ViewModelEventArgs e)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelAttached, sender, e);
        }

        private void OnChildViewModelDeactivated(object sender, ViewModelEventArgs e)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelDeattached, sender, e);
        }
    }
}
