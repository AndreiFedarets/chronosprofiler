using Chronos.Client.Win.DotNet.BasicProfiler.Models;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class FunctionsMenuItem : UnitsMenuAdapter
    {
        public FunctionsMenuItem(ISession session)
            : base(session)
        {
        }

        protected override IUnitsModel CreateModel(ISession session)
        {
            IFunctionCollection units = session.ServiceContainer.Resolve<IFunctionCollection>();
            return new FunctionsModel(units);
        }
    }
}
