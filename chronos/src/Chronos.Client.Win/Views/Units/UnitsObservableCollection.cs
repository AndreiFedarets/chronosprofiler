using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chronos.Core;

namespace Chronos.Client.Win.Views.Units
{
	internal class UnitsObservableCollection<T> : ObservableCollection<T> where T : UnitBase
	{
		private readonly IDictionary<uint, T> _dictionary;
 
		public UnitsObservableCollection(IEnumerable<T> units)
			: base(units)
		{
			_dictionary = this.ToDictionary(x => x.Id, x => x);
		}

		public void Update(T[] units)
		{
			foreach (T unit in units)
			{
				T original;
				if (_dictionary.TryGetValue(unit.Id, out original))
				{
					Remove(original);
					_dictionary.Remove(unit.Id);
					Add(unit);
					_dictionary.Add(unit.Id, unit);
				}
				else
				{
					Add(unit);
					_dictionary.Add(unit.Id, unit);
				}
			}
		}
	}
}
