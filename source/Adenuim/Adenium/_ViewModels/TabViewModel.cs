using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Adenium.Layouting;
using Adenium.ViewModels;
using Caliburn.Micro;

namespace Adenium
{
    public class TabViewModel : Conductor<IViewModel>.Collection.OneActive, IContainerViewModel
    {
        private readonly ViewModelContext _context;

        public TabViewModel()
        {
            _context = new ViewModelContext(this);
        }

        public string ViewModelUid
        {
            get { return _context.ViewModelUid; }
        }

        public Guid InstanceId
        {
            get { return _context.InstanceId; }
        }

        public new IContainerViewModel Parent
        {
            get { return base.Parent as IContainerViewModel; }
        }

        public IMenuCollection Menus
        {
            get { return _context.Menus; }
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
            bool contains;
            if (tabItemViewModel != null)
            {
                viewModel = tabItemViewModel.MainViewModel;
                contains = Items.Contains(tabItemViewModel);
            }
            else
            {
                tabItemViewModel = FindFirstChild<TabItemViewModel>(x => Equals(x.MainViewModel, viewModel));
                contains = tabItemViewModel != null;
            }
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
                OnViewModelAttached(tabItemViewModel);
                //Activate viewModel on TabItemViewModel level
                tabItemViewModel.ActivateMainViewModel();
            }
        }

        public override void DeactivateItem(IViewModel viewModel, bool close)
        {
            TabItemViewModel tabItemViewModel = viewModel as TabItemViewModel;
            bool contains;
            if (tabItemViewModel != null)
            {
                viewModel = tabItemViewModel.MainViewModel;
                contains = Items.Contains(tabItemViewModel);
            }
            else
            {
                tabItemViewModel = FindFirstChild<TabItemViewModel>(x => Equals(x.MainViewModel, viewModel));
                contains = tabItemViewModel != null;
            }
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
                OnViewModelDeattached(tabItemViewModel);
            }
        }

        public void RemoveItems()
        {
            foreach (IViewModel viewModel in Items)
            {
                DeactivateItem(viewModel, true);
            }
        }

        public T FindFirstChild<T>(Func<T, bool> condition = null) where T : IViewModel
        {
            return _context.FindFirstChild(condition);
        }

        public IEnumerator<IViewModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }

        private void OnViewModelAttached(IViewModel viewModel)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelAttached, this, viewModel);
        }

        private void OnViewModelDeattached(IViewModel viewModel)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelDeattached, this, viewModel);
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
