using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class ExportDefinitionCollection : ReadOnlyCollection<ExportDefinition>
    {
        internal ExportDefinitionCollection(IList<ExportDefinition> list)
            : base(list)
        {
        }

        public bool ContainsApplication(string applicationName)
        {
            return Items.Any(x => string.Equals(x.Application, applicationName, StringComparison.InvariantCultureIgnoreCase));
        }

        public LoadBehavior LoadBehavior
        {
            get
            {
                if (this.Any(x => x.LoadBehavior == LoadBehavior.OnStartup))
                {
                    return LoadBehavior.OnStartup;
                }
                return LoadBehavior.OnDemand;
            }
        }

        public ExportDefinition FindByApplication(string applicationName)
        {
            ExportDefinition definition = Items.FirstOrDefault(x => string.Equals(x.Application, applicationName, StringComparison.InvariantCultureIgnoreCase));
            if (definition == null)
            {
                throw new TempException();
            }
            return definition;
        }

        internal void Add(ExportDefinition definition)
        {
            //TODO: proper resolving of name conflicts
            if (ContainsApplication(definition.Application))
            {
                throw new TempException();
            }
            Items.Add(definition);
        }
    }
}
