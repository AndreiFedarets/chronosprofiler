using System.Collections.Generic;

namespace Chronos.Registry
{
    public sealed class RegistryKeyCollection : IEnumerable<RegistryKey>
    {
        private readonly List<RegistryKey> _keys;

        internal RegistryKeyCollection(List<RegistryKey> keys)
        {
            _keys = keys;
        }

        public IEnumerator<RegistryKey> GetEnumerator()
        {
            return _keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void SetParent(RegistryKey registryKey)
        {
            foreach (RegistryKey key in _keys)
            {
                key.SetParent(registryKey);
            }
        }

        internal void Import(VariableCollection variables)
        {
            foreach (RegistryKey key in _keys)
            {
                key.Import(variables);
            }
        }

        internal void Remove()
        {
            foreach (RegistryKey key in _keys)
            {
                key.Remove();
            }
        }
    }
}
