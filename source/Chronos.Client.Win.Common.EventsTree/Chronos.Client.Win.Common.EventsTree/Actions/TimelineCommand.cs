using Chronos.Client.Win.Common.EventsTree.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.Common.EventsTree.Actions
{
    public sealed class TimelineCommand : ActivateViewModelAction
    {
        public TimelineCommand()
            : base(Constants.ViewModels.TimelineViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.TimelineMenuItem_Text; }
        }
    }
}
