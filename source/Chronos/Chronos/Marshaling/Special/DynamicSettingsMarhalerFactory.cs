using System;

namespace Chronos.Marshaling
{
    public class DynamicSettingsMarhalerFactory : ITypeMarshalerFactory
    {
        public bool CanMarshal(Type type)
        {
            return typeof(DynamicSettings).IsAssignableFrom(type);
        }

        public bool TryCreate(Type type, out ITypeMarshaler typeMarshaler)
        {
            if (!typeof (DynamicSettings).IsAssignableFrom(type))
            {
                typeMarshaler = null;
                return false;
            }
            typeMarshaler = new DynamicSettingsMarhaler(type);
            return true;
        }
    }
}
