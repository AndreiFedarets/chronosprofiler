﻿using System;
using Chronos.Common;
using Chronos.Storage;

namespace Chronos.DotNet.SqlProfiler
{
    [Serializable]
    [DataTable(TableName = "DotNet_SqlProfiler_SqlQueries")]
    public sealed class SqlQueryNativeInfo : NativeUnitBase
    {
    }
}
