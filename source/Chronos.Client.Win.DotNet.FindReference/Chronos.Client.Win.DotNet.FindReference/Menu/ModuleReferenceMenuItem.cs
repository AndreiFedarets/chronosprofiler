using Adenium.Menu;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.Common.FindReference
{
    internal sealed class ModuleReferenceMenuItem : MenuItem
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;
        private readonly UnitsListViewModel _unitsViewModel;

        public ModuleReferenceMenuItem(UnitsListViewModel unitsViewModel, IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _unitsViewModel = unitsViewModel;
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string Text
        {
            get { return Resources.ModuleReferenceMenuItem_Text; }
        }

        public override void OnAction()
        {
            ModuleInfo moduleInfo = (ModuleInfo)_unitsViewModel.SelectedUnit;
        }
    }
}
