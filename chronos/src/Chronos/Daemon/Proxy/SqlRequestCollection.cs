using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class SqlRequestCollection : UnitCollection<SqlRequestInfo>, ISqlRequestCollection
	{
        public SqlRequestCollection(IProcessShadow processShadow, IUnitCollection<SqlRequestInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.SqlRequest)
		{
		}
	}
}
