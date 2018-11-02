using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.ViewModels;
using Chronos.Client.Win.Common.ViewModels;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.DotNet.FindReference.ViewModels;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class ClassReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IProfilingApplication _application;
        private readonly IFunctionCollection _functions;

        public ClassReferenceMenuItem(IProfilingApplication application, IFunctionCollection functions)
        {
            _application = application;
            _functions = functions;
        }

        public override string GetText()
        {
            return Resources.ClassReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            EventsTreeViewModel viewModel = (EventsTreeViewModel)_application.MainViewModel.ActivateItem(Common.EventsTree.Constants.ViewModels.EventsTreeViewModel);
            UnitsListViewModel<ClassInfo> unitsViewModel = (UnitsListViewModel<ClassInfo>)OwnerViewModel;
            ClassInfo classInfo = unitsViewModel.SelectedUnit;
            ClassEventSearchAdapter adapter = new ClassEventSearchAdapter(classInfo, _functions);
            FindReferenceViewModel findReferenceViewModel = viewModel.Parent.FindFirstChild<FindReferenceViewModel>();
            if (findReferenceViewModel != null)
            {
                findReferenceViewModel.BeginSearch(adapter);
            }
        }
    }
}
