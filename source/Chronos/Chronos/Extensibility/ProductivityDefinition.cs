using System;
using System.Collections.Generic;

namespace Chronos.Extensibility
{
    public sealed class ProductivityDefinition
    {
        internal ProductivityDefinition(Guid uid, List<ExportDefinition> exports, List<DependencyDefinition> dependencies,
            List<LocalizationDefinition> localizations, List<AttributeDefinition> attributes, List<PrerequisiteDefinition> prerequisites)
        {
            Uid = uid;
            Exports = new ExportDefinitionCollection(exports);
            Dependencies = new DependencyDefinitionCollection(dependencies);
            Localization = new LocalizationDefinitionCollection(localizations);
            Attributes = new AttributeDefinitionCollection(attributes);
            Prerequisites = new PrerequisiteDefinitionCollection(prerequisites);
        }

        /// <summary>
        /// Productivity unique identifier
        /// </summary>
        public Guid Uid { get; private set; }

        public ExportDefinitionCollection Exports { get; private set; }

        public DependencyDefinitionCollection Dependencies { get; private set; }

        public LocalizationDefinitionCollection Localization { get; private set; }

        public AttributeDefinitionCollection Attributes { get; private set; }

        public PrerequisiteDefinitionCollection Prerequisites { get; private set; }
    }
}
