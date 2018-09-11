using Chronos.Communication.Managed;
using System;
using System.Collections.Generic;

namespace Chronos.Host
{
    public interface IApplicationCollection : IEnumerable<IApplication>
    {
        event EventHandler<ApplicationEventArgs> ApplicationConnected;

        event EventHandler<ApplicationEventArgs> ApplicationDisconnected;

        IApplication Connect(ConnectionSettings connectionSettings);

        IApplication ConnectLocal(bool runIfNotLaunched);

        bool Disconnect(IApplication application);
    }
}
