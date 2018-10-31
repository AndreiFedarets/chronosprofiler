using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.ViewModels.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, IInitializable, ILayoutProvider, IServiceConsumer
    {
        private readonly EventsTreeViewModelCollection _eventsTreeViewModels;

        public ProfilingTypeAdapter()
        {
            _eventsTreeViewModels = new EventsTreeViewModelCollection();
        }

        void IInitializable.Initialize(IChronosApplication applicationObject)
        {
            IProfilingApplication application = applicationObject as IProfilingApplication;
            if (application != null)
            {
                _eventsTreeViewModels.Initialize(application);
            }
        }

        void ILayoutProvider.ConfigureContainer(IContainer container)
        {
        }

        string ILayoutProvider.GetLayout(IViewModel viewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(viewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
            container.Register(_eventsTreeViewModels);
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {

        }

        //private void BuildMenu()
        //{
        //    ResolutionDependencies dependencies = new ResolutionDependencies();
        //    dependencies.Register(_application);
        //    IMenu menu = MenuReader.ReadMenu(Resources.Menu, dependencies);
        //    _application.MainViewModel.Menus[Constants.Menus.MainMenu].Merge(menu);

        //}

        //[MessageHandler(Win.Constants.Message.BuildProfilingViewMenu)]
        //internal void BuildProfilingViewMenu(IContainerViewModel viewModel, List<IMenu> menus)
        //{
        //    Container container = new Container();
        //    container.RegisterInstance(_application);
        //    MenuReader reader = new MenuReader();
        //    IMenu menu = reader.ReadMenu(Resources.Menu, container);
        //    menus.Add(menu);
        //}
    }
}
