using Layex.Actions;

namespace Chronos.Client.Win.DotNet.SqlProfiler.Actions
{
    public sealed class SqlQueriesCommand : ActivateViewModelAction
    {
        public SqlQueriesCommand()
            : base(Constants.ViewModels.SqlQueriesViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Properties.Resources.SqlQueriesMenuItem_Text; }
        }
    }
}
