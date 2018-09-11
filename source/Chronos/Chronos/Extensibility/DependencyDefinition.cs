using System;

namespace Chronos.Extensibility
{
    public sealed class DependencyDefinition
    {
        internal DependencyDefinition(Guid uid, DependencyType dependencyType)
        {
            Uid = uid;
            DependencyType = dependencyType;
        }

        public Guid Uid { get; private set; }

        public DependencyType DependencyType { get; private set; } 
    }
}
