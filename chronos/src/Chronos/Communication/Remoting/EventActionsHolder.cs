using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace Chronos.Communication.Remoting
{
	internal class EventActionsHolder<T>
	{
		private readonly IList<Action<T>> _actions;

		public EventActionsHolder()
		{
			_actions = new List<Action<T>>();
		}

		public void Invoke(T arg)
		{
			lock (_actions)
			{
				IList<Action<T>> brokenActions = new List<Action<T>>();
				foreach (Action<T> action in _actions)
				{
					try
					{
						action.Invoke(arg);
					}
					catch (RemotingException)
					{
						brokenActions.Add(action);
					}
				}
				foreach (Action<T> action in brokenActions)
				{
					_actions.Remove(action);
				}
				brokenActions.Clear();
			}
		}

		public void Add(Action<T> action)
		{
			lock (_actions)
			{
				_actions.Add(action);
			}
		}

		public void Remove(Action<T> action)
		{
			lock (_actions)
			{
				_actions.Remove(action);
			}
		}
	}

	internal class EventActionsHolder
	{
		private readonly IList<Action> _actions;

		public EventActionsHolder()
		{
			_actions = new List<Action>();
		}

		public void Invoke()
		{
			lock (_actions)
			{
				IList<Action> brokenActions = new List<Action>();
				foreach (Action action in _actions)
				{
					try
					{
						action.Invoke();
					}
					catch (RemotingException)
					{
						brokenActions.Add(action);
					}
				}
				foreach (Action action in brokenActions)
				{
					_actions.Remove(action);
				}
				brokenActions.Clear();
			}
		}

		public void Add(Action action)
		{
			lock (_actions)
			{
				_actions.Add(action);
			}
		}

		public void Remove(Action action)
		{
			lock (_actions)
			{
				_actions.Remove(action);
			}
		}
	}
}
