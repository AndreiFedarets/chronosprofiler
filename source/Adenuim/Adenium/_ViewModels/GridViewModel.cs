using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    public class GridViewModel : Conductor<IViewModel>.Collection.AllActive, IContainerViewModel
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
            bool contains = Items.Contains(viewModel);
            if (!contains)
            {
                Contracts.RegisterItem(viewModel);
                IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
                if (containerViewModel != null)
                {
                    containerViewModel.ViewModelAttached += OnChildViewModelAttached;
                    containerViewModel.ViewModelDeattached += OnChildViewModelDeattached;
                }
            }
            base.ActivateItem(viewModel);
            if (!contains)
            {
                OnViewModelAttached(viewModel);
            }
        }

        public override void DeactivateItem(IViewModel viewModel, bool close)
        {
            bool contains = Items.Contains(viewModel);
            base.DeactivateItem(viewModel, close);
            if (contains && close)
            {
                Contracts.UnregisterItem(viewModel);
                IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
                if (containerViewModel != null)
                {
                    containerViewModel.ViewModelAttached -= OnChildViewModelAttached;
                    containerViewModel.ViewModelDeattached -= OnChildViewModelDeattached;
                }
                OnViewModelDeattached(viewModel);
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

        private void OnViewModelAttached(IViewModel viewModel)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelAttached, this, viewModel);
        }

        private void OnViewModelDeattached(IViewModel viewModel)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelDeattached, this, viewModel);
        }

        private void OnChildViewModelAttached(object sender, ViewModelEventArgs e)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelAttached, sender, e);
        }

        private void OnChildViewModelDeattached(object sender, ViewModelEventArgs e)
        {
            ViewModelEventArgs.RaiseEvent(ViewModelDeattached, sender, e);
        }
    }
}
