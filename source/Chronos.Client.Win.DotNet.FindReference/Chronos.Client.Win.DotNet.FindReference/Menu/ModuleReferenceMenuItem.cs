using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.ViewModels;
using Chronos.Client.Win.Common.ViewModels;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.DotNet.FindReference.ViewModels;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class ModuleReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IProfilingApplication _application;
        private readonly IFunctionCollection _functions;

        public ModuleReferenceMenuItem(IProfilingApplication application, IFunctionCollection functions)
        {
            _application = application;
            _functions = functions;
        }

        public override string GetText()
        {
            return Resources.ModuleReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            EventsTreeViewModel viewModel = (EventsTreeViewModel)_application.MainViewModel.ActivateItem(Common.EventsTree.Constants.ViewModels.EventsTreeViewModel);
            UnitsListViewModel<ModuleInfo> unitsViewModel = (UnitsListViewModel<ModuleInfo>)OwnerViewModel;
            ModuleInfo moduleInfo = unitsViewModel.SelectedUnit;
            ModuleEventSearchAdapter adapter = new ModuleEventSearchAdapter(moduleInfo, _functions);
            FindReferenceViewModel findReferenceViewModel = viewModel.Parent.FindFirstChild<FindReferenceViewModel>();
            if (findReferenceViewModel != null)
            {
                findReferenceViewModel.BeginSearch(adapter);
            }
        }
    }
}
