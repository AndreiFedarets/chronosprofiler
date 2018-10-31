using Adenium.Layouting;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class ModuleReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;
        private readonly UnitsListViewModel _unitsViewModel;

        public ModuleReferenceMenuItem(UnitsListViewModel unitsViewModel, IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _unitsViewModel = unitsViewModel;
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string GetText()
        {
            return Resources.ModuleReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            ModuleInfo moduleInfo = (ModuleInfo)_unitsViewModel.SelectedUnit;
        }
    }
}
