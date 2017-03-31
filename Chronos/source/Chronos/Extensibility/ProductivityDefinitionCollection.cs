using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class ProductivityDefinitionCollection : ReadOnlyCollection<ProductivityDefinition>
    {
        internal ProductivityDefinitionCollection(IList<ProductivityDefinition> list)
            : base(list)
        {
        }

        public ProductivityDefinition this[Guid uid]
        {
            get
            {
                ProductivityDefinition definition = Items.FirstOrDefault(x => x.Uid == uid);
                if (definition == null)
                {
                    throw new ProductivityNotFoundException(uid);
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
