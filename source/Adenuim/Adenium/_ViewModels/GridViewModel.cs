using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    public class GridViewModel : Conductor<IViewModel>.Collection.AllActive, IContainerViewModel, IHaveLayout, IHaveScope
    {
        private readonly ViewModelContext _context;

        public GridViewModel()
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
            bool contains = Items.Contains(viewModel);
            if (!contains)
            {
                Contracts.RegisterItem(viewModel);
                IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
                if (containerViewModel != null)
                {
                    containerViewModel.ItemAttached += OnChildViewModelAttached;
                    containerViewModel.ItemDeattached += OnChildViewModelDeattached;
                }
            }
            base.ActivateItem(viewModel);
            if (!contains)
            {
                OnItemAttached(viewModel);
            }
        }

        public override void DeactivateItem(IViewModel viewModel, bool close)
        {
            if (viewModel == null)
            {
                return;
            }
            bool contains = Items.Contains(viewModel);
            base.DeactivateItem(viewModel, close);
            if (contains && close)
            {
                Contracts.UnregisterItem(viewModel);
                IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
                if (containerViewModel != null)
                {
                    containerViewModel.ItemAttached -= OnChildViewModelAttached;
                    containerViewModel.ItemDeattached -= OnChildViewModelDeattached;
                }
                OnItemDeattached(viewModel);
            }
        }

        public void RemoveItems()
        {
            List<IViewModel> viewModels = new List<IViewModel>(Items);
            foreach (IViewModel viewModel in viewModels)
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

        public void Dispose()
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

        private void OnChildViewModelAttached(object sender, ViewModelEventArgs e)
        {
            ViewModelEventArgs.RaiseEvent(ItemAttached, sender, e);
        }

        private void OnChildViewModelDeattached(object sender, ViewModelEventArgs e)
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
