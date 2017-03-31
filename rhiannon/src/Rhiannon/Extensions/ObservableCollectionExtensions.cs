using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rhiannon.Extensions
{
	public static class ObservableCollectionExtensions
	{
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
		{
			return new ObservableCollection<T>(collection);
		}
	}
}
