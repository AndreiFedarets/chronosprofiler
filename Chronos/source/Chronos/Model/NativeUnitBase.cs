using System;
using Chronos.Storage;

namespace Chronos.Model
{
    [Serializable]
    public abstract class NativeUnitBase
    {
        protected NativeUnitBase()
        {
            Uid = 0;
            Id = 0;
            BeginLifetime = 0;
            EndLifetime = 0;
            Name = "<UNKNOWN>";
        }

        [DataTableColumn(PrimaryKey = true)]
        public ulong Uid { get; set; }

        [DataTableColumn]
        public ulong Id { get; set; }

        [DataTableColumn]
        public ulong BeginLifetime { get; set; }

        [DataTableColumn]
        public ulong EndLifetime { get; set; }

        [DataTableColumn]
        public string Name { get; set; }
    }
}
