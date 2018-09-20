using System;
using System.Collections.Generic;

namespace Chronos.Extensibility
{
    public sealed class ProfilingTypeDefinition
    {
        internal ProfilingTypeDefinition(Guid uid, Guid frameworkUid, List<ExportDefinition> exports, List<DependencyDefinition> dependencies,
            List<LocalizationDefinition> localizations, List<AttributeDefinition> attributes, List<PrerequisiteDefinition> prerequisites)
        {
            Uid = uid;
            FrameworkUid = frameworkUid;
            Exports = new ExportDefinitionCollection(exports);
            Dependencies = new DependencyDefinitionCollection(dependencies);
            Localization = new LocalizationDefinitionCollection(localizations);
            Attributes = new AttributeDefinitionCollection(attributes);
            Prerequisites = new PrerequisiteDefinitionCollection(prerequisites);
        }

        public Guid Uid { get; private set; }

        /// <summary>
        /// Get visibility of Profiling Type for user
        /// </summary>
        public bool IsHidden { get; private set; }

        public Guid FrameworkUid { get; private set; }

        public ExportDefinitionCollection Exports { get; private set; }

        public DependencyDefinitionCollection Dependencies { get; private set; }

        public LocalizationDefinitionCollection Localization { get; private set; }

        public AttributeDefinitionCollection Attributes { get; private set; }

        public PrerequisiteDefinitionCollection Prerequisites { get; private set; }

        //TODO: override Equals and GetHashCode
    }
}
