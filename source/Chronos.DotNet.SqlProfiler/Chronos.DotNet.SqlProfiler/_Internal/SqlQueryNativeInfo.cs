using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.DotNet.SqlProfiler
{
    [Serializable]
    [DataTable(TableName = "SqlProfiler_SqlQueries")]
    public sealed class SqlQueryNativeInfo : NativeUnitBase
    {
    }
}
