using Adenium;
using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal class PerformanceMenuItem : ProfilingMenuItemBase
    {
        public PerformanceMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string Text
        {
            get { return Resources.PerformanceMenuItem_Text; }
        }

        protected override IViewModel GetViewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}
