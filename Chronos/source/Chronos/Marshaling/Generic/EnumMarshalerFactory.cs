using System;

namespace Chronos.Marshaling
{
    public class EnumMarshalerFactory : ITypeMarshalerFactory
    {
        public bool CanMarshal(Type type)
        {
            if (!type.IsEnum)
            {
                return false;
            }
            Type underlyingType = Enum.GetUnderlyingType(type);
            return MarshalingManager.IsKnownType(underlyingType);
        }

        public bool TryCreate(Type type, out ITypeMarshaler typeMarshaler)
        {
            if (!type.IsEnum)
            {
                typeMarshaler = null;
                return false;
            }
            Type underlyingType = Enum.GetUnderlyingType(type);
            ITypeMarshaler underlyingMarshaler = MarshalingManager.GetMarshaler(underlyingType);
            if (underlyingMarshaler == null)
            {
                typeMarshaler = null;
                return false;
            }
            typeMarshaler = new EnumMarshaler(type, underlyingMarshaler);
            return true;
        }
    }
}