using Chronos.Extensibility;
using System;
using System.Collections.Generic;

namespace Chronos
{
    internal sealed class ApplicationExtensionCollection : RemoteBaseObject, IApplicationExtensionCollection
    {
        private readonly Dictionary<Guid, IApplicationExtension> _collection;

        public ApplicationExtensionCollection(ApplicationExtensionDefinitionCollection definitions, IExportLoader exportLoader)
        {
            _collection = LoadCollection(definitions, exportLoader);
        }

        public IApplicationExtension this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IApplicationExtension item;
                    if (!_collection.TryGetValue(uid, out item))
                    {
                        throw new ApplicationExtensionNotFoundException(uid);
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

        public bool TryGet(Guid uid, out IApplicationExtension item)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return _collection.TryGetValue(uid, out item);
            }
        }

        public IEnumerator<IApplicationExtension> GetEnumerator()
        {
            List<IApplicationExtension> collection;
            lock (Lock)
            {
                VerifyDisposed();
                collection = new List<IApplicationExtension>(_collection.Values);
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
                foreach (IApplicationExtension item in _collection.Values)
                {
                    item.TryDispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        private static Dictionary<Guid, IApplicationExtension> LoadCollection(ApplicationExtensionDefinitionCollection definitions, IExportLoader exportLoader)
        {
            Dictionary<Guid, IApplicationExtension> collection = new Dictionary<Guid, IApplicationExtension>();
            foreach (ApplicationExtensionDefinition definition in definitions)
            {
                IApplicationExtension item = new ApplicationExtension(definition, exportLoader);
                collection.Add(definition.Uid, item);
            }
            return collection;
        }
    }
}
