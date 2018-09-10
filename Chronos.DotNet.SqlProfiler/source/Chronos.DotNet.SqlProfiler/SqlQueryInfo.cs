using System;
using Chronos.Model;

namespace Chronos.DotNet.SqlProfiler
{
    [Serializable]
    public sealed class SqlQueryInfo : UnitBase
    {
        public SqlQueryInfo(SqlQueryNativeInfo queryInfo)
            : base(queryInfo)
        {
            SetDependencies();
        }

        private SqlQueryNativeInfo QueryNativeInfo
        {
            get { return (SqlQueryNativeInfo)NativeUnit; }
        }

        internal void SetDependencies()
        {
        }
    }
}
