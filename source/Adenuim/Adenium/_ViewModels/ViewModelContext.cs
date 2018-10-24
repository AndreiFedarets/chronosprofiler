using System;
using Adenium.Menu;

namespace Adenium
{
    internal sealed class ViewModelContext : IDisposable
    {
        private readonly IViewModel _viewModel;

        public ViewModelContext(IViewModel viewModel)
        {
            _viewModel = viewModel;
            InstanceId = Guid.NewGuid();
            Menus = new MenuCollection();
            Contracts = new ContractCollection(viewModel);
        }

        public Guid TypeId
        {
            get { return _viewModel.GetType().GUID; }
        }

        public Guid InstanceId { get; private set; }

        public ContractCollection Contracts { get; private set; }

        public IMenuCollection Menus { get; private set; }

        public ContainerViewModelLayout GetViewModelLayout()
        {
            Type viewModelType = _viewModel.GetType();
            ContainerViewModelLayout layout = LayoutProvider.Current.GetViewModelLayout(viewModelType);
            return layout;
        }

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
            Menus.Dispose();
        }
    }
}
