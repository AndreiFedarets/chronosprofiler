﻿using Chronos.Common.EventsTree;

namespace Chronos.DotNet.TracingProfiler
{
    internal sealed class UnmanagedToManagedTransactionEventMessage : RemoteBaseObject, IUnmanagedToManagedTransactionEventMessage
    {
        public UnmanagedToManagedTransactionEventMessage()
        {
        }

        public string BuildMessage(IEvent @event)
        {
            return BuildMessageInternal(@event);
        }

        internal static string BuildMessageInternal(IEvent @event)
        {
            return "Transaction to managed code";
        }
    }
}
