using System;

namespace Chronos.Storage
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DataTableColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }

        public bool PrimaryKey { get; set; }

        public string DataType { get; set; }
    }
}
