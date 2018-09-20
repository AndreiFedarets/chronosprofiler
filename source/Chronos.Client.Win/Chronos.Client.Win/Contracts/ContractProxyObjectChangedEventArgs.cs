using System;

namespace Chronos.Client.Win.Contracts
{
    public class ContractProxyObjectChangedEventArgs : EventArgs
    {
        public ContractProxyObjectChangedEventArgs(object oldObject, object newObject)
        {
            OldObject = oldObject;
            NewObject = newObject;
        }

        public object OldObject { get; private set; }

        public object NewObject { get; private set; }
    }
}
