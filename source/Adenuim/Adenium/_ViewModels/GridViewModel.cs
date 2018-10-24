using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Adenium.Menu;
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
                    containerViewModel.ViewModelAttached += OnChildViewModelActivated;
                    containerViewModel.ViewModelDeattached += OnChildViewModelDeactivated;
                }
            }
            base.ActivateItem(viewModel);
            if (!contains)
            {
                ViewModelEventArgs.RaiseEvent(ViewModelAttached, this, viewModel);
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
                    containerViewModel.ViewModelAttached -= OnChildViewModelActivated;
                    containerViewModel.ViewModelDeattached -= OnChildViewModelDeactivated;
                }
                ViewModelEventArgs.RaiseEvent(ViewModelDeattached, this, viewModel);
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
