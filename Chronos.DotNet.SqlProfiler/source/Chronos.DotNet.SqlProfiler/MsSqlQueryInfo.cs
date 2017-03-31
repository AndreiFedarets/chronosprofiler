using System;
using Chronos.Model;

namespace Chronos.DotNet.SqlProfiler
{
    [Serializable]
    public sealed class MsSqlQueryInfo : UnitBase
    {
        public MsSqlQueryInfo(MsSqlQueryNativeInfo queryInfo)
            : base(queryInfo)
        {
            SetDependencies();
        }

        private MsSqlQueryNativeInfo QueryNativeInfo
        {
            get { return (MsSqlQueryNativeInfo)NativeUnit; }
        }

        internal void SetDependencies()
        {
        }
    }
}
