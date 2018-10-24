using System;
using Adenium;

namespace Chronos.Client.Win
{
    internal interface IApplicationStrategy : IDisposable
    {
        Guid Uid { get; }

        void Initialize(IApplicationBase application, IContainer container);

        void Run();
    }
}
