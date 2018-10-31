using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree.Menu
{
    internal sealed class EventsTreeMenuItem : MenuControlHandlerBase
    {
        private readonly IProfilingApplication _application;

        public EventsTreeMenuItem(IProfilingApplication application)
        {
            _application = application;
        }

        public override string GetText()
        {
            return Resources.EventsTreeMenuItem_Text;
        }

        public override void OnAction()
        {
            IEventsTreeViewModelCollection viewModels = _application.ServiceContainer.Resolve<IEventsTreeViewModelCollection>();
            viewModels.Open();
        }
    }
}
