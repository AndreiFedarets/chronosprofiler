using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class ProfilingTargetDefinitionCollection : ReadOnlyCollection<ProfilingTargetDefinition>
    {
        internal ProfilingTargetDefinitionCollection(IList<ProfilingTargetDefinition> list)
            : base(list)
        {
        }

        public ProfilingTargetDefinition this[Guid uid]
        {
            get
            {
                ProfilingTargetDefinition definition = Items.FirstOrDefault(x => x.Uid == uid);
                if (definition == null)
                {
                    throw new ProfilingTargetNotFoundException(uid);
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
