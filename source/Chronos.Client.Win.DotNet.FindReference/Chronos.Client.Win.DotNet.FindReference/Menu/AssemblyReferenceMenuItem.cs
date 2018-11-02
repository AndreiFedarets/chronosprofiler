using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.ViewModels;
using Chronos.Client.Win.Common.ViewModels;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.DotNet.FindReference.ViewModels;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class AssemblyReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IProfilingApplication _application;
        private readonly IFunctionCollection _functions;

        public AssemblyReferenceMenuItem(IProfilingApplication application, IFunctionCollection functions)
        {
            _application = application;
            _functions = functions;
        }

        public override string GetText()
        {
            return Resources.AssemblyReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            EventsTreeViewModel viewModel = (EventsTreeViewModel)_application.MainViewModel.ActivateItem(Common.EventsTree.Constants.ViewModels.EventsTreeViewModel);
            UnitsListViewModel<AssemblyInfo> unitsViewModel = (UnitsListViewModel<AssemblyInfo>)OwnerViewModel;
            AssemblyInfo assemblyInfo = unitsViewModel.SelectedUnit;
            AssemblyEventSearchAdapter adapter = new AssemblyEventSearchAdapter(assemblyInfo, _functions);
            FindReferenceViewModel findReferenceViewModel = viewModel.Parent.FindFirstChild<FindReferenceViewModel>();
            if (findReferenceViewModel != null)
            {
                findReferenceViewModel.BeginSearch(adapter);
            }
        }
    }
}
