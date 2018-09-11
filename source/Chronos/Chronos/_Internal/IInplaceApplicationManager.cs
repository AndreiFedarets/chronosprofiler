using System;

namespace Chronos
{
    internal interface IInplaceApplicationManager : IDisposable
    {
        void Run(params object[] args);
    }
}
