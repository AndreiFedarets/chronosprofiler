using Chronos.Client.Win.DotNet.BasicProfiler.Models;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class AssembliesMenuItem : UnitsMenuAdapter
    {
        public AssembliesMenuItem(ISession session)
            : base(session)
        {
        }

        protected override IUnitsModel CreateModel(ISession session)
        {
            IAssemblyCollection units = session.ServiceContainer.Resolve<IAssemblyCollection>();
            return new AssembliesModel(units);
        }
    }
}
