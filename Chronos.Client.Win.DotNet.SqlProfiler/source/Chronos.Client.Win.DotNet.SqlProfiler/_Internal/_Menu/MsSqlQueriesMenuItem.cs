using Chronos.Client.Win.DotNet.SqlProfiler.Models;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    internal sealed class MsSqlQueriesMenuItem : UnitsMenuAdapter
    {
        public MsSqlQueriesMenuItem(ISession session)
            : base(session)
        {
        }

        protected override IUnitsModel CreateModel(ISession session)
        {
            IMsSqlQueryCollection units = session.ServiceContainer.Resolve<IMsSqlQueryCollection>();
            return new MsSqlQueriesModel(units);
        }
    }
}
