using System;

namespace Chronos.Client.Win.Contracts
{
    public interface IContractProxy
    {
        object UnderlyingObject { get; }

        event EventHandler<ContractProxyObjectChangedEventArgs> UnderlyingObjectChanged;
    }
}
