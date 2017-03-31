using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos
{
    [Serializable]
    public abstract class UniqueSettingsCollection<T> : IEnumerable<T> where T : UniqueSettings
    {
        protected readonly Dictionary<Guid, T> Collection;

        protected UniqueSettingsCollection()
            : this(new Dictionary<Guid, T>())
        {
        }

        protected UniqueSettingsCollection(Dictionary<Guid, T> collection)
        {
            Collection = collection;
        }

        protected UniqueSettingsCollection(IEnumerable<T> collection)
        {
            Collection = collection.ToDictionary(x => x.Uid, x => x);
        }

        public T this[Guid uid]
        {
            get
            {
                lock (Collection)
                {
                    T settings;
                    Collection.TryGetValue(uid, out settings);
                    return settings;
                }
            }
        }

        public T GetOrCreate(Guid uid)
        {
            lock (Collection)
            {
                T settings;
                if (!Collection.TryGetValue(uid, out settings))
                {
                    settings = (T)Activator.CreateInstance(typeof(T), uid);
                    Collection.Add(uid, settings);
                    OnSettingsAdded(settings);
                }
                return settings;
            }
        }

        public void Add(T settings)
        {
            lock (Collection)
            {
                if (Collection.ContainsKey(settings.Uid))
                {
                    Collection[settings.Uid] = settings;
                }
                else
                {
                    Collection.Add(settings.Uid, settings);
                    OnSettingsAdded(settings);
                }
            }
        }

        public void Remove(Guid uid)
        {
            lock (Collection)
            {
                T settings;
                if (Collection.TryGetValue(uid, out settings))
                {
                    Collection.Remove(uid);
                    OnSettingsRemoved(settings);
                }
            }
        }

        public bool Contains(Guid uid)
        {
            lock (Collection)
            {
                return Collection.ContainsKey(uid);
            }
        }

        public void Clear()
        {
            lock (Collection)
            {
                Collection.Clear();
                OnCollectionCleared();
            }
        }

        public virtual void Validate()
        {
            lock (Collection)
            {
                foreach (T settings in Collection.Values)
                {
                    settings.Validate();
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> collection;
            lock (Collection)
            {
                collection = new List<T>(Collection.Values);
            }
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected Dictionary<Guid, T> CloneSettings()
        {
            Dictionary<Guid, T> collection = new Dictionary<Guid, T>();
            lock (Collection)
            {
                foreach (KeyValuePair<Guid, T> item in Collection)
                {
                    collection.Add(item.Key, (T)item.Value.Clone());
                }
            }
            return collection;
        }

        protected virtual void OnSettingsAdded(T settings)
        {
            
        }

        protected virtual void OnSettingsRemoved(T settings)
        {

        }

        protected virtual void OnCollectionCleared()
        {

        }
    }
}
