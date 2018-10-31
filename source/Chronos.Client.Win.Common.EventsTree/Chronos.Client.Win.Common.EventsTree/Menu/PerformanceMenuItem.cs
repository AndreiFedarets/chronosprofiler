using Adenium;
using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.Menu.Specialized;

namespace Chronos.Client.Win.Common.EventsTree.Menu
{
    internal class PerformanceMenuItem : ProfilingMenuItemBase
    {
        public PerformanceMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string GetText()
        {
            return Resources.PerformanceMenuItem_Text;
        }

        protected override IViewModel GetViewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}
