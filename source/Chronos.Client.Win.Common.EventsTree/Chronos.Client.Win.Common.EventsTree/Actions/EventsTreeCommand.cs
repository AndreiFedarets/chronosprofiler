using Chronos.Client.Win.Common.EventsTree.Properties;
using Layex.Actions;

namespace Chronos.Client.Win.Common.EventsTree.Actions
{
    public sealed class EventsTreeCommand : ActivateViewModelAction
    {
        public EventsTreeCommand()
            : base(Constants.ViewModels.EventsTreeViewModel)
        {
        }

        public override string DisplayName
        {
            get { return Resources.EventsTreeMenuItem_Text; }
        }
    }
}
