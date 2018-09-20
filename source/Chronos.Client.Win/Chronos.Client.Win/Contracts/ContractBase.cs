using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.Contracts
{
    public abstract class ContractBase<TSource, TConsumer> : IContract<TSource, TConsumer>, IContract
        where TSource : IContractSource
        where TConsumer : IContractConsumer
    {
        protected readonly List<TSource> Sources;
        protected readonly List<TConsumer> Consumers;

        protected ContractBase()
        {
            Sources = new List<TSource>();
            Consumers = new List<TConsumer>();
        }

        public void Register(object item)
        {
            if (item is IContractProxy)
            {
                IContractProxy proxy = (IContractProxy) item;
                object underlyingObject = proxy.UnderlyingObject;
                Register(underlyingObject);   
                proxy.UnderlyingObjectChanged += OnProxyUnderlyingObjectChanged;
            }
            if (item is TSource)
            {
                RegisterSource((TSource)item);
            }
            if (item is TConsumer)
            {
                RegisterConsumer((TConsumer)item);
            }
        }

        private void OnProxyUnderlyingObjectChanged(object sender, ContractProxyObjectChangedEventArgs e)
        {
            object oldObject = e.OldObject;
            if (oldObject != null)
            {
                Unregister(oldObject);
            }
            object newObject = e.NewObject;
            if (newObject != null)
            {
                Register(newObject);
            }
        }

        public void Unregister(object item)
        {
            if (item is IContractProxy)
            {
                IContractProxy proxy = (IContractProxy)item;
                proxy.UnderlyingObjectChanged -= OnProxyUnderlyingObjectChanged;
                object underlyingObject = proxy.UnderlyingObject; 
                Unregister(underlyingObject);   
            }
            if (item is TSource)
            {
                UnregisterSource((TSource)item);
            }
            if (item is TConsumer)
            {
                UnregisterConsumer((TConsumer)item);
            }
        }

        private void RegisterSource(TSource source)
        {
            if (Sources.Contains(source))
            {
                return;
            }
            Sources.Add(source);
            source.ContractSourceChanged += OnContractSourceChanged;
        }

        private void RegisterConsumer(TConsumer consumer)
        {
            if (Consumers.Contains(consumer))
            {
                return;
            }
            Consumers.Add(consumer);
        }

        private void UnregisterSource(TSource source)
        {
            if (!Sources.Contains(source))
            {
                return;
            }
            source.ContractSourceChanged -= OnContractSourceChanged;
            Sources.Remove(source);
        }

        private void UnregisterConsumer(TConsumer consumer)
        {
            if (!Consumers.Contains(consumer))
            {
                return;
            }
            Consumers.Remove(consumer);
        }

        private void OnContractSourceChanged(object sender, EventArgs e)
        {
            OnContractSourceChanged();
        }

        protected abstract void OnContractSourceChanged();
    }
}
