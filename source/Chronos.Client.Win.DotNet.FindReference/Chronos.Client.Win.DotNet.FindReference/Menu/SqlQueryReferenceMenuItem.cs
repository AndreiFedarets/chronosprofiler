using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.ViewModels;
using Chronos.Client.Win.Common.ViewModels;
using Chronos.Client.Win.DotNet.FindReference.Properties;
using Chronos.Client.Win.DotNet.FindReference.ViewModels;
using Chronos.DotNet.SqlProfiler;

namespace Chronos.Client.Win.DotNet.FindReference.Menu
{
    internal sealed class SqlQueryReferenceMenuItem : MenuControlHandlerBase
    {
        private readonly IProfilingApplication _application;

        public SqlQueryReferenceMenuItem(IProfilingApplication application)
        {
            _application = application;
        }

        public override string GetText()
        {
            return Resources.SqlQueryReferenceMenuItem_Text;
        }

        public override void OnAction()
        {
            EventsTreeViewModel viewModel = (EventsTreeViewModel)_application.MainViewModel.ActivateItem(Common.EventsTree.Constants.ViewModels.EventsTreeViewModel);
            UnitsListViewModel<SqlQueryInfo> unitsViewModel = (UnitsListViewModel<SqlQueryInfo>)OwnerViewModel;
            SqlQueryInfo sqlQueryInfo = unitsViewModel.SelectedUnit;
            SqlQueryEventSearchAdapter adapter = new SqlQueryEventSearchAdapter(sqlQueryInfo);
            FindReferenceViewModel findReferenceViewModel = viewModel.Parent.FindFirstChild<FindReferenceViewModel>();
            if (findReferenceViewModel != null)
            {
                findReferenceViewModel.BeginSearch(adapter);
            }
        }
    }
}
