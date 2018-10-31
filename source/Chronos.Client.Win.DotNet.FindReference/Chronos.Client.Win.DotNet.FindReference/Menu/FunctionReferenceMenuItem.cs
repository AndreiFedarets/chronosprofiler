using Adenium.Layouting;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Client.Win.ViewModels.DotNet.FindReference;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class FunctionReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;
        private readonly UnitsListViewModel _unitsViewModel;

        public FunctionReferenceMenuItem(UnitsListViewModel unitsViewModel, IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _unitsViewModel = unitsViewModel;
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string GetText()
        {
            return Resources.FunctionReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            FunctionInfo functionInfo = (FunctionInfo) _unitsViewModel.SelectedUnit;
            FunctionEventSearchAdapter adapter = new FunctionEventSearchAdapter(functionInfo);
            IEventsTreeViewModel viewModel = _eventsTreeViewModels.Open();
            FindReferenceViewModel findReferenceViewModel = viewModel.Parent.FindFirstChild<FindReferenceViewModel>();
            if (findReferenceViewModel != null)
            {
                findReferenceViewModel.BeginSearch(adapter);
            }
            //viewModel.EventSearch.BeginSearch(adapter);
        }
    }
}
