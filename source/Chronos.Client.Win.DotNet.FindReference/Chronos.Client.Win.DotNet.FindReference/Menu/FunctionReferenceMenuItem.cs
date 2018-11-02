using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.ViewModels;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class FunctionReferenceMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.FunctionReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            IContainerViewModel containerViewModel = (IContainerViewModel) OwnerViewModel;
            IViewModel viewModel = containerViewModel.ActivateItem(Common.EventsTree.Constants.ViewModels.EventsTreeViewModel);
            UnitsListViewModel<FunctionInfo> unitsViewModel = (UnitsListViewModel<FunctionInfo>)OwnerViewModel;
            FunctionInfo functionInfo = unitsViewModel.SelectedUnit;
            FunctionEventSearchAdapter adapter = new FunctionEventSearchAdapter(functionInfo);
            //IEventsTreeViewModel viewModel = _eventsTreeViewModels.Open();
            //FindReferenceViewModel findReferenceViewModel = viewModel.Parent.FindFirstChild<FindReferenceViewModel>();
            //if (findReferenceViewModel != null)
            //{
            //    findReferenceViewModel.BeginSearch(adapter);
            //}
            //viewModel.EventSearch.BeginSearch(adapter);
        }
    }
}
