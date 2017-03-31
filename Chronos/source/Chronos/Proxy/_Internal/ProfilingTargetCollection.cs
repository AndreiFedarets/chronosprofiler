using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Proxy
{
    internal sealed class ProfilingTargetCollection : ProxyBaseObject<IProfilingTargetCollection>, IProfilingTargetCollection
    {
        private readonly Dictionary<Guid, ProfilingTarget> _collection;
        private volatile bool _initialized;

        public ProfilingTargetCollection(IProfilingTargetCollection profilingTargets)
            : base(profilingTargets)
        {
            _initialized = false;
            _collection = new Dictionary<Guid, ProfilingTarget>();
        }

        public IProfilingTarget this[Guid uid]
        {
            get
            {
                lock (_collection)
                {
                    VerifyDisposed();
                    ProfilingTarget profilingTarget;
                    if (_collection.TryGetValue(uid, out profilingTarget))
                    {
                        return profilingTarget;
                    }
                    IProfilingTarget remoteProfilingTarget = Execute(() => RemoteObject[uid]);
                    profilingTarget = new ProfilingTarget(remoteProfilingTarget);
                    _collection.Add(uid, profilingTarget);
                    return profilingTarget;
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

        public bool TryGet(Guid uid, out IProfilingTarget item)
        {
            lock (_collection)
            {
                ProfilingTarget profilingTarget;
                if (_collection.TryGetValue(uid, out profilingTarget))
                {
                    item = profilingTarget;
                    return true;
                }
                IProfilingTarget remoteProfilingTarget = null;
                if (Execute(() => RemoteObject.TryGet(uid, out remoteProfilingTarget)))
                {
                    profilingTarget = new ProfilingTarget(remoteProfilingTarget);
                    _collection.Add(uid, profilingTarget);
                    item = profilingTarget;
                    return true;
                }
            }
            item = null;
            return false;
        }

        public IEnumerator<IProfilingTarget> GetEnumerator()
        {
            List<IProfilingTarget> collection;
            lock (_collection)
            {
                VerifyDisposed();
                Initialize();
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
            lock (_collection)
            {
                VerifyDisposed();
                foreach (ProfilingTarget profilingTarget in _collection.Values)
                {
                    profilingTarget.Dispose();
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
            List<IProfilingTarget> remoteProfilingTargets = Execute(() => RemoteObject.ToList());
            if (remoteProfilingTargets.Count == _collection.Count)
            {
                return;
            }
            foreach (IProfilingTarget remoteProfilingTarget in remoteProfilingTargets)
            {
                ProfilingTarget profilingTarget = new ProfilingTarget(remoteProfilingTarget);
                if (!_collection.ContainsKey(profilingTarget.Definition.Uid))
                {
                    _collection.Add(profilingTarget.Definition.Uid, profilingTarget);
                }
            }
            _initialized = true;
        }
    }
}
