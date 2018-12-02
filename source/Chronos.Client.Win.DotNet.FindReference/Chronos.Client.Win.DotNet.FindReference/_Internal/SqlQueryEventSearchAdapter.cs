using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;
using Chronos.DotNet.SqlProfiler;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal sealed class SqlQueryEventSearchAdapter : IEventSearchAdapter
    {
        private readonly SqlQueryInfo _sqlQueryInfo;

        public SqlQueryEventSearchAdapter(SqlQueryInfo sqlQueryInfo)
        {
            _sqlQueryInfo = sqlQueryInfo;
        }

        public string SearchText
        {
            get { return _sqlQueryInfo.Name; }
        }

        public bool Match(EventTreeItem item)
        {
            IEvent @event = item.Event;
            if (@event.EventType != Chronos.DotNet.SqlProfiler.Constants.EventType.SqlQuery)
            {
                return false;
            }
            return  @event.Unit == _sqlQueryInfo.Uid;
        }
    }
}
