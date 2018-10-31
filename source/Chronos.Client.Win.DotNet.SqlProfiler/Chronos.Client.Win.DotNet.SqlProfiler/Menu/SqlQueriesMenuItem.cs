using Chronos.Client.Win.DotNet.SqlProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.SqlProfiler;
using Chronos.DotNet.SqlProfiler;

namespace Chronos.Client.Win.DotNet.SqlProfiler.Menu
{
    internal sealed class SqlQueriesMenuItem : UnitsMenuItemBase
    {
        public SqlQueriesMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string GetText()
        {
            return Resources.SqlQueriesMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            ISqlQueryCollection collection = Application.ServiceContainer.Resolve<ISqlQueryCollection>();
            SqlQueriesModel model = new SqlQueriesModel(collection);
            return model;
        }
    }
}
