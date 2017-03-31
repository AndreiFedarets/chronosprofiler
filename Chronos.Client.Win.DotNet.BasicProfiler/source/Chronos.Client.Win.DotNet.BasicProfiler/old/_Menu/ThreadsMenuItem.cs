using Chronos.Client.Win.DotNet.BasicProfiler.Models;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class ThreadsMenuItem : UnitsMenuAdapter
    {
        public ThreadsMenuItem(ISession session)
            : base(session)
        {
        }

        protected override IUnitsModel CreateModel(ISession session)
        {
            IThreadCollection units = session.ServiceContainer.Resolve<IThreadCollection>();
            return new ThreadsModel(units);
        }
    }
}
