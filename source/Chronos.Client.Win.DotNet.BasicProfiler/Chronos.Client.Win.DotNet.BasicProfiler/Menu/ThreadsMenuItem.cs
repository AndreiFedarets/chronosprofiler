using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class ThreadsMenuItem : UnitsMenuItemBase
    {
        public ThreadsMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }

        public override string Text
        {
            get { return Resources.ThreadsMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IThreadCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IThreadCollection>();
            ThreadsModel model = new ThreadsModel(collection);
            return model;
        }
    }
}
