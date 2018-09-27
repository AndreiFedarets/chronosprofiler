using System;

namespace Chronos.Client
{
    public interface IDispatcher
    {
        void Invoke(Action action);

        T Invoke<T>(Func<T> action);

        void BeginInvoke(Action action);
    }
}
