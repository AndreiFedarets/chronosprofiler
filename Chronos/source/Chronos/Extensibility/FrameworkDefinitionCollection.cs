using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class FrameworkDefinitionCollection : ReadOnlyCollection<FrameworkDefinition>
    {
        internal FrameworkDefinitionCollection(IList<FrameworkDefinition> list)
            : base(list)
        {
        }

        public FrameworkDefinition this[Guid uid]
        {
            get
            {
                FrameworkDefinition definition = Items.FirstOrDefault(x => x.Uid == uid);
                if (definition == null)
                {
                    throw new FrameworkNotFoundException(uid);
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
