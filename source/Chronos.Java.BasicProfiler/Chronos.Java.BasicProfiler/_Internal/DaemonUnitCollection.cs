using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Storage;

namespace Chronos.Daemon.DotNet.BasicProfiler
{
    internal abstract class DaemonUnitCollection<T> : RemoteBaseObject, IEnumerable<T> where T : DaemonUnitBase
    {
        private readonly Dictionary<uint, T> _dictionaryByUid;
        private readonly Dictionary<ulong, List<T>> _dictionaryById;
        private readonly SafeEventHandlerCollection<DaemonUnitCollectionEventArgs<T>> _unitsUpdated;

        protected DaemonUnitCollection()
        {
            _dictionaryByUid = new Dictionary<uint, T>();
            _dictionaryById = new Dictionary<ulong, List<T>>();
            _unitsUpdated = new SafeEventHandlerCollection<DaemonUnitCollectionEventArgs<T>>();
        }

        public T this[uint uid]
        {
            get
            {
                T unit;
                lock (Lock)
                {
                    _dictionaryByUid.TryGetValue(uid, out unit);
                }
                return unit;
            }
        }

        public T this[ulong id, uint lifetime]
        {
            get
            {
                lock (Lock)
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
                        uint beginLifetime = unit.BeginLifetime;
                        uint endLifetime = unit.EndLifetime == 0 ? (uint)Environment.TickCount : unit.EndLifetime;
                        if (beginLifetime <= lifetime && endLifetime >= lifetime)
                        {
                            return unit;
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
                lock (Lock)
                {
                    return _dictionaryByUid.Count;
                }
            }
        }

        public event EventHandler<DaemonUnitCollectionEventArgs<T>> UnitsUpdated
        {
            add { _unitsUpdated.Add(value);}
            remove { _unitsUpdated.Remove(value); }
        }

        public void Update(T[] units)
        {
            lock (Lock)
            {
                foreach (T unit in units)
                {
                    UpdateUnitInternal(unit);
                }
            }
            RaiseUnitsUpdatedEvent(units);
        }

        public void Update(T unit)
        {
            lock (Lock)
            {
                UpdateUnitInternal(unit);
            }
            RaiseUnitsUpdatedEvent(new[] {unit});
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> units;
            lock (Lock)
            {
                units = new List<T>(_dictionaryByUid.Values);
            }
            return units.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void UpdateUnitInternal(T unit)
        {
            _dictionaryByUid[unit.Uid] = unit;
            List<T> theSameIdUnits;
            if (!_dictionaryById.TryGetValue(unit.Id, out theSameIdUnits))
            {
                theSameIdUnits = new List<T>();
                _dictionaryById.Add(unit.Id, theSameIdUnits);
            }
            theSameIdUnits.Add(unit);
        }

        private void RaiseUnitsUpdatedEvent(T[] units)
        {
            _unitsUpdated.Raise(this, () => new DaemonUnitCollectionEventArgs<T>(units));
        }

        public void Save(IDataStorage storage)
        {
            IDataTable<T> table = storage.OpenTable<T>();
            table.Add(_dictionaryByUid.Values);
        }

        public void Load(IDataStorage storage)
        {
            IDataTable<T> table = storage.OpenTable<T>();
            IEnumerable<T> units = table;
            Update(units.ToArray());
        }
    }
}
