using System;
using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IUnitCollection<T> : IEnumerable<T> where T : UnitBase
	{
		int Revision { get; }

		T this[uint id] { get; }

		T this[ulong managedId, uint lifetime] { get; }

        uint UnitType { get; }

		void Update(T[] units);

		void Update(T unit);

		//T[] Select(int startRevision, int endRevision);

		event Action<T[]> UnitsUpdated;

		int Count { get; }

		List<T> Snapshot();

		IDisposable Lock();
	}
}
