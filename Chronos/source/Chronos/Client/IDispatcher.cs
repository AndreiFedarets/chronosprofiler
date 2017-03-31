using System;

namespace Chronos.Client
{
    public interface IDispatcher
    {
        void Invoke(Action action);

        void BeginInvoke(Action action);
    }
}
