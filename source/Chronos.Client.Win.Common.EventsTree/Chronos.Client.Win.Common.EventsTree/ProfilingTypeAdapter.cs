using Adenium;
using Adenium.Layouting;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, ILayoutProvider, IServiceConsumer
    {
        private IEventTreeCollection _eventTrees;
        private IEventMessageBuilder _eventMessageBuilder;

        void ILayoutProvider.ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
            container.RegisterInstance(_eventTrees);
            container.RegisterInstance(_eventMessageBuilder);
            container.RegisterInstance(_eventMessageBuilder);
        }

        string ILayoutProvider.GetLayout(IViewModel targetViewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            _eventTrees = container.Resolve<IEventTreeCollection>();
            _eventMessageBuilder = container.Resolve<IEventMessageBuilder>();
        }
    }
}
