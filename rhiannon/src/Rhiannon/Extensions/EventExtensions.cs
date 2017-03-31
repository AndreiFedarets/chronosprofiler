using System;

namespace Rhiannon.Extensions
{
	public static class EventExtensions
	{
		public static void SafeInvoke(this Action action)
		{
			Action handler = action;
			if (handler != null)
			{
				handler();
			}
		}

		public static void SafeInvoke<T>(this Action<T> action, T value)
		{
			Action<T> handler = action;
			if (handler != null)
			{
				handler(value);
			}
		}

		public static void SafeInvoke<T>(this EventHandler<T> @event, object sender, T eventArgs) where T : EventArgs
		{
			EventHandler<T> handler = @event;
			if (handler != null)
			{
				handler(sender, eventArgs);
			}
		}

		public static void SafeInvoke(this EventHandler @event, object sender, EventArgs eventArgs)
		{
			EventHandler handler = @event;
			if (handler != null)
			{
				handler(sender, eventArgs);
			}
		}
	}
}
