using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.Common.FindReference
{
    internal sealed class AssemblyReferenceMenuItem : MenuItem
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;
        private readonly UnitsViewModel _unitsViewModel;

        public AssemblyReferenceMenuItem(UnitsViewModel unitsViewModel, IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _unitsViewModel = unitsViewModel;
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string Text
        {
            get { return Resources.AssemblyReferenceMenuItem_Text; }
        }

        public override void OnAction()
        {
            AssemblyInfo assemblyInfo = (AssemblyInfo)_unitsViewModel.SelectedUnit;
        }
    }
}
