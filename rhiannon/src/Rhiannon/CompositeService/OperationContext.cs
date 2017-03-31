using System;
using System.Collections.Generic;

namespace Rhiannon.CompositeService
{
	public class OperationContext
	{
		private readonly IDictionary<Type, object> _items;

		public OperationContext()
		{
			_items = new Dictionary<Type, object>();
		}

		public void Set<T>(T item)
		{
			_items.Add(typeof(T), item);
		}

		public T Get<T>()
		{
			return (T)_items[typeof (T)];
		}
	}
}
