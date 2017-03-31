using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chronos.Extensibility
{
    public sealed class ExtensionDefinition
    {
        internal ExtensionDefinition(Guid uid, string baseDirectory, List<ProfilingTypeDefinition> profilingTypes, List<ProfilingTargetDefinition> profilingTargets,
            List<FrameworkDefinition> frameworks, List<ProductivityDefinition> productivities, List<AttachmentDefinition> attachments, List<LocalizationDefinition> localizations)
        {
            Uid = uid;
            BaseDirectory = baseDirectory;
            ProfilingTypes = new ProfilingTypeDefinitionCollection(profilingTypes);
            ProfilingTargets = new ReadOnlyCollection<ProfilingTargetDefinition>(profilingTargets);
            Frameworks = new ReadOnlyCollection<FrameworkDefinition>(frameworks);
            Productivities = new ReadOnlyCollection<ProductivityDefinition>(productivities);
            Localization = new LocalizationDefinitionCollection(localizations);
            Attachments = new AttachmentDefinitionCollection(attachments);
        }

        public string BaseDirectory { get; private set; }

        public Guid Uid { get; private set; }

        public ProfilingTypeDefinitionCollection ProfilingTypes { get; private set; }

        public ReadOnlyCollection<ProfilingTargetDefinition> ProfilingTargets { get; private set; }

        public ReadOnlyCollection<FrameworkDefinition> Frameworks { get; private set; }

        public ReadOnlyCollection<ProductivityDefinition> Productivities { get; private set; }

        public ReadOnlyCollection<AttachmentDefinition> Attachments { get; private set; }

        public LocalizationDefinitionCollection Localization { get; private set; }
    }
}
