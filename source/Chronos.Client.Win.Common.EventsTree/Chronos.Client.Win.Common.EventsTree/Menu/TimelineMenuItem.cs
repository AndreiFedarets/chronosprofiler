using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal sealed class TimelineMenuItem : ProfilingMenuItemBase
    {
        public TimelineMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string Text
        {
            get { return Resources.TimelineMenuItem_Text; }
        }

        protected override IViewModel GetViewModel()
        {
            IEventTreeCollection eventTrees = Application.ServiceContainer.Resolve<IEventTreeCollection>();
            IProfilingTimer profilingTimer = Application.ServiceContainer.Resolve<IProfilingTimer>();
            IEventMessageBuilder messageBuilder = Application.ServiceContainer.Resolve<IEventMessageBuilder>();
            IEventsTreeViewModelCollection eventsTreeViewModels = Application.ServiceContainer.Resolve<IEventsTreeViewModelCollection>();
            return new TimelineViewModel(eventTrees, profilingTimer, messageBuilder, eventsTreeViewModels);
        }
    }
}
