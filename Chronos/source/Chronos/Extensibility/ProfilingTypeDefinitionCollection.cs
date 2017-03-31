using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class ProfilingTypeDefinitionCollection : ReadOnlyCollection<ProfilingTypeDefinition>
    {
        internal ProfilingTypeDefinitionCollection(IList<ProfilingTypeDefinition> list)
            : base(list)
        {
        }

        public ProfilingTypeDefinition this[Guid uid]
        {
            get
            {
                ProfilingTypeDefinition definition = Items.FirstOrDefault(x => x.Uid == uid);
                if (definition == null)
                {
                    throw new ProfilingTypeNotFoundException(uid);
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
