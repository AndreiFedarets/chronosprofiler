using System;

namespace Adenium
{
    public interface IDispatcher
    {
        void Invoke(Action action);

        T Invoke<T>(Func<T> action);

        void BeginInvoke(Action action);
    }
}
