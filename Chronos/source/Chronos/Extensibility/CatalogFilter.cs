using System.Collections.Generic;
using System.Linq;

namespace Chronos.Extensibility
{
    public static class CatalogFilter
    {
        public static Catalog Filter(Catalog catalog, ConfigurationSettings configurationSettings)
        {
            ExtensionDefinition extension;
            //Extensions
            List<ExtensionDefinition> extensions = new List<ExtensionDefinition>();
            //Productivities
            List<ProductivityDefinition> productivities = new List<ProductivityDefinition>(catalog.Productivities);
            //ProfilingTypes
            List<ProfilingTypeDefinition> profilingTypes = new List<ProfilingTypeDefinition>();
            foreach (ProfilingTypeSettings settings in configurationSettings.ProfilingTypesSettings)
            {
                ProfilingTypeDefinition profilingType = catalog.ProfilingTypes[settings.Uid];
                profilingTypes.Add(profilingType);
                extension = catalog.Extensions.First(x => x.ProfilingTypes.Contains(profilingType));
                if (!extensions.Contains(extension))
                {
                    extensions.Add(extension);
                }
            }
            //Frameworks
            List<FrameworkDefinition> frameworks = new List<FrameworkDefinition>();
            foreach (FrameworkSettings settings in configurationSettings.FrameworksSettings)
            {
                FrameworkDefinition framework = catalog.Frameworks[settings.Uid];
                frameworks.Add(framework);
                extension = catalog.Extensions.First(x => x.Frameworks.Contains(framework));
                if (!extensions.Contains(extension))
                {
                    extensions.Add(extension);
                }
            }
            //ProfilingTarget
            ProfilingTargetDefinition profilingTarget = catalog.ProfilingTargets[configurationSettings.ProfilingTargetSettings.Uid];
            List<ProfilingTargetDefinition> profilingTargets = new List<ProfilingTargetDefinition> { profilingTarget };
            extension = catalog.Extensions.First(x => x.ProfilingTargets.Contains(profilingTarget));
            if (!extensions.Contains(extension))
            {
                extensions.Add(extension);
            }
            catalog = new Catalog(extensions, frameworks, profilingTypes, profilingTargets, productivities);
            return catalog;
        }
    }
}
