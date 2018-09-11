using System;

namespace Chronos.Client
{
    internal sealed class DefaultDispatcher : IDispatcher
    {
        public void Invoke(Action action)
        {
            action();
        }

        public void BeginInvoke(Action action)
        {
            action.BeginInvoke(null, null);
        }
    }
}
