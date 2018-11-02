using System;
using Adenium.Layouting;

namespace Adenium
{
    internal sealed class ViewModelContext : IDisposable, IHaveLayout, IHaveScope
    {
        private readonly IViewModel _viewModel;

        public ViewModelContext(IViewModel viewModel)
        {
            _viewModel = viewModel;
            InstanceId = Guid.NewGuid();
            Menus = new MenuCollection();
            Contracts = new ContractCollection(viewModel);
        }

        public string ViewModelUid
        {
            get
            {
                string viewModelUid;
                ViewModelAttribute attribute = ViewModelAttribute.GetAttribute(_viewModel);
                if (attribute != null)
                {
                    viewModelUid = attribute.ViewModelUid;
                }
                else
                {
                    viewModelUid = _viewModel.GetType().FullName;
                }
                return viewModelUid.ToLowerInvariant();
            }
        }

        public Guid InstanceId { get; private set; }

        public ContractCollection Contracts { get; private set; }

        public IMenuCollection Menus { get; private set; }

        public ViewModelLayout Layout { get; private set; }

        public IContainerViewModel LogicalParent
        {
            get
            {
                IContainerViewModel logicalParent = _viewModel.Parent;
                TabItemViewModel tabItemViewModel = logicalParent as TabItemViewModel;
                if (tabItemViewModel != null)
                {
                    logicalParent = tabItemViewModel.Parent;
                }
                return logicalParent;
            }
        }

        public IContainer ScopeContainer { get; private set; }

        public T FindFirstChild<T>(Func<T, bool> condition) where T : IViewModel
        {
            IContainerViewModel containerViewModel = _viewModel as IContainerViewModel;
            if (containerViewModel == null)
            {
                return default(T);
            }
            foreach (IViewModel viewModel in containerViewModel)
            {
                if (!(viewModel is T))
                {
                    continue;
                }
                if (condition == null || condition((T)viewModel))
                {
                    return (T)viewModel;
                }
            }
            return default(T);
        }

        public void Dispose()
        {
            ((IDisposable)Menus).Dispose();
        }

        public void AssignLayout(ViewModelLayout layout)
        {
            Layout = layout;
        }

        public void AssignScopeContainer(IContainer container)
        {
            ScopeContainer = container;
        }

    }
}
