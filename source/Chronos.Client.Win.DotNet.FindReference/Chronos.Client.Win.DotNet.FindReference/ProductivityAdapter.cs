using System;
using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.ViewModels.Common.EventsTree;

namespace Chronos.Client.Win.DotNet.FindReference
{
    public class ProductivityAdapter : IProductivityAdapter, IInitializable, ILayoutProvider, IServiceConsumer
    {
        private static readonly Guid EventTreeUid;
        private IEventsTreeViewModelCollection _eventsTreeViewModels;
        private bool _initialized;

        static ProductivityAdapter()
        {
            EventTreeUid = new Guid("{B3352C62-FCAB-45CA-8EEB-EA296E8C3122}");
        }

        void IInitializable.Initialize(IChronosApplication applicationObject)
        {
            IProfilingApplication application = applicationObject as IProfilingApplication;
            if (application == null)
            {
                return;
            }
            if (!application.ProfilingTypes.Contains(EventTreeUid))
            {
                return;
            }
            _initialized = true;
        }

        void ILayoutProvider.ConfigureContainer(IContainer container)
        {
            container.RegisterInstance(_eventsTreeViewModels);
        }

        string ILayoutProvider.GetLayout(IViewModel viewModel)
        {
            if (!_initialized)
            {
                return string.Empty;
            }
            return LayoutFileReader.ReadViewModelLayout(viewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {

        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            _eventsTreeViewModels = container.Resolve<IEventsTreeViewModelCollection>();
        }

        //private void OnViewModelAttached(object sender, ViewModelEventArgs e)
        //{
        //    UnitsListViewModel unitsViewModel = e.ViewModel as UnitsListViewModel;
        //    if (unitsViewModel != null)
        //    {
        //        InitializeUnitContextMenu(unitsViewModel);
        //    }
        //    EventsTreeViewModel eventsTreeViewModel = e.ViewModel as EventsTreeViewModel;
        //    if (eventsTreeViewModel != null)
        //    {
        //        InitializeFindReferenceViewModel(eventsTreeViewModel);
        //    }
        //}

        //private void InitializeUnitContextMenu(UnitsListViewModel unitsViewModel)
        //{
        //    MenuItem menuItem = null;
        //    if (unitsViewModel.UnitType == typeof(AssemblyInfo))
        //    {
        //        menuItem = new AssemblyReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
        //    }
        //    if (unitsViewModel.UnitType == typeof(ModuleInfo))
        //    {
        //        menuItem = new ModuleReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
        //    }
        //    if (unitsViewModel.UnitType == typeof(ClassInfo))
        //    {
        //        menuItem = new ClassReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
        //    }
        //    if (unitsViewModel.UnitType == typeof(FunctionInfo))
        //    {
        //        menuItem = new FunctionReferenceMenuItem(unitsViewModel, _eventsTreeViewModels);
        //    }
        //    if (menuItem != null)
        //    {
        //        unitsViewModel.Menus[Constants.Menus.ItemContextMenu].Add(menuItem);
        //    }
        //}

        //private void InitializeFindReferenceViewModel(EventsTreeViewModel eventsTreeViewModel)
        //{
        //    IContainerViewModel parentViewModel = eventsTreeViewModel.Parent;
        //    if (parentViewModel == null)
        //    {
        //        return;
        //    }
        //    FindReferenceViewModel findReferenceViewModel = new FindReferenceViewModel(eventsTreeViewModel);
        //    parentViewModel.ActivateItem(findReferenceViewModel);
        //}
    }
}
