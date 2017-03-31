using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Chronos.Core;
using Chronos.Core.Internal;
using Chronos.Daemon.Internal;

namespace Chronos.Daemon.Proxy
{
    internal class UnitCollection<T> : IUnitCollection<T> where T : UnitBase, new()
    {
        protected readonly T Default;
	    private readonly IProcessShadow _processShadow;
		private UnitCollectionEventsRouter<T> _eventsRouter;
		private readonly IUnitCollection<T> _collection;
		protected readonly IDictionary<uint, T> DictionaryById;
		protected readonly IDictionary<ulong, IList<T>> DictionaryByManagedId;
		private readonly object _lock;
		private int _revision;

        public UnitCollection(IProcessShadow processShadow, IUnitCollection<T> collection, uint unitType)
        {
            Default = new T();
			_lock = new object();
		    _processShadow = processShadow;
			_collection = collection;
			_revision = -1;
			DictionaryById = new Dictionary<uint, T>();
			DictionaryByManagedId = new Dictionary<ulong, IList<T>>();
			UnitType = unitType;
			Initialize();
		}

		private void Initialize()
		{
			using (_collection.Lock())
			{
				T[] units = _collection.ToArray();
				int revision = _collection.Revision;
				Update(units);
				_revision = revision;
				_eventsRouter = new UnitCollectionEventsRouter<T>(_collection, Update);
			}
		}

		public int Count
		{
			get { return DictionaryById.Count; }
		}

		public int Revision
		{
			get { return _revision; }
		}

		public T this[uint id]
		{
			get
			{
				T unit;
                DictionaryById.TryGetValue(id, out unit);
                if (unit == default(T))
                {
                    return Default;
                }
				return unit;
			}
		}

		public T this[ulong managedId, uint lifetime]
		{
			get
			{
				IList<T> theSameManagedIdUnits;
				if (!DictionaryByManagedId.TryGetValue(managedId, out  theSameManagedIdUnits))
				{
                    return Default;
				}
				foreach (T unit in theSameManagedIdUnits)
				{
					uint beginLifetime = unit.BeginLifetime;
					uint endLifetime = unit.EndLifetime == 0 ? (uint)Environment.TickCount : unit.EndLifetime;
					if (beginLifetime <= lifetime && endLifetime >= lifetime)
					{
						return unit;
					}
                }
                return Default;
			}
		}

        public uint UnitType { get; private set; }

		public void Update(T[] units)
		{
			Monitor.Enter(_lock);
			_revision++;
			foreach (T unit in units)
			{
				UpdateUnit(unit);
			}
			InvokeUnitsUpdated(units);
			Monitor.Exit(_lock);
		}

		private void UpdateUnit(T unit)
		{
			unit.Revision = _revision;
			if (DictionaryById.ContainsKey(unit.Id))
			{
				DictionaryById[unit.Id] = unit;
			}
			else
			{
				DictionaryById.Add(unit.Id, unit);
			}
			IList<T> theSameManagedIdUnits;
			if (!DictionaryByManagedId.TryGetValue(unit.ManagedId, out theSameManagedIdUnits))
			{
				theSameManagedIdUnits = new List<T>();
				DictionaryByManagedId.Add(unit.ManagedId, theSameManagedIdUnits);
			}
			theSameManagedIdUnits.Add(unit);
            unit.SetProcessShadow(_processShadow);
		}

		public void Update(T unit)
		{
			Monitor.Enter(_lock);
			_revision++;
			UpdateUnit(unit);
			InvokeUnitsUpdated(new[] { unit });
			Monitor.Exit(_lock);
		}

		public event Action<T[]> UnitsUpdated;

		private void InvokeUnitsUpdated(T[] units)
		{
			Action<T[]> handler = UnitsUpdated;
			if (handler != null)
			{
				handler(units);
			}
		}

		public List<T> Snapshot()
		{
			Monitor.Enter(_lock);
			List<T> collection = new List<T>(DictionaryById.Values);
			Monitor.Exit(_lock);
			return collection;
		}

		public IDisposable Lock()
		{
			return new DisposableMonitor(_lock);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return DictionaryById.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
