using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Proxy
{
    internal sealed class ProfilingTypeCollection : ProxyBaseObject<IProfilingTypeCollection>, IProfilingTypeCollection
    {
        private readonly Dictionary<Guid, ProfilingType> _collection;
        private volatile bool _initialized;

        public ProfilingTypeCollection(IProfilingTypeCollection profilingTypes)
            : base(profilingTypes)
        {
            _initialized = false;
            _collection = new Dictionary<Guid, ProfilingType>();
        }

        public IProfilingType this[Guid uid]
        {
            get
            {
                lock (_collection)
                {
                    VerifyDisposed();
                    ProfilingType profilingType;
                    if (_collection.TryGetValue(uid, out profilingType))
                    {
                        return profilingType;
                    }
                    IProfilingType remoteProfilingType = Execute(() => RemoteObject[uid]);
                    profilingType = new ProfilingType(remoteProfilingType);
                    _collection.Add(uid, profilingType);
                    return profilingType;
                }
            }
        }

        public bool Contains(Guid uid)
        {
            VerifyDisposed();
            lock (_collection)
            {
                if (_collection.ContainsKey(uid))
                {
                    return true;
                }
            }
            return Execute(() => RemoteObject.Contains(uid));
        }

        public bool TryGet(Guid uid, out IProfilingType item)
        {
            lock (_collection)
            {
                ProfilingType profilingType;
                if (_collection.TryGetValue(uid, out profilingType))
                {
                    item = profilingType;
                    return true;
                }
                IProfilingType remoteProfilingType = null;
                if (Execute(() => RemoteObject.TryGet(uid, out remoteProfilingType)))
                {
                    profilingType = new ProfilingType(remoteProfilingType);
                    _collection.Add(uid, profilingType);
                    item = profilingType;
                    return true;
                }
            }
            item = null;
            return false;
        }

        public IEnumerator<IProfilingType> GetEnumerator()
        {
            List<IProfilingType> collection;
            lock (_collection)
            {
                VerifyDisposed();
                Initialize();
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
            lock (_collection)
            {
                VerifyDisposed();
                foreach (ProfilingType profilingType in _collection.Values)
                {
                    profilingType.Dispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            List<IProfilingType> remoteProfilingTypes = Execute(() => RemoteObject.ToList());
            if (remoteProfilingTypes.Count == _collection.Count)
            {
                return;
            }
            foreach (IProfilingType remoteProfilingType in remoteProfilingTypes)
            {
                ProfilingType profilingType = new ProfilingType(remoteProfilingType);
                if (!_collection.ContainsKey(profilingType.Definition.Uid))
                {
                    _collection.Add(profilingType.Definition.Uid, profilingType);
                }
            }
            _initialized = true;
        }
    }
}
