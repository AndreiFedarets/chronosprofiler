using System.Collections.Generic;
using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Common.EventsTree;
using Chronos.Messaging;

namespace Chronos.Client.Win.Common.EventsTree
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, IInitializable, IMessageBusHandler, IServiceConsumer
    {
        private readonly EventsTreeViewModelCollection _eventsTreeViewModels;
        private IProfilingApplication _application;
        private IEventMessageBuilder _eventMessageBuilder;

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
            }
        }

        [MessageHandler(Win.Constants.Message.BuildProfilingViewMenu)]
        internal void BuildProfilingViewMenu(ProfilingViewModel viewModel, List<IMenu> menus)
        {
            ResolutionDependencies dependencies = new ResolutionDependencies();
            _eventsTreeViewModels.Initialize(viewModel, _eventMessageBuilder);
            dependencies.Register<IEventsTreeViewModelCollection>(_eventsTreeViewModels);
            dependencies.Register(viewModel);
            IMenu menu = MenuReader.ReadMenu(Resources.Menu, dependencies);
            menus.Add(menu);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
            container.Register(_eventsTreeViewModels);
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            _eventMessageBuilder = container.Resolve<IEventMessageBuilder>();
        }
    }
}
