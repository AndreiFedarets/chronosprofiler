using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class DependencyDefinitionCollection : ReadOnlyCollection<DependencyDefinition>
    {
        internal DependencyDefinitionCollection(IList<DependencyDefinition> list)
            : base(list)
        {
        }

        public bool ContainsUid(Guid uid)
        {
            return Items.Any(x => x.Uid == uid);
        }

        public DependencyDefinition FindByUid(Guid uid)
        {
            return Items.FirstOrDefault(x => x.Uid == uid);
        }
    }
}
