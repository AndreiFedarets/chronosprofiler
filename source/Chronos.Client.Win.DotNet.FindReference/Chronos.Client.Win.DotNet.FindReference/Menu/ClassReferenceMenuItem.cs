using Adenium.Menu;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.Common.FindReference
{
    internal sealed class ClassReferenceMenuItem : MenuItem
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;
        private readonly UnitsListViewModel _unitsViewModel;

        public ClassReferenceMenuItem(UnitsListViewModel unitsViewModel, IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _unitsViewModel = unitsViewModel;
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string Text
        {
            get { return Resources.ClassReferenceMenuItem_Text; }
        }

        public override void OnAction()
        {
            ClassInfo classInfo = (ClassInfo)_unitsViewModel.SelectedUnit;
        }
    }
}
