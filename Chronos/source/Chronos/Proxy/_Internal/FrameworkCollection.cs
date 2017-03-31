using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Proxy
{
    internal sealed class FrameworkCollection : ProxyBaseObject<IFrameworkCollection>, IFrameworkCollection
    {
        private readonly Dictionary<Guid, Framework> _collection;
        private volatile bool _initialized;

        public FrameworkCollection(IFrameworkCollection frameworks)
            : base(frameworks)
        {
            _initialized = false;
            _collection = new Dictionary<Guid, Framework>();
        }

        public IFramework this[Guid uid]
        {
            get
            {
                lock (_collection)
                {
                    VerifyDisposed();
                    Framework framework;
                    if (_collection.TryGetValue(uid, out framework))
                    {
                        return framework;
                    }
                    IFramework remoteFramework = Execute(() => RemoteObject[uid]);
                    framework = new Framework(remoteFramework);
                    _collection.Add(uid, framework);
                    return framework;
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

        public bool TryGet(Guid uid, out IFramework item)
        {
            lock (_collection)
            {
                Framework framework;
                if (_collection.TryGetValue(uid, out framework))
                {
                    item = framework;
                    return true;
                }
                IFramework remoteFramework = null;
                if (Execute(() => RemoteObject.TryGet(uid, out remoteFramework)))
                {
                    framework = new Framework(remoteFramework);
                    _collection.Add(uid, framework);
                    item = framework;
                    return true;
                }
            }
            item = null;
            return false;
        }

        public IEnumerator<IFramework> GetEnumerator()
        {
            List<IFramework> collection;
            lock (_collection)
            {
                VerifyDisposed();
                Initialize();
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
            lock (_collection)
            {
                VerifyDisposed();
                foreach (Framework framework in _collection.Values)
                {
                    framework.Dispose();
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
            List<IFramework> remoteFrameworks = Execute(() => RemoteObject.ToList());
            if (remoteFrameworks.Count == _collection.Count)
            {
                return;
            }
            foreach (IFramework remoteFramework in remoteFrameworks)
            {
                Framework framework = new Framework(remoteFramework);
                if (!_collection.ContainsKey(framework.Definition.Uid))
                {
                    _collection.Add(framework.Definition.Uid, framework);
                }
            }
            _initialized = true;
        }
    }
}
