using System.Windows;
using Layex;
using Layex.Layouts;
using Layex.ViewModels;

namespace Chronos.Client.Win
{
    internal sealed class Bootstrapper : BootstrapperBase
    {
        private readonly IDependencyContainer _container;
        private readonly ILayoutProvider _layoutProvider;
        private readonly string _mainViewModelName;

        public Bootstrapper(IDependencyContainer container, ILayoutProvider layoutProvider, string mainViewModelName)
        {
            _container = container;
            _layoutProvider = layoutProvider;
            _mainViewModelName = mainViewModelName;
        }

        protected override IDependencyContainer CreateDependencyContainer()
        {
            return _container;
        }

        protected override ILayoutProvider CreateLayoutProvider()
        {
            return _layoutProvider;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            IViewModelManager viewModelManager = _container.Resolve<IViewModelManager>();
            viewModelManager.Activate(_mainViewModelName);
        }
    }
}
