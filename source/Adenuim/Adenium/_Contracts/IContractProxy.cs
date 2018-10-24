using System;

namespace Adenium
{
    public interface IContractProxy
    {
        object UnderlyingObject { get; }

        event EventHandler<ContractProxyObjectChangedEventArgs> UnderlyingObjectChanged;
    }
}
