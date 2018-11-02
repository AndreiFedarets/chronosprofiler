using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.ViewModels;
using Chronos.Client.Win.Common.ViewModels;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.DotNet.FindReference.ViewModels;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class FunctionReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IProfilingApplication _application;

        public FunctionReferenceMenuItem(IProfilingApplication application)
        {
            _application = application;
        }

        public override string GetText()
        {
            return Resources.FunctionReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            EventsTreeViewModel viewModel = (EventsTreeViewModel)_application.MainViewModel.ActivateItem(Common.EventsTree.Constants.ViewModels.EventsTreeViewModel);
            UnitsListViewModel<FunctionInfo> unitsViewModel = (UnitsListViewModel<FunctionInfo>)OwnerViewModel;
            FunctionInfo functionInfo = unitsViewModel.SelectedUnit;
            FunctionEventSearchAdapter adapter = new FunctionEventSearchAdapter(functionInfo);
            FindReferenceViewModel findReferenceViewModel = viewModel.Parent.FindFirstChild<FindReferenceViewModel>();
            if (findReferenceViewModel != null)
            {
                findReferenceViewModel.BeginSearch(adapter);
            }
        }
    }
}
