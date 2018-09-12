using System;
using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos
{
    internal sealed class ProfilingTargetCollection : RemoteBaseObject, IProfilingTargetCollection
    {
        private readonly Dictionary<Guid, IProfilingTarget> _collection;

        public ProfilingTargetCollection(ProfilingTargetDefinitionCollection definitions, IExportLoader exportLoader)
        {
            _collection = LoadCollection(definitions, exportLoader);
        }

        public IProfilingTarget this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IProfilingTarget item;
                    if (!_collection.TryGetValue(uid, out item))
                    {
                        throw new ProfilingTargetNotFoundException(uid);
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

        public bool TryGet(Guid uid, out IProfilingTarget item)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return _collection.TryGetValue(uid, out item);
            }
        }

        public IEnumerator<IProfilingTarget> GetEnumerator()
        {
            List<IProfilingTarget> collection;
            lock (Lock)
            {
                VerifyDisposed();
                collection = new List<IProfilingTarget>(_collection.Values);
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
                foreach (IProfilingTarget item in _collection.Values)
                {
                    item.TryDispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        private static Dictionary<Guid, IProfilingTarget> LoadCollection(ProfilingTargetDefinitionCollection definitions, IExportLoader exportLoader)
        {
            Dictionary<Guid, IProfilingTarget> collection = new Dictionary<Guid, IProfilingTarget>();
            foreach (ProfilingTargetDefinition definition in definitions)
            {
                IProfilingTarget item = new ProfilingTarget(definition, exportLoader);
                collection.Add(definition.Uid, item);
            }
            return collection;
        }
    }
}
