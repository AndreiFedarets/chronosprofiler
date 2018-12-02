using Chronos.Common.EventsTree;

namespace Chronos.DotNet.SqlProfiler
{
    internal sealed class SqlQueryEventMessage : RemoteBaseObject, ISqlQueryEventMessage 
    {
        private readonly ISqlQueryCollection _queries;

        public SqlQueryEventMessage(ISqlQueryCollection queries)
        {
            _queries = queries;
        }

        public string BuildMessage(IEvent @event)
        {
            return BuildMessageInternal(@event, _queries);
        }

        internal static string BuildMessageInternal(IEvent @event, ISqlQueryCollection queries)
        {
            SqlQueryInfo queryInfo = queries[@event.Unit];
            if (queryInfo == null)
            {
                return "<UNKNOWN QUERY>";
            }
            return queryInfo.Name;
        }
    }
}
