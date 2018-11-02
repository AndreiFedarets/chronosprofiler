using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.Common.EventsTree
{
    public class Reference<T1>
    {
        public Reference(T1 item)
        {
            Item = item;
        }

        public virtual long Count
        {
            get { return 1; }
        }

        public T1 Item { get; private set; }
    }

    public class Reference<T1, T2> : Reference<T1>
    {
        private readonly Dictionary<T2, Reference<T2>> _collection;

        public Reference(T1 item)
            : base(item)
        {
            _collection = new Dictionary<T2, Reference<T2>>();
        }

        public override long Count
        {
            get { return Collection.Sum(x => x.Count); }
        }

        public IEnumerable<Reference<T2>> Collection
        {
            get { return _collection.Values; }
        }

        public Reference<T2> this[T2 item2]
        {
            get
            {
                Reference<T2> reference;
                if (!_collection.TryGetValue(item2, out reference))
                {
                    reference = new Reference<T2>(item2);
                    _collection.Add(item2, reference);
                }
                return reference;
            }
        }

        public void Add(T2 item2)
        {
            Reference<T2> reference;
            if (_collection.TryGetValue(item2, out reference))
            {

            }
            else
            {
                _collection.Add(item2, new Reference<T2>(item2));
            }
        }
    }


    public class Reference<T1, T2, T3> : Reference<T1>
    {
        private readonly Dictionary<T2, Reference<T2, T3>> _collection;

        public Reference(T1 item)
            : base(item)
        {
            _collection = new Dictionary<T2, Reference<T2, T3>>();
        }

        public override long Count
        {
            get { return Collection.Sum(x => x.Count); }
        }

        public IEnumerable<Reference<T2, T3>> Collection
        {
            get { return _collection.Values; }
        }

        public Reference<T2, T3> this[T2 item2]
        {
            get
            {
                Reference<T2, T3> reference;
                if (!_collection.TryGetValue(item2, out reference))
                {
                    reference = new Reference<T2, T3>(item2);
                    _collection.Add(item2, reference);
                }
                return reference;
            }
        }

        public void Add(T2 item2, T3 item3)
        {
            Reference<T2, T3> reference;
            if (_collection.TryGetValue(item2, out reference))
            {
                reference.Add(item3);
            }
            else
            {
                reference = new Reference<T2, T3>(item2);
                reference.Add(item3);
                _collection.Add(item2, reference);
            }
        }
    }

    public class HeaderReference<T1, T2> : Reference<byte, T1, T2>
    {
        public HeaderReference(byte eventType)
            : base(eventType)
        {
        }

    }
}
