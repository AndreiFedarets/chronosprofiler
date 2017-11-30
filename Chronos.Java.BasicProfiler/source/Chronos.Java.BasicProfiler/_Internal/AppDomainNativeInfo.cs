using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.Java.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "BasicProfiler_AppDomains")]
    public sealed class AppDomainNativeInfo : NativeUnitBase
    {
    }
}
