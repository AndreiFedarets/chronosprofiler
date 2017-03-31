using System;
using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos
{
    internal sealed class ProfilingTypeCollection : RemoteBaseObject, IProfilingTypeCollection
    {
        private readonly Dictionary<Guid, IProfilingType> _collection;

        public ProfilingTypeCollection(ProfilingTypeDefinitionCollection definitions, IExportLoader exportLoader)
        {
            _collection = LoadCollection(definitions, exportLoader);
        }

        public IProfilingType this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IProfilingType item;
                    if (!_collection.TryGetValue(uid, out item))
                    {
                        throw new ProfilingTypeNotFoundException(uid);
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

        public bool TryGet(Guid uid, out IProfilingType item)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return _collection.TryGetValue(uid, out item);
            }
        }

        public IEnumerator<IProfilingType> GetEnumerator()
        {
            List<IProfilingType> collection;
            lock (Lock)
            {
                VerifyDisposed();
                collection = new List<IProfilingType>(_collection.Values);
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
                foreach (IProfilingType item in _collection.Values)
                {
                    item.TryDispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        private static Dictionary<Guid, IProfilingType> LoadCollection(ProfilingTypeDefinitionCollection definitions, IExportLoader exportLoader)
        {
            Dictionary<Guid, IProfilingType> collection = new Dictionary<Guid, IProfilingType>();
            foreach (ProfilingTypeDefinition definition in definitions)
            {
                IProfilingType item = new ProfilingType(definition, exportLoader);
                collection.Add(definition.Uid, item);
            }
            return collection;
        }
    }
}
