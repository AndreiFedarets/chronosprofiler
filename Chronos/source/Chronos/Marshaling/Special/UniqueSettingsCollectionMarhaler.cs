using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chronos.Marshaling
{
    public class UniqueSettingsCollectionMarhaler : ITypeMarshaler
    {
        private readonly ITypeMarshaler _elementMarshaler;

        public UniqueSettingsCollectionMarhaler(Type type, ITypeMarshaler elementMarshaler)
        {
            ManagedType = type;
            _elementMarshaler = elementMarshaler;
        }

        public Type ManagedType { get; private set; }

        public void MarshalObject(object value, Stream stream)
        {
            List<UniqueSettings> settings = new List<UniqueSettings>(((IEnumerable)value).Cast<UniqueSettings>());
            Int32Marshaler.Marshal(settings.Count(), stream);
            foreach (UniqueSettings element in settings)
            {
                _elementMarshaler.MarshalObject(element, stream);
            }
        }

        public object DemarshalObject(Stream stream)
        {
            List<UniqueSettings> collection = new List<UniqueSettings>();
            int count = Int32Marshaler.Demarshal(stream);
            for (int i = 0; i < count; i++)
            {
                UniqueSettings element = (UniqueSettings)_elementMarshaler.DemarshalObject(stream);
                collection.Add(element);
            }
            DynamicSettings settings = (DynamicSettings)Activator.CreateInstance(ManagedType, collection);
            return settings;
        }
    }
}
