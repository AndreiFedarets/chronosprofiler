using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Chronos.Client.Win
{
    public class ResolutionDependencies : IEnumerable<KeyValuePair<Type, object>>
    {
        private readonly Dictionary<Type, object> _collection;

        public ResolutionDependencies()
        {
            _collection = new Dictionary<Type, object>();
        }

        public void Register<T>(T service)
        {
            _collection[typeof (T)] = service;
        }

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void RegisterInContainer(IContainer container)
        {
            foreach (KeyValuePair<Type, object> pair in _collection)
            {
                container.RegisterInstance(pair.Key, pair.Value);
            }
        }
    }
}
