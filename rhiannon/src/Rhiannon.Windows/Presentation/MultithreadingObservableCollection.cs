using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Rhiannon.Windows.Presentation
{
	public class MultithreadingObservableCollection<T> : ObservableCollection<T>
	{
		private readonly Dispatcher _dispatcher;

		private delegate void SetItemCallback(int index, T item);
		private delegate void RemoveItemCallback(int index);
		private delegate void ClearItemsCallback();
		private delegate void InsertItemCallback(int index, T item);
		private delegate void MoveItemCallback(int oldIndex, int newIndex);

		public Dispatcher Dispatcher
		{
			get { return _dispatcher; }
		}

		public MultithreadingObservableCollection(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public MultithreadingObservableCollection()
		{
			_dispatcher = Dispatcher.CurrentDispatcher;
		}

		public MultithreadingObservableCollection(IEnumerable<T> collection)
			: base(collection)
		{
			_dispatcher = Dispatcher.CurrentDispatcher;
		}

		protected override void SetItem(int index, T item)
		{
			if (_dispatcher.CheckAccess())
			{
				base.SetItem(index, item);
			}
			else
			{
				_dispatcher.Invoke(DispatcherPriority.Send, new SetItemCallback(base.SetItem), index, new object[] { item });
			}
		}

		protected override void RemoveItem(int index)
		{
			if (_dispatcher.CheckAccess())
			{
				base.RemoveItem(index);
			}
			else
			{
				_dispatcher.Invoke(DispatcherPriority.Send, new RemoveItemCallback(base.RemoveItem), index);
			}
		}

		protected override void ClearItems()
		{
			if (_dispatcher.CheckAccess())
			{
				base.ClearItems();
			}
			else
			{
				_dispatcher.Invoke(DispatcherPriority.Send, new ClearItemsCallback(base.ClearItems));
			}
		}

		protected override void InsertItem(int index, T item)
		{
			if (_dispatcher.CheckAccess())
			{
				base.InsertItem(index, item);
			}
			else
			{
				_dispatcher.Invoke(DispatcherPriority.Send, new InsertItemCallback(base.InsertItem), index, new object[] { item });
			}
		}

		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (_dispatcher.CheckAccess())
			{
				base.MoveItem(oldIndex, newIndex);
			}
			else
			{
				_dispatcher.Invoke(DispatcherPriority.Send, new MoveItemCallback(base.MoveItem), oldIndex, new object[] { newIndex });
			}
		}
	}
}
