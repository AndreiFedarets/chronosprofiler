using Chronos.Client.Win.DotNet.SqlProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.SqlProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.DotNet.SqlProfiler;

namespace Chronos.Client.Win.Menu.DotNet.SqlProfiler
{
    internal sealed class SqlQueriesMenuItem : UnitsMenuItemBase
    {
        public SqlQueriesMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }

        public override string Text
        {
            get { return Resources.SqlQueriesMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            ISqlQueryCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<ISqlQueryCollection>();
            SqlQueriesModel model = new SqlQueriesModel(collection);
            return model;
        }
    }
}
