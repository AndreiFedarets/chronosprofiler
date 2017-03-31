using System;
using System.Collections.Generic;
using Chronos.Communication.Remoting;
using System.Threading;
using Chronos.Core.Internal;

namespace Chronos.Core
{
	public abstract class UnitCollection<T> : MarshalByRefObject, IUnitCollection<T> where T : UnitBase, new()
	{
	    protected readonly T Default;
		protected readonly IDictionary<uint, T> DictionaryById;
		protected readonly IDictionary<ulong, IList<T>> DictionaryByManagedId;
		private readonly EventActionsHolder<T[]> _unitsUpdatedEvent;
		private readonly object _lock;
		private int _revision;

		protected UnitCollection()
		{
            Default = new T();
			_revision = -1;
			_lock = new object();
			DictionaryById = new Dictionary<uint, T>();
			DictionaryByManagedId = new Dictionary<ulong, IList<T>>();
			_unitsUpdatedEvent = new EventActionsHolder<T[]>();
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public int Count
		{
			get { return DictionaryById.Count; }
		}

        public abstract uint UnitType { get; }

		public event Action<T[]> UnitsUpdated
		{
			add { _unitsUpdatedEvent.Add(value); }
			remove { _unitsUpdatedEvent.Remove(value); }
		}

		public int Revision
		{
			get { return _revision; }
		}

		public T this[uint id]
		{
			get
			{
				T unit = null;
				for (int i = 0; i < 100; i++)
				{
					if (DictionaryById.TryGetValue(id, out unit))
					{
						break;
					}
					Thread.Sleep(50);
				}
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
					return default(T);
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

		public void Update(T[] units)
		{
			Monitor.Enter(_lock);
			_revision++;
			foreach (T unit in units)
			{
                UpdateUnitInternal(unit);
			}
		    RaiseUnitsUpdatedEvent(units);
			Monitor.Exit(_lock);
		}

		public void Update(T unit)
		{
			Monitor.Enter(_lock);
			_revision++;
            UpdateUnitInternal(unit);
            RaiseUnitsUpdatedEvent(new[] { unit });
			Monitor.Exit(_lock);
		}

        internal void UpdateUnitInternal(T unit)
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

        protected void RaiseUnitsUpdatedEvent(T[] units)
        {
            _unitsUpdatedEvent.Invoke(units);
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
