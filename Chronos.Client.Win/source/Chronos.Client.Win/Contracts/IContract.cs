﻿namespace Chronos.Client.Win.Contracts
{
    public interface IContract
    {
        void Register(object item);

        void Unregister(object item);
    }


    public interface IContract<TSource, TConsumer>
    {
        void Register(object item);

        void Unregister(object item);
    }
}
