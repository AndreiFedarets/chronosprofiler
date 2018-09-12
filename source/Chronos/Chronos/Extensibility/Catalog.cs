using System.Collections.Generic;

namespace Chronos.Extensibility
{
    public sealed class Catalog
    {
        public readonly static Catalog EmptyCatalog;

        static Catalog()
        {
            EmptyCatalog = new Catalog(new List<ExtensionDefinition>(), new List<FrameworkDefinition>(),
                new List<ProfilingTypeDefinition>(), new List<ProfilingTargetDefinition>(),
                new List<ProductivityDefinition>(), new List<ApplicationExtensionDefinition>());
        }

        public Catalog(List<ExtensionDefinition> extensions, List<FrameworkDefinition> frameworks, List<ProfilingTypeDefinition> profilingTypes,
            List<ProfilingTargetDefinition> profilingTargets, List<ProductivityDefinition> productivities, List<ApplicationExtensionDefinition> applicationExtensions)
        {
            Extensions = new ExtensionDefinitionCollection(extensions);
            Frameworks = new FrameworkDefinitionCollection(frameworks);
            ProfilingTypes = new ProfilingTypeDefinitionCollection(profilingTypes);
            ProfilingTargets = new ProfilingTargetDefinitionCollection(profilingTargets);
            Productivities = new ProductivityDefinitionCollection(productivities);
            ApplicationExtensions = new ApplicationExtensionDefinitionCollection(applicationExtensions);
        }

        public ExtensionDefinitionCollection Extensions { get; private set; }

        public FrameworkDefinitionCollection Frameworks { get; private set; }

        public ProfilingTypeDefinitionCollection ProfilingTypes { get; private set; }

        public ProductivityDefinitionCollection Productivities { get; private set; }

        public ProfilingTargetDefinitionCollection ProfilingTargets { get; private set; }

        public ApplicationExtensionDefinitionCollection ApplicationExtensions { get; private set; }
    }
}
