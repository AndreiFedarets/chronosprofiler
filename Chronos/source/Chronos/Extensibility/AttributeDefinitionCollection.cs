using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class AttributeDefinitionCollection : ReadOnlyCollection<AttributeDefinition>
    {
        internal AttributeDefinitionCollection(IList<AttributeDefinition> list)
            : base(list)
        {
        }

        public AttributeDefinition this[string name]
        {
            get
            {
                AttributeDefinition definition = Items.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
                if (definition == null)
                {
                    throw new AttributeNotFoundException(name);
                }
                return definition;
            }
        }

        public object GetAttributeValue(string name, object defaultValue)
        {
            AttributeDefinition definition = Items.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
            if (definition == null)
            {
                return defaultValue;
            }
            return definition.Value;
        }

        public bool Contains(string name)
        {
            return Items.Any(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        internal void Add(AttributeDefinition definition)
        {
            //TODO: proper resolving of name conflicts
            if (Contains(definition.Name))
            {
                throw new TempException();
            }
            Items.Add(definition);
        }
    }
}
