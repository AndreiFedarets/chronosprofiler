using System;
using System.Collections.Generic;

namespace Chronos.Extensibility
{
    public sealed class AttachmentDefinition
    {
        internal AttachmentDefinition(Guid targetUid, List<ExportDefinition> exports,
            List<DependencyDefinition> dependencies, List<AttributeDefinition> attributes)
        {
            TargetUid = targetUid;
            Exports = new ExportDefinitionCollection(exports);
            Dependencies = new DependencyDefinitionCollection(dependencies);
            Attributes = new AttributeDefinitionCollection(attributes);
        }

        public Guid TargetUid { get; private set; }

        public ExportDefinitionCollection Exports { get; private set; }

        public DependencyDefinitionCollection Dependencies { get; private set; }

        public AttributeDefinitionCollection Attributes { get; private set; }

        //TODO: override Equals and GetHashCode
    }
}
