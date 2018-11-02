using System;
using Chronos.Common;
using Chronos.Storage;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "DotNet_BasicProfiler_AppDomains")]
    public sealed class AppDomainNativeInfo : NativeUnitBase
    {
    }
}
