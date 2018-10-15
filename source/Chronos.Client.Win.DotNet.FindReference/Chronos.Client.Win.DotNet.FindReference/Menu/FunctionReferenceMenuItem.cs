using Chronos.Client.Win.DotNet.FindReference;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.Common.FindReference
{
    internal sealed class FunctionReferenceMenuItem : MenuItem
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;
        private readonly UnitsViewModel _unitsViewModel;

        public FunctionReferenceMenuItem(UnitsViewModel unitsViewModel, IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _unitsViewModel = unitsViewModel;
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string Text
        {
            get { return Resources.FunctionReferenceMenuItem_Text; }
        }

        public override void OnAction()
        {
            FunctionInfo functionInfo = (FunctionInfo) _unitsViewModel.SelectedUnit;
            FunctionEventSearchAdapter adapter = new FunctionEventSearchAdapter(functionInfo);
            IEventsTreeViewModel viewModel = _eventsTreeViewModels.Open();
            viewModel.EventSearch.BeginSearch(adapter);
        }
    }
}
