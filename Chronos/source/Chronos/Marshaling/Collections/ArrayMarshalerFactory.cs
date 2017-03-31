using System;

namespace Chronos.Marshaling
{
    public class ArrayMarshalerFactory : ITypeMarshalerFactory
    {
        public bool CanMarshal(Type type)
        {
            if (!type.IsArray)
            {
                return false;
            }
            Type elementType = type.GetElementType();
            return MarshalingManager.IsKnownType(elementType);
        }

        public bool TryCreate(Type type, out ITypeMarshaler typeMarshaler)
        {
            if (!type.IsArray)
            {
                typeMarshaler = null;
                return false;
            }
            Type elementType = type.GetElementType();
            ITypeMarshaler elementMarshaler = MarshalingManager.GetMarshaler(elementType);
            if (elementMarshaler == null)
            {
                typeMarshaler = null;
                return false;
            }
            typeMarshaler = new ArrayMarshaler(type, elementMarshaler);
            return true;
        }
    }
}
