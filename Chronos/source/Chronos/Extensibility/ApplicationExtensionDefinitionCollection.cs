using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class ApplicationExtensionDefinitionCollection : ReadOnlyCollection<ApplicationExtensionDefinition>
    {
        internal ApplicationExtensionDefinitionCollection(IList<ApplicationExtensionDefinition> list)
            : base(list)
        {
        }

        public ApplicationExtensionDefinition this[Guid uid]
        {
            get
            {
                ApplicationExtensionDefinition definition = Items.FirstOrDefault(x => x.Uid == uid);
                if (definition == null)
                {
                    throw new ApplicationExtensionNotFoundException(uid);
                }
                return definition;
            }
        }

        public bool Contains(Guid uid)
        {
            return Items.Any(x => x.Uid == uid);
        }
    }
}
