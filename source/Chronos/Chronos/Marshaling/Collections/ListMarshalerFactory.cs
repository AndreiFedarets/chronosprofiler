using System;
using System.Collections.Generic;

namespace Chronos.Marshaling
{
    public class ListMarshalerFactory : ITypeMarshalerFactory
    {
        public bool CanMarshal(Type type)
        {
            if (!IsList(type))
            {
                return false;
            }
            Type elementType = type.GetProperty("Item").PropertyType;
            return MarshalingManager.IsKnownType(elementType);
        }

        public bool TryCreate(Type type, out ITypeMarshaler typeMarshaler)
        {
            if (!IsList(type))
            {
                typeMarshaler = null;
                return false;
            }
            Type elementType = type.GetProperty("Item").PropertyType;
            ITypeMarshaler elementMarshaler = MarshalingManager.GetMarshaler(elementType);
            if (elementMarshaler == null)
            {
                typeMarshaler = null;
                return false;
            }
            typeMarshaler = new ListMarshaler(type, elementMarshaler);
            return true;
        }

        private static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (List<>);
        }
    }
}
