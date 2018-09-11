using System;
using Microsoft.Practices.Unity;

namespace Chronos.Client.Win
{
    internal interface IApplicationStrategy : IDisposable
    {
        Guid Uid { get; }

        void Initialize(IApplicationBase application, IContainer container);

        void Run();
    }
}
