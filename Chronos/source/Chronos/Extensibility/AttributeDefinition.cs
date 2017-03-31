using System;

namespace Chronos.Extensibility
{
    public sealed class AttributeDefinition
    {
        internal AttributeDefinition(string name, string valueString, string typeString)
        {
            Name = name;
            ValueString = valueString;
            TypeString = typeString;
        }

        public string Name { get; private set; }

        public string ValueString { get; private set; }

        public string TypeString { get; private set; }

        public Type Type
        {
            get { return Type.GetType(TypeString); }
        }

        public object Value
        {
            get { return Convert.ChangeType(ValueString, Type); }
        }
    }
}
