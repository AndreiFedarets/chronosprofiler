using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Proxy
{
    internal sealed class PrerequisiteCollection : ProxyBaseObject<IPrerequisiteCollection>, IPrerequisiteCollection
    {
        private readonly List<Prerequisite> _collection;
        private volatile bool _initialized;

        public PrerequisiteCollection(IPrerequisiteCollection prerequisites)
            : base(prerequisites)
        {
            _initialized = false;
            _collection = new List<Prerequisite>();
        }

        public List<PrerequisiteValidationResult> Validate(bool failedOnly)
        {
            VerifyDisposed();
            return Execute(() => RemoteObject.Validate(failedOnly));
        }

        public IEnumerator<IPrerequisite> GetEnumerator()
        {
            List<IPrerequisite> collection;
            lock (_collection)
            {
                VerifyDisposed();
                Initialize();
                collection = new List<IPrerequisite>(_collection);
            }
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            ExecuteDispose(() =>
            {
                List<Prerequisite> collection;
                lock (_collection)
                {
                    collection = new List<Prerequisite>(_collection);
                    _collection.Clear();
                }
                foreach (Prerequisite prerequisite in collection)
                {
                    prerequisite.Dispose();
                }
            });
            base.Dispose();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            List<IPrerequisite> remotePrerequisites = Execute(() => RemoteObject.ToList());
            foreach (IPrerequisite remotePrerequisite in remotePrerequisites)
            {
                Prerequisite prerequisite = new Prerequisite(remotePrerequisite);
                _collection.Add(prerequisite);
            }
            _initialized = true;
        }
    }
}
