using System;

namespace Adenium
{
    public static class EventExtensions
    {
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
