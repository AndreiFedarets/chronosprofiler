using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal sealed class EventsTreeMenuItem : ProfilingMenuItemBase
    {
        public EventsTreeMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }

        public override string Text
        {
            get { return Resources.EventsTreeMenuItem_Text; }
        }

        protected override ViewModels.ViewModel GetViewModel()
        {
            IProfilingApplication application = ProfilingViewModel.Application;
            IEventTreeCollection eventTrees = application.ServiceContainer.Resolve<IEventTreeCollection>();
            IEventMessageBuilder messageBuilder = application.ServiceContainer.Resolve<IEventMessageBuilder>();
            return new EventsTreeViewModel(eventTrees, messageBuilder);
        }
    }
}
