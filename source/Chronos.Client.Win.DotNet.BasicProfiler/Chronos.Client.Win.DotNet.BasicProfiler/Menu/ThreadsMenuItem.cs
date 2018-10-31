using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class ThreadsMenuItem : UnitsMenuItemBase
    {
        public ThreadsMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string GetText()
        {
            return Resources.ThreadsMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            IThreadCollection collection = Application.ServiceContainer.Resolve<IThreadCollection>();
            ThreadsModel model = new ThreadsModel(collection);
            return model;
        }
    }
}
