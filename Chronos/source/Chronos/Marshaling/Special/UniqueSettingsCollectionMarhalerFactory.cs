using System;

namespace Chronos.Marshaling
{
    public class UniqueSettingsCollectionMarhalerFactory : ITypeMarshalerFactory
    {
        public bool CanMarshal(Type type)
        {
            return IsUniqueSettingsCollection(type) && !type.IsAbstract;
        }

        public bool TryCreate(Type type, out ITypeMarshaler typeMarshaler)
        {
            if (!IsUniqueSettingsCollection(type) || type.IsAbstract)
            {
                typeMarshaler = null;
                return false;
            }
            Type elementType = GetSettingsType(type);
            ITypeMarshaler elementMarshaler = MarshalingManager.GetMarshaler(elementType);
            typeMarshaler = new UniqueSettingsCollectionMarhaler(type, elementMarshaler);
            return true;
        }

        private static bool IsUniqueSettingsCollection(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof (UniqueSettingsCollection<>))
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        private static Type GetSettingsType(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(UniqueSettingsCollection<>))
                {
                    return type.GetGenericArguments()[0];
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
