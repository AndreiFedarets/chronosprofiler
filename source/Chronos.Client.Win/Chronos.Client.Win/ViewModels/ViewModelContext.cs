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
            ContextMenu = new Menu.Menu();
            Contracts = new ContractCollection(viewModel);
        }

        public Guid TypeId
        {
            get { return _viewModel.GetType().GUID; }
        }

        public Guid InstanceId { get; private set; }

        public ContractCollection Contracts { get; private set; }

        //TODO: I don't like it to be here
        public IMenu ContextMenu { get; private set; }

        void IDisposable.Dispose()
        {
            ContextMenu.TryDispose();
        }
    }
}
