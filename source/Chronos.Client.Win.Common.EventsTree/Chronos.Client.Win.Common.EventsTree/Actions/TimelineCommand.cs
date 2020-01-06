using Chronos.Client.Win.Common.EventsTree.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.Common.EventsTree.Actions
{
    public sealed class TimelineCommand : ActivateViewModelAction
    {
        public TimelineCommand()
            : base(Constants.ViewModels.EventsTreeViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.EventsTreeMenuItem_Text; }
        }
    }
}
