using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chronos.Marshaling
{
    public class DynamicSettingsMarhaler : ITypeMarshaler
    {
        public DynamicSettingsMarhaler(Type type)
        {
            ManagedType = type;
        }

        public Type ManagedType { get; private set; }

        public void MarshalObject(object value, Stream stream)
        {
            DynamicSettings settings = (DynamicSettings) value;
            Int32Marshaler.Marshal(settings.Count(), stream);
            foreach (KeyValuePair<Guid, DynamicSettingsValue> pair in settings)
            {
                GuidMarshaler.Marshal(pair.Key, stream);
                ByteArrayMarshaler.Marshal(pair.Value.Serialize(), stream);
            }
        }

        public object DemarshalObject(Stream stream)
        {
            Dictionary<Guid, DynamicSettingsValue> collection = new Dictionary<Guid, DynamicSettingsValue>();
            int count = Int32Marshaler.Demarshal(stream);
            for (int i = 0; i < count; i++)
            {
                Guid key = GuidMarshaler.Demarshal(stream);
                byte[] data = ByteArrayMarshaler.Demarshal(stream);
                DynamicSettingsValue entry = new DynamicSettingsValue();
                entry.Deserialize(data);
                collection.Add(key, entry);
            }
            DynamicSettings settings = (DynamicSettings)Activator.CreateInstance(ManagedType, collection);
            return settings;
        }
    }
}
