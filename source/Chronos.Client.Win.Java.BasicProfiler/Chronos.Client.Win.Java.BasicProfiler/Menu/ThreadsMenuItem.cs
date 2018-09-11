using Chronos.Client.Win.Java.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.Java.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Java.BasicProfiler;

namespace Chronos.Client.Win.Menu.Java.BasicProfiler
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
            //IThreadCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IThreadCollection>();
            //ThreadsModel model = new ThreadsModel(collection);
            //return model;
            return null;
        }
    }
}
