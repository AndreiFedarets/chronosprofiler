using System;
using Chronos.Client.Win.Contracts;
using Chronos.Client.Win.Menu;

namespace Chronos.Client.Win.ViewModels
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

        void IDisposable.Dispose()
        {
            Menus.TryDispose();
        }
    }
}
