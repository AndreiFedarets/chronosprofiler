using Chronos.Client.Win.DotNet.BasicProfiler.Models;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class ModulesMenuItem : UnitsMenuAdapter
    {
        public ModulesMenuItem(ISession session)
            : base(session)
        {
        }

        protected override IUnitsModel CreateModel(ISession session)
        {
            IModuleCollection units = session.ServiceContainer.Resolve<IModuleCollection>();
            return new ModulesModel(units);
        }
    }
}
