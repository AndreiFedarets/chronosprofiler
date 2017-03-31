using Chronos.Client.Win.DotNet.BasicProfiler.Models;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class ClassesMenuItem : UnitsMenuAdapter
    {
        public ClassesMenuItem(ISession session)
            : base(session)
        {
        }

        protected override IUnitsModel CreateModel(ISession session)
        {
            IClassCollection units = session.ServiceContainer.Resolve<IClassCollection>();
            return new ClassesModel(units);
        }
    }
}
