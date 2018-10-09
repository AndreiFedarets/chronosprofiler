using System;

namespace Chronos.Storage
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class DataTableAttribute : Attribute
    {
        public string TableName { get; set; }
    }
}
