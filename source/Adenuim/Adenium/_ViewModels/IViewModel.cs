using System;
using Adenium.Layouting;

namespace Adenium
{
    public interface IViewModel : IDisposable
    {
        Guid TypeId { get; }

        Guid InstanceId { get; }

        string DisplayName { get; }

        IContainerViewModel Parent { get; }

        IMenuCollection Menus { get; }
    }
}
