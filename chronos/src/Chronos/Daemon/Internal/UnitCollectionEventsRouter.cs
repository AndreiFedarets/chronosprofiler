using System;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
	internal class UnitCollectionEventsRouter<T> : MarshalByRefObject, IDisposable where T : UnitBase
	{
		private readonly IUnitCollection<T> _collection;
		private readonly Action<T[]> _unitsUpdated;

		public UnitCollectionEventsRouter(IUnitCollection<T> collection, Action<T[]> unitsUpdated)
		{
			_collection = collection;
			_unitsUpdated = unitsUpdated;
			_collection.UnitsUpdated += OnUnitsUpdated;
		}

		public void OnUnitsUpdated(T[] units)
		{
			_unitsUpdated.Invoke(units);
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public void Dispose()
		{
			_collection.UnitsUpdated -= OnUnitsUpdated;
		}
	}
}
