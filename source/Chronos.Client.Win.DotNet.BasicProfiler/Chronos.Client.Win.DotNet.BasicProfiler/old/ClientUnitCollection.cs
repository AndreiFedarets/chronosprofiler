using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using Chronos.Daemon.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal abstract class ClientUnitCollection<TC, TD> : IDisposable, IEnumerable<TC>, INotifyCollectionChanged
        where TC : ClientUnitBase
        where TD : DaemonUnitBase
    {
        private readonly Dispatcher _dispatcher;
        private readonly object _lock;
        private IDaemonUnitCollection<TD> _daemonUnits;
        private readonly ICollectionView _collectionView;
        private readonly Dictionary<uint, TC> _dictionaryByUid;
        private readonly Dictionary<ulong, List<TC>> _dictionaryById;
        private readonly ObservableCollection<TC> _collection;
        private readonly SafeEventSubscriber<DaemonUnitCollectionEventArgs<TD>> _unitsUpdated;

        protected ClientUnitCollection()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _lock = new object();
            _collection = new ObservableCollection<TC>();
            _dictionaryByUid = new Dictionary<uint, TC>();
            _dictionaryById = new Dictionary<ulong, List<TC>>();
            _collectionView = CollectionViewSource.GetDefaultView(_collection);
            _unitsUpdated = new SafeEventSubscriber<DaemonUnitCollectionEventArgs<TD>>(OnUnitServerCollectionUpdated);
            _collection.CollectionChanged += OnCollectionChanged;
        }

        public TC this[uint uid]
        {
            get
            {
                TC unit;
                lock (_lock)
                {
                    _dictionaryByUid.TryGetValue(uid, out unit);
                }
                return unit;
            }
        }

        public TC this[ulong id, uint lifetime]
        {
            get
            {
                lock (_lock)
                {
                    List<TC> theSameIdUnits;
                    if (!_dictionaryById.TryGetValue(id, out theSameIdUnits))
                    {
                        return default(TC);
                    }
                    if (theSameIdUnits.Count == 1)
                    {
                        return theSameIdUnits[0];
                    }
                    foreach (TC unit in theSameIdUnits)
                    {
                        uint beginLifetime = unit.BeginLifetime;
                        uint endLifetime = unit.EndLifetime == 0 ? (uint)Environment.TickCount : unit.EndLifetime;
                        if (beginLifetime <= lifetime && endLifetime >= lifetime)
                        {
                            return unit;
                        }
                    }
                }
                return default(TC);
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

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<TC> GetEnumerator()
        {
            List<TC> units;
            lock (_lock)
            {
                units = new List<TC>(_dictionaryByUid.Values);
            }
            return units.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IDisposable.Dispose()
        {
            _daemonUnits.UnitsUpdated -= _unitsUpdated.OnEvent;
            _collection.CollectionChanged -= OnCollectionChanged;
        }

        internal void Initialize(IDaemonUnitCollection<TD> daemonUnits)
        {
            _daemonUnits = daemonUnits;
            lock (_lock)
            {
                _daemonUnits.UnitsUpdated += _unitsUpdated.OnEvent;
                UpdateUnits(_daemonUnits);
            }
        }

        protected abstract TC CreateClientUnit(TD daemonUnit);

        public void OnUnitServerCollectionUpdated(object sender, DaemonUnitCollectionEventArgs<TD> e)
        {
            Action action = () => UpdateUnits(e.Units);
            _dispatcher.BeginInvoke(action);
        }

        private void UpdateUnits(IEnumerable<TD> daemonUnits)
        {
            lock (_lock)
            {
                foreach (TD daemonUnit in daemonUnits)
                {
                    bool newUnit = false;
                    TC clientUnit;
                    if (_dictionaryByUid.TryGetValue(daemonUnit.Uid, out clientUnit))
                    {
                        clientUnit.Update(daemonUnit);
                    }
                    else
                    {
                        clientUnit = CreateClientUnit(daemonUnit);
                        newUnit = true;
                    }
                    _dictionaryByUid[clientUnit.Uid] = clientUnit;
                    List<TC> theSameIdUnits;
                    if (!_dictionaryById.TryGetValue(clientUnit.Id, out theSameIdUnits))
                    {
                        theSameIdUnits = new List<TC>();
                        _dictionaryById.Add(clientUnit.Id, theSameIdUnits);
                    }
                    theSameIdUnits.Add(clientUnit);
                    if (newUnit)
                    {
                        _collection.Add(clientUnit);
                    }
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged.SafeInvoke(this, e);
        }
    }
}
