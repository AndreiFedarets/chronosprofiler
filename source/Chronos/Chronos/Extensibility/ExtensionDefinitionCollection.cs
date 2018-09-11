using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class ExtensionDefinitionCollection : ReadOnlyCollection<ExtensionDefinition>
    {
        internal ExtensionDefinitionCollection(IList<ExtensionDefinition> list)
            : base(list)
        {
        }

        public ExtensionDefinition this[Guid uid]
        {
            get
            {
                ExtensionDefinition definition = Items.FirstOrDefault(x => x.Uid == uid);
                if (definition == null)
                {
                    throw new ExtensionNotFoundException(uid);
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
