using System.Collections.Generic;

namespace Chronos.Registry
{
    public sealed class RegistryValueCollection : IEnumerable<RegistryValue>
    {
        private readonly List<RegistryValue> _values;

        internal RegistryValueCollection(List<RegistryValue> values)
        {
            _values = values;
        }

        public IEnumerator<RegistryValue> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void SetParent(RegistryKey parent)
        {
            foreach (RegistryValue value in _values)
            {
                value.SetParent(parent);
            }
        }

        internal void Import()
        {
            foreach (RegistryValue value in _values)
            {
                value.Import();
            }
        }

        internal void Remove()
        {
            foreach (RegistryValue value in _values)
            {
                value.Remove();
            }
        }
    }
}
