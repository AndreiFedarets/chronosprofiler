using System;
using System.IO;
using Chronos.Marshaling;

namespace Chronos
{
    [Serializable]
    public sealed class DynamicSettingsValue
    {
        private object _value;
        private byte[] _data;

        private bool IsSerialized
        {
            get { return _value == null && _data != null; }
        }

        public T GetValue<T>()
        {
            if (IsSerialized)
            {
                using (MemoryStream stream = new MemoryStream(_data))
                {
                    _value = MarshalingManager.Demarshal<T>(stream);
                }
                MarkDeserialized();
            }
            return (T)_value;
        }

        public void SetValue<T>(object value)
        {
            if (IsSerialized)
            {
                MarkDeserialized();
            }
            _value = value;
        }

        public void Deserialize(byte[] data)
        {
            _value = null;
            _data = data;
        }

        public byte[] Serialize()
        {
            if (IsSerialized)
            {
                return _data;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                MarshalingManager.Marshal(_value, stream);
                return stream.ToArray();
            }
        }

        public DynamicSettingsValue Clone()
        {
            DynamicSettingsValue clone = new DynamicSettingsValue();
            clone._data = Serialize();
            return clone;
        }

        private void MarkDeserialized()
        {
            _data = null;
        }
    }
}
