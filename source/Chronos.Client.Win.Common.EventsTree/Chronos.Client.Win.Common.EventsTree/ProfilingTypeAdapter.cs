using System.Collections.Generic;
using Adenium;
using Adenium.Menu;
using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Messaging;

namespace Chronos.Client.Win.Common.EventsTree
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, IInitializable, IMessageBusHandler, IServiceConsumer
    {
        private readonly EventsTreeViewModelCollection _eventsTreeViewModels;
        private IProfilingApplication _application;

        public ProfilingTypeAdapter()
        {
            _eventsTreeViewModels = new EventsTreeViewModelCollection();
        }

        public object CreateSettingsPresentation(ProfilingTypeSettings profilingTypeSettings)
        {
            return null;
        }

        void IInitializable.Initialize(IChronosApplication applicationObject)
        {
            _application = applicationObject as IProfilingApplication;
            if (_application != null)
            {
                _application.MessageBus.Subscribe(this);
                //BuildMenu();
            }
            _eventsTreeViewModels.Initialize(_application);
        }

        //private void BuildMenu()
        //{
        //    ResolutionDependencies dependencies = new ResolutionDependencies();
        //    dependencies.Register(_application);
        //    IMenu menu = MenuReader.ReadMenu(Resources.Menu, dependencies);
        //    _application.MainViewModel.Menus[Constants.Menus.MainMenu].Merge(menu);

        //}

        [MessageHandler(Win.Constants.Message.BuildProfilingViewMenu)]
        internal void BuildProfilingViewMenu(IContainerViewModel viewModel, List<IMenu> menus)
        {
            Container container = new Container();
            container.RegisterInstance(_application);
            MenuReader reader = new MenuReader();
            IMenu menu = reader.ReadMenu(Resources.Menu, container);
            menus.Add(menu);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
            container.Register(_eventsTreeViewModels);
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {

        }
    }
}
