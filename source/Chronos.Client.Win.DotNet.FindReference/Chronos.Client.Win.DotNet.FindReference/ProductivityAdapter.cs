using System;
using Adenium;
using Adenium.Menu;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.Menu.Common.FindReference;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Client.Win.ViewModels.DotNet.FindReference;
using Chronos.DotNet.BasicProfiler;
using Chronos.Messaging;

namespace Chronos.Client.Win.DotNet.FindReference
{
    public class ProductivityAdapter : IProductivityAdapter, IInitializable, IMessageBusHandler, IServiceConsumer
    {
        private static readonly Guid BasicProfilerUid;
        private static readonly Guid EventTreeUid;
        private IProfilingApplication _application;
        private IEventsTreeViewModelCollection _eventsTreeViewModels;

        static ProductivityAdapter()
        {
            BasicProfilerUid = new Guid("{3F6FCEF6-8A28-4F44-8F39-51DA5A804724}");
            EventTreeUid = new Guid("{B3352C62-FCAB-45CA-8EEB-EA296E8C3122}");
        }

        public void Initialize(IChronosApplication applicationObject)
        {
            _application = applicationObject as IProfilingApplication;
            if (_application == null)
            {
                return;
            }
            if (!_application.ProfilingTypes.Contains(EventTreeUid))
            {
                return;
            }
            _application.MessageBus.Subscribe(this);
            _application.MainViewModel.ViewModelAttached += OnViewModelAttached;
        }

        private void OnViewModelAttached(object sender, ViewModelEventArgs e)
        {
            UnitsListViewModel unitsViewModel = e.ViewModel as UnitsListViewModel;
            if (unitsViewModel != null)
            {
                InitializeUnitContextMenu(unitsViewModel);
            }
            EventsTreeViewModel eventsTreeViewModel = e.ViewModel as EventsTreeViewModel;
            if (eventsTreeViewModel != null)
            {
                InitializeFindReferenceViewModel(eventsTreeViewModel);
            }
        }

        private void InitializeUnitContextMenu(UnitsListViewModel unitsViewModel)
        {
            MenuItem menuItem = null;
            if (unitsViewModel.UnitType == typeof(AssemblyInfo))
            {
                menuItem = new AssemblyReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
            }
            if (unitsViewModel.UnitType == typeof(ModuleInfo))
            {
                menuItem = new ModuleReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
            }
            if (unitsViewModel.UnitType == typeof(ClassInfo))
            {
                menuItem = new ClassReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
            }
            if (unitsViewModel.UnitType == typeof(FunctionInfo))
            {
                menuItem = new FunctionReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
            }
            if (menuItem != null)
            {
                unitsViewModel.Menus[Constants.Menus.ItemContextMenu].Add(menuItem);
            }
        }

        private void InitializeFindReferenceViewModel(EventsTreeViewModel eventsTreeViewModel)
        {
            IContainerViewModel parentViewModel = eventsTreeViewModel.Parent;
            if (parentViewModel == null)
            {
                return;
            }
            FindReferenceViewModel findReferenceViewModel = new FindReferenceViewModel(eventsTreeViewModel);
            parentViewModel.ActivateItem(findReferenceViewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {

        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            _eventsTreeViewModels = container.Resolve<IEventsTreeViewModelCollection>();
        }
    }
}
