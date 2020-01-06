using Layex;
using System;

namespace Chronos.Client.Win
{
    internal interface IApplicationStrategy : IDisposable
    {
        Guid Uid { get; }

        void Initialize(IApplicationBase application, IDependencyContainer container);

        void Run();
    }
}
