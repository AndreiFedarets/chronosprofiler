using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Chronos.Client;
using Chronos.Model;

namespace Chronos.Proxy.Model
{
    public abstract class UnitCollectionProxyBase<T> : ProxyBaseObject<IUnitCollection<T>>, IUnitCollection<T>, INotifyCollectionChanged
        where T : UnitBase
    {
        private readonly object _lock;
        private readonly Dictionary<ulong, T> _dictionaryByUid;
        private readonly Dictionary<ulong, List<T>> _dictionaryById;
        private readonly ObservableCollection<T> _collection;
        private readonly RemoteEventSubscription<UnitCollectionEventArgs<T>> _unitsUpdatedSubscriber;
        private readonly SafeEventHandlerCollection<UnitCollectionEventArgs<T>> _unitsUpdatedEvent;

        protected UnitCollectionProxyBase(IUnitCollection<T> remoteObject)
            : base(remoteObject)
        {
            _lock = new object();
            _collection = new ObservableCollection<T>();
            _dictionaryByUid = new Dictionary<ulong, T>();
            _dictionaryById = new Dictionary<ulong, List<T>>();
            _unitsUpdatedSubscriber = new RemoteEventSubscription<UnitCollectionEventArgs<T>>(remoteObject, "UnitsUpdated", OnUnitServerCollectionUpdated);
            _unitsUpdatedEvent = new SafeEventHandlerCollection<UnitCollectionEventArgs<T>>();
            _collection.CollectionChanged += OnCollectionChanged;
            InitializeCollection();
        }

        public T this[uint uid]
        {
            get
            {
                T unit;
                lock (_lock)
                {
                    _dictionaryByUid.TryGetValue(uid, out unit);
                }
                return unit;
            }
        }

        public T this[ulong id, ulong lifetime]
        {
            get
            {
                lock (_lock)
                {
                    List<T> theSameIdUnits;
                    if (!_dictionaryById.TryGetValue(id, out theSameIdUnits))
                    {
                        return default(T);
                    }
                    if (theSameIdUnits.Count == 1)
                    {
                        return theSameIdUnits[0];
                    }
                    foreach (T unit in theSameIdUnits)
                    {
                        ulong beginLifetime = unit.BeginLifetime;
                        ulong endLifetime = unit.EndLifetime;
                        if (endLifetime == 0)
                        {
                            if (beginLifetime <= lifetime)
                            {
                                return unit;
                            }
                        }
                        else
                        {
                            if (beginLifetime <= lifetime && endLifetime >= lifetime)
                            {
                                return unit;
                            }
                        }
                    }
                }
                return default(T);
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _dictionaryByUid.Count;
                }
            }
        }

        public event EventHandler<UnitCollectionEventArgs<T>> UnitsUpdated
        {
            add { _unitsUpdatedEvent.Add(value); }
            remove { _unitsUpdatedEvent.Remove(value); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<T> GetEnumerator()
        {
            List<T> units;
            lock (_lock)
            {
                units = new List<T>(_dictionaryByUid.Values);
            }
            return units.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            ExecuteDispose(() =>
            {
                _unitsUpdatedSubscriber.Dispose();
                _collection.CollectionChanged -= OnCollectionChanged;
            });
            base.Dispose();
        }

        private void InitializeCollection()
        {
            lock (_lock)
            {
                _unitsUpdatedSubscriber.Subscribe();
                UpdateUnits(RemoteObject);
            }
        }

        public void OnUnitServerCollectionUpdated(object sender, UnitCollectionEventArgs<T> e)
        {
            UpdateUnits(e.Units);
        }

        private void UpdateUnits(IEnumerable<T> units)
        {
            List<T> updatedUnits = new List<T>();
            lock (_lock)
            {
                foreach (T unit in units)
                {
                    bool newUnit = false;
                    T proxyUnit;
                    if (_dictionaryByUid.TryGetValue(unit.Uid, out proxyUnit))
                    {
                        proxyUnit.Update(unit.NativeUnit);
                    }
                    else
                    {
                        proxyUnit = Convert(unit);
                        _dictionaryByUid.Add(unit.Uid, proxyUnit);
                        newUnit = true;
                    }
                    updatedUnits.Add(proxyUnit);
                    List<T> theSameIdUnits;
                    if (!_dictionaryById.TryGetValue(proxyUnit.Id, out theSameIdUnits))
                    {
                        theSameIdUnits = new List<T>();
                        _dictionaryById.Add(proxyUnit.Id, theSameIdUnits);
                    }
                    theSameIdUnits.Add(proxyUnit);
                    if (newUnit)
                    {
                        _collection.Add(proxyUnit);
                    }
                }
            }
            _unitsUpdatedEvent.Raise(this, () => new UnitCollectionEventArgs<T>(updatedUnits.ToArray()));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged.SafeInvoke(this, e);
        }

        protected abstract T Convert(T unit);
    }
}
