using System;
using System.Runtime.Remoting;

namespace Chronos.Communication.Remoting
{
    public class RemotingExecutor : IRemotingExecutor
    {
        public event Action ConnectionRestored;

        public void OnConnectionRestored()
        {
            Action handler = ConnectionRestored;
            if (handler != null)
            {
                handler();
            }
        }

        public event Action ConnectionFailed;

        public void NotifyConnectionFailed()
        {
            Action handler = ConnectionFailed;
            if (handler != null)
            {
                handler();
            }
        }

        public T Execute<T>(Func<T> func)
        {
            try
            {
                T result = func();
                return result;
            }
            catch (RemotingException)
            {
                NotifyConnectionFailed();
                return default(T);
            }
        }

        public void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (RemotingException)
            {
                NotifyConnectionFailed();
            }
        }
    }
}
