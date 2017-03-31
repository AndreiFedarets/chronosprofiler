using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.DotNet.SqlProfiler
{
    [Serializable]
    [DataTable(TableName = "SqlProfiler_MsSqlQueries")]
    public sealed class MsSqlQueryNativeInfo : NativeUnitBase
    {
    }
}
