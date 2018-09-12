using System;
using System.Collections.Generic;
using System.Diagnostics;
using Chronos.Storage;

namespace Chronos.Model
{
    public abstract class UnitCollectionBase<TUnit, TNativeUnit> : RemoteBaseObject, IUnitCollection<TUnit>
        where TUnit : UnitBase
        where TNativeUnit : NativeUnitBase
    {
        private readonly Dictionary<ulong, TUnit> _dictionaryByUid;
        private readonly Dictionary<ulong, List<TUnit>> _dictionaryById;
        private readonly SafeEventHandlerCollection<UnitCollectionEventArgs<TUnit>> _unitsUpdated;

        protected UnitCollectionBase()
        {
            _dictionaryByUid = new Dictionary<ulong, TUnit>();
            _dictionaryById = new Dictionary<ulong, List<TUnit>>();
            _unitsUpdated = new SafeEventHandlerCollection<UnitCollectionEventArgs<TUnit>>();
        }

        public TUnit this[uint uid]
        {
            get
            {
                TUnit unit;
                lock (Lock)
                {
                    _dictionaryByUid.TryGetValue(uid, out unit);
                }
                return unit;
            }
        }

        public TUnit this[ulong id, ulong lifetime]
        {
            get
            {
                lock (Lock)
                {
                    List<TUnit> theSameIdUnits;
                    if (!_dictionaryById.TryGetValue(id, out theSameIdUnits))
                    {
                        return default(TUnit);
                    }
                    if (theSameIdUnits.Count == 1)
                    {
                        return theSameIdUnits[0];
                    }
                    foreach (TUnit unit in theSameIdUnits)
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
                return default(TUnit);
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

        public event EventHandler<UnitCollectionEventArgs<TUnit>> UnitsUpdated
        {
            add { _unitsUpdated.Add(value);}
            remove { _unitsUpdated.Remove(value); }
        }

        public void Update(TNativeUnit[] units)
        {
            TUnit[] updated = new TUnit[units.Length];
            lock (Lock)
            {
                for (int i = 0; i < units.Length; i++ )
                {
                    updated[i] = UpdateUnitInternal(units[i]);
                }
            }
            RaiseUnitsUpdatedEvent(updated);
        }

        public void Update(TNativeUnit unit)
        {
            TUnit[] updated = new TUnit[1];
            lock (Lock)
            {
                updated[0] = UpdateUnitInternal(unit);
            }
            RaiseUnitsUpdatedEvent(updated);
        }

        public IEnumerator<TUnit> GetEnumerator()
        {
            List<TUnit> units;
            lock (Lock)
            {
                units = new List<TUnit>(_dictionaryByUid.Values);
            }
            return units.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Save(IDataStorage storage)
        {
            //IDataTable<TUnit> table = storage.OpenTable<TUnit>();
            //table.Add(_dictionaryByUid.Values);
        }

        public void Load(IDataStorage storage)
        {
            //IDataTable<TUnit> table = storage.OpenTable<TUnit>();
            //IEnumerable<TUnit> units = table;
            //Update(units.ToArray());
        }

        private TUnit UpdateUnitInternal(TNativeUnit nativeUnit)
        {
            TUnit unit;
            if (_dictionaryByUid.TryGetValue(nativeUnit.Uid, out unit))
            {
                unit.Update(nativeUnit);
            }
            else
            {
                unit = Convert(nativeUnit);
                _dictionaryByUid.Add(unit.Uid, unit);
                List<TUnit> theSameIdUnits;
                if (!_dictionaryById.TryGetValue(unit.Id, out theSameIdUnits))
                {
                    theSameIdUnits = new List<TUnit>();
                    _dictionaryById.Add(unit.Id, theSameIdUnits);
                }
                theSameIdUnits.Add(unit);
            }
            return unit;
        }

        protected abstract TUnit Convert(TNativeUnit nativeUnit);

        private void RaiseUnitsUpdatedEvent(TUnit[] units)
        {
            try
            {
                _unitsUpdated.Raise(this, () => new UnitCollectionEventArgs<TUnit>(units));
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
            }
        }
    }
}
