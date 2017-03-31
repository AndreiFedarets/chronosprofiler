using System;
using System.Collections.Generic;

namespace Chronos
{
    /// <summary>
    /// Represents base class for dynamically extended settings
    /// </summary>
    [Serializable]
    public abstract class DynamicSettings : IEnumerable<KeyValuePair<Guid, DynamicSettingsValue>>
    {
        private readonly Dictionary<Guid, DynamicSettingsValue> _collection;

        /// <summary>
        /// Create new instance of DynamicSettings
        /// </summary>
        protected DynamicSettings()
        {
            _collection = new Dictionary<Guid, DynamicSettingsValue>();
        }

        /// <summary>
        /// Create new instance of DynamicSettings with predefined values
        /// </summary>
        protected DynamicSettings(Dictionary<Guid, DynamicSettingsValue> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Get setting value
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="token">Token of setting</param>
        /// <returns>Value of setting</returns>
        public T Get<T>(Guid token)
        {
            lock (_collection)
            {
                DynamicSettingsValue entry = _collection[token];
                return entry.GetValue<T>();
            }
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="token">Token of setting</param>
        /// <param name="value">Value of setting</param>
        public void Set<T>(Guid token, T value)
        {
            lock (_collection)
            {
                DynamicSettingsValue entry;
                if (!_collection.TryGetValue(token, out entry))
                {
                    entry = new DynamicSettingsValue();
                    _collection.Add(token, entry);
                }
                entry.SetValue<T>(value);
            }
        }

        public bool Contains(Guid token)
        {
            lock (_collection)
            {
                return _collection.ContainsKey(token);
            }
        }

        /// <summary>
        /// Clone internal properties storage
        /// </summary>
        /// <returns>Cloned storage</returns>
        public Dictionary<Guid, DynamicSettingsValue> CloneProperties()
        {
            Dictionary<Guid, DynamicSettingsValue> clone = new Dictionary<Guid, DynamicSettingsValue>();
            foreach (KeyValuePair<Guid, DynamicSettingsValue> pair in _collection)
            {
                clone.Add(pair.Key, pair.Value.Clone());
            }
            return clone;
        }

        /// <summary>
        /// Get internal properties storage
        /// </summary>
        /// <returns>Original storage</returns>
        public Dictionary<Guid, DynamicSettingsValue> GetProperties()
        {
            return _collection;
        }

        public abstract DynamicSettings Clone();

        public IEnumerator<KeyValuePair<Guid, DynamicSettingsValue>> GetEnumerator()
        {
            List<KeyValuePair<Guid, DynamicSettingsValue>> collection;
            lock (_collection)
            {
                collection = new List<KeyValuePair<Guid, DynamicSettingsValue>>(_collection);
            }
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Validate()
        {
            
        }
    }
}
