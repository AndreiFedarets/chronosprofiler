using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal class PerformanceMenuItem : ProfilingMenuItemBase
    {
        public PerformanceMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }

        public override string Text
        {
            get { return Resources.PerformanceMenuItem_Text; }
        }

        protected override ViewModels.ViewModel GetViewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}
