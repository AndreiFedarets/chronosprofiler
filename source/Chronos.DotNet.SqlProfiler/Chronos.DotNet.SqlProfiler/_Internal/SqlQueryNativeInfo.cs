using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.DotNet.SqlProfiler
{
    [Serializable]
    [DataTable(TableName = "DotNet_SqlProfiler_SqlQueries")]
    public sealed class SqlQueryNativeInfo : NativeUnitBase
    {
    }
}
