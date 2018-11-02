using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    public class TabViewModel : Conductor<IViewModel>.Collection.OneActive, IContainerViewModel, IHaveLayout, IHaveScope
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

        public IContainerViewModel LogicalParent
        {
            get { return _context.LogicalParent; }
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

        public event EventHandler<ViewModelEventArgs> ItemAttached;

        public event EventHandler<ViewModelEventArgs> ItemDeattached;

        public override void ActivateItem(IViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }
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
                tabItemViewModel = ViewModelManager.Instance.CreateViewModel<TabItemViewModel, IViewModel>(this, viewModel);
                tabItemViewModel.ItemAttached += OnChildViewModelActivated;
                tabItemViewModel.ItemDeattached += OnChildViewModelDeactivated;
                //Register viewModel in TabViewModel contracts scope
                //It will also be registered in scope of TabItemViewModel automatically, later
                Contracts.RegisterItem(viewModel);
            }
            base.ActivateItem(tabItemViewModel);
            if (!contains)
            {
                OnItemAttached(tabItemViewModel);
                //Activate viewModel on TabItemViewModel level
                tabItemViewModel.ActivateMainViewModel();
            }
        }

        public override void DeactivateItem(IViewModel viewModel, bool close)
        {
            if (viewModel == null)
            {
                return;
            }
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
                tabItemViewModel.ItemAttached -= OnChildViewModelActivated;
                tabItemViewModel.ItemDeattached -= OnChildViewModelDeactivated;
                Contracts.UnregisterItem(viewModel);
            }
            if (close)
            {
                OnItemDeattached(tabItemViewModel);
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

        private void OnItemAttached(IViewModel viewModel)
        {
            ViewModelManager.Instance.BuildViewModelLayout(viewModel);
            ViewModelEventArgs.RaiseEvent(ItemAttached, this, viewModel);
        }

        private void OnItemDeattached(IViewModel viewModel)
        {
            ViewModelManager.Instance.ResetViewModelLayout(viewModel);
            ViewModelEventArgs.RaiseEvent(ItemDeattached, this, viewModel);
        }

        private void OnChildViewModelActivated(object sender, ViewModelEventArgs e)
        {
            ViewModelEventArgs.RaiseEvent(ItemAttached, sender, e);
        }

        private void OnChildViewModelDeactivated(object sender, ViewModelEventArgs e)
        {
            ViewModelEventArgs.RaiseEvent(ItemDeattached, sender, e);
        }

        ViewModelLayout IHaveLayout.Layout
        {
            get { return _context.Layout; }
        }

        void IHaveLayout.AssignLayout(ViewModelLayout layout)
        {
            _context.AssignLayout(layout);
        }

        IContainer IHaveScope.ScopeContainer
        {
            get { return _context.ScopeContainer; }
        }

        void IHaveScope.AssignScopeContainer(IContainer container)
        {
            ConfigureScopeContainer(container);
            _context.AssignScopeContainer(container);
        }

        protected virtual void ConfigureScopeContainer(IContainer container)
        {

        }
    }
}
