using System;
using Chronos.Client.Win.Menu.Common.FindReference;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
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
            if (!_application.ProfilingTypes.Contains(EventTreeUid) || !_application.ProfilingTypes.Contains(BasicProfilerUid))
            {
                return;
            }
            _application.MessageBus.Subscribe(this);
            _application.MainViewModel.ViewModelAttached += OnViewModelAttached;
        }

        private void OnViewModelAttached(object sender, ViewModelEventArgs e)
        {
            UnitsViewModel unitsViewModel = e.ViewModel as UnitsViewModel;
            if (unitsViewModel == null)
            {
                return;
            }
            if (unitsViewModel.UnitType == typeof(AssemblyInfo))
            {
                unitsViewModel.ContextMenu.Add(new AssemblyReferenceMenuItem(unitsViewModel, _eventsTreeViewModels));
            }
            if (unitsViewModel.UnitType == typeof(ModuleInfo))
            {
                unitsViewModel.ContextMenu.Add(new ModuleReferenceMenuItem(unitsViewModel, _eventsTreeViewModels));
            }
            if (unitsViewModel.UnitType == typeof(ClassInfo))
            {
                unitsViewModel.ContextMenu.Add(new ClassReferenceMenuItem(unitsViewModel, _eventsTreeViewModels));
            }
            if (unitsViewModel.UnitType == typeof(FunctionInfo))
            {
                unitsViewModel.ContextMenu.Add(new FunctionReferenceMenuItem(unitsViewModel, _eventsTreeViewModels));
            }
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
