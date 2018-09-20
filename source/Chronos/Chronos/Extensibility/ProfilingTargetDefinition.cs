using System;
using System.Collections.Generic;

namespace Chronos.Extensibility
{
    public sealed class ProfilingTargetDefinition
    {
        internal ProfilingTargetDefinition(Guid uid, List<ExportDefinition> exports, List<LocalizationDefinition> localizations, 
            List<AttributeDefinition> attributes, List<PrerequisiteDefinition> prerequisites)
        {
            Uid = uid;
            Exports = new ExportDefinitionCollection(exports);
            Localization = new LocalizationDefinitionCollection(localizations);
            Attributes = new AttributeDefinitionCollection(attributes);
            Prerequisites = new PrerequisiteDefinitionCollection(prerequisites);
        }

        public Guid Uid { get; private set; }

        public ExportDefinitionCollection Exports { get; private set; }

        public LocalizationDefinitionCollection Localization { get; private set; }

        public AttributeDefinitionCollection Attributes { get; private set; }

        public PrerequisiteDefinitionCollection Prerequisites { get; private set; }

        //TODO: override Equals and GetHashCode
    }
}
