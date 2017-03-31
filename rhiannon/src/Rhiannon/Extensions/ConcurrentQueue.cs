using System.Collections.Generic;

namespace Rhiannon.Extensions
{
	public class ConcurrentQueue<T>
	{
		private readonly Queue<T> _queue;

		public ConcurrentQueue()
		{
			_queue = new Queue<T>();
		}

		public int Count
		{
			get
			{
				lock (_queue)
				{
					return _queue.Count;
				}
			}
		}

		public T Peek()
		{
			lock (_queue)
			{
				return _queue.Peek();
			}
		}

		public void Enqueue(T item)
		{
			lock (_queue)
			{
				_queue.Enqueue(item);
			}
		}

		public T Dequeue()
		{
			lock (_queue)
			{
				return _queue.Dequeue();
			}
		}

		public T DequeueOrDefault()
		{
			lock (_queue)
			{
				if (_queue.Count > 0)
				{
					return _queue.Dequeue();
				}
				return default(T);
			}
		}
	}
}
