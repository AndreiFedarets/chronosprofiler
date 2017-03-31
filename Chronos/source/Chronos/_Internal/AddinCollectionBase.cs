using System;
using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos
{
    internal abstract class AddinCollectionBase<T> : RemoteBaseObject, IEnumerable<T>
    {
        private readonly dynamic _definitions;
        private readonly Dictionary<Guid, T> _collection;
        private readonly IExportLoader _exportLoader;

        protected AddinCollectionBase(dynamic definitions, IExportLoader exportLoader)
        {
            _definitions = definitions;
            _exportLoader = exportLoader;
            _collection = new Dictionary<Guid, T>();
            Initialize();
        }

        public T this[Guid uid]
        {
            get
            {
                dynamic definition = _definitions[uid];
                lock (Lock)
                {
                    VerifyDisposed();
                    T item;
                    if (!_collection.TryGetValue(uid, out item))
                    {
                        item = CreateItemAndAddToCollection(definition);
                    }
                    return item;
                }
            }
        }

        public bool Contains(Guid uid)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return _definitions.Contains(uid);
            }
        }

        public bool TryGet(Guid uid, out T item)
        {
            lock (Lock)
            {
                VerifyDisposed();
                if (!_definitions.Contains(uid))
                {
                    item = default(T);
                    return false;
                }
                if (!_collection.TryGetValue(uid, out item))
                {
                    dynamic definition = _definitions[uid];
                    CreateItemAndAddToCollection(definition);
                }
                return true;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> collection;
            lock (Lock)
            {
                VerifyDisposed();
                if (_definitions.Count != _collection.Count)
                {
                    foreach (dynamic definition in _definitions)
                    {
                        if (!_collection.ContainsKey(definition.Uid))
                        {
                            CreateItemAndAddToCollection(definition);
                        }
                    }
                }
                collection = new List<T>(_collection.Values);
            }
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                foreach (T item in _collection.Values)
                {
                    item.TryDispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        protected T CreateItemAndAddToCollection(dynamic definition)
        {
            T item = CreateItem(definition, _exportLoader);
            _collection.Add(definition.Uid, item);
            return item;
        }

        private void Initialize()
        {
            foreach (dynamic definition in _definitions)
            {
                if (definition.Exports.LoadBehavior == LoadBehavior.OnStartup)
                {
                    CreateItemAndAddToCollection(definition);
                }
            }
        }

        protected abstract T CreateItem(dynamic definition, IExportLoader exportLoader);
    }
}
