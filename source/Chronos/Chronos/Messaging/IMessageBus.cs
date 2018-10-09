using System;

namespace Chronos.Messaging
{
    public interface IMessageBus
    {
        void SendMessage(object sender, uint message, object parameter);

        void PostMessage(object sender, uint message, object parameter);

        IDisposable Subscribe(IMessageBusHandler handler);
    }
}
