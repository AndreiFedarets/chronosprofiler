using System.Collections.Generic;

namespace Rhiannon.Extensions
{
	public class ConcurrentDictionary<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> _dictionary;

		public ConcurrentDictionary()
		{
			_dictionary = new Dictionary<TKey, TValue>();
		}

	    public TValue this[TKey key]
	    {
	        get
	        {
	            lock (_dictionary)
	            {
	                return _dictionary[key];
	            }
	        }
            set
            {
                lock (_dictionary)
                {
                    _dictionary[key] = value;
                }
            }
	    }

		public bool TryGetValue(TKey key, out TValue value)
		{
			lock (_dictionary)
			{
				return _dictionary.TryGetValue(key, out value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			lock (_dictionary)
			{
				_dictionary.Add(key, value);
			}
		}

		public void Remove(TKey key)
		{
			lock (_dictionary)
			{
				_dictionary.Remove(key);
			}
		}
	}
}
