using System;

namespace Adenium
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

        internal static void RaiseEvent(EventHandler<ContractProxyObjectChangedEventArgs> eventHandler, object sender, object oldViewModel, object newViewModel)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, new ContractProxyObjectChangedEventArgs(oldViewModel, newViewModel));
            }
        }
    }
}
