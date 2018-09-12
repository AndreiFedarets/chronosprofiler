using System;
using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos
{
    internal sealed class FrameworkCollection : RemoteBaseObject, IFrameworkCollection
    {
        private readonly Dictionary<Guid, IFramework> _collection;

        public FrameworkCollection(FrameworkDefinitionCollection definitions, IExportLoader exportLoader)
        {
            _collection = LoadCollection(definitions, exportLoader);
        }

        public IFramework this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IFramework item;
                    if (!_collection.TryGetValue(uid, out item))
                    {
                        throw new FrameworkNotFoundException(uid);
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
                return _collection.ContainsKey(uid);
            }
        }

        public bool TryGet(Guid uid, out IFramework item)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return _collection.TryGetValue(uid, out item);
            }
        }

        public IEnumerator<IFramework> GetEnumerator()
        {
            List<IFramework> collection;
            lock (Lock)
            {
                VerifyDisposed();
                collection = new List<IFramework>(_collection.Values);
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
                foreach (IFramework item in _collection.Values)
                {
                    item.TryDispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        private static Dictionary<Guid, IFramework> LoadCollection(FrameworkDefinitionCollection definitions, IExportLoader exportLoader)
        {
            Dictionary<Guid, IFramework> collection = new Dictionary<Guid, IFramework>();
            foreach (FrameworkDefinition definition in definitions)
            {
                IFramework item = new Framework(definition, exportLoader);
                collection.Add(definition.Uid, item);
            }
            return collection;
        }
    }
}
