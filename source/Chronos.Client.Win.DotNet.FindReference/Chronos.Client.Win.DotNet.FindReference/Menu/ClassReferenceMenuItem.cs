using Adenium.Layouting;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.Common.FindReference
{
    internal sealed class ClassReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;
        private readonly UnitsListViewModel _unitsViewModel;

        public ClassReferenceMenuItem(UnitsListViewModel unitsViewModel, IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _unitsViewModel = unitsViewModel;
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string GetText()
        {
            return Resources.ClassReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            ClassInfo classInfo = (ClassInfo)_unitsViewModel.SelectedUnit;
        }
    }
}
