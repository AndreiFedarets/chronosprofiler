using System;
using System.Collections.Generic;
using System.Reflection;
using Adenium;
using Chronos.Messaging;

namespace Chronos.Client.Win
{
    internal sealed class ClientMessageBus : IMessageBus, IDisposable
    {
        private readonly Dictionary<IMessageBusHandler, MessageBusSubscription> _subscriptions;

        static ClientMessageBus()
        {
            Current = new ClientMessageBus();
        }

        private ClientMessageBus()
        {
            _subscriptions = new Dictionary<IMessageBusHandler, MessageBusSubscription>();
        }

        public static IMessageBus Current { get; private set; }

        public void SendMessage(object sender, uint message, object parameter)
        {
            SmartDispatcher.Main.Invoke(() => SendMessageInternal(sender, message, parameter));
        }

        public void PostMessage(object sender, uint message, object parameter)
        {
            SmartDispatcher.Main.BeginInvoke(() => SendMessageInternal(sender, message, parameter));
        }

        public IDisposable Subscribe(IMessageBusHandler handler)
        {
            if (handler == null)
            {
                throw new TempException();
            }
            MessageBusSubscription subscription = new MessageBusSubscription(this, handler);
            lock (_subscriptions)
            {
                if (!_subscriptions.ContainsKey(handler))
                {
                    _subscriptions.Add(handler, subscription);   
                }
            }
            return subscription;
        }

        public void Dispose()
        {
            lock (_subscriptions)
            {
                _subscriptions.Clear();
            }
        }

        private void SendMessageInternal(object sender, uint message, object parameter)
        {
            lock (_subscriptions)
            {
                foreach (MessageBusSubscription subscription in _subscriptions.Values)
                {
                    subscription.Invoke(sender, message, parameter);
                }   
            }
        }

        private void Unsubscribe(IMessageBusHandler handler)
        {
            lock (_subscriptions)
            {
                _subscriptions.Remove(handler);
            }
        }

        private sealed class MessageBusSubscription : IDisposable
        {
            private readonly Dictionary<uint, MethodInfo> _handlers;
            private readonly IMessageBusHandler _handler;
            private readonly ClientMessageBus _messageBus;

            public MessageBusSubscription(ClientMessageBus messageBus, IMessageBusHandler handler)
            {
                _messageBus = messageBus;
                _handler = handler;
                _handlers = GetHandlers(handler);
            }

            public void Invoke(object sender, uint message, object parameter)
            {
                MethodInfo targetMethodInfo;
                if (_handlers.TryGetValue(message, out targetMethodInfo))
                {
                    targetMethodInfo.Invoke(_handler, new[] {sender, parameter});
                }
            }

            public void Dispose()
            {
                _messageBus.Unsubscribe(_handler);
            }

            private static Dictionary<uint, MethodInfo> GetHandlers(IMessageBusHandler handler)
            {
                Dictionary<uint, MethodInfo> handlers = new Dictionary<uint, MethodInfo>();
                Type handlerType = handler.GetType();
                foreach (MethodInfo methodInfo in handlerType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    MessageHandlerAttribute attribute = methodInfo.GetCustomAttribute<MessageHandlerAttribute>();
                    if (attribute != null)
                    {
                        handlers[attribute.Message] = methodInfo;
                    }
                }
                return handlers;
            }
        }
    }
}
