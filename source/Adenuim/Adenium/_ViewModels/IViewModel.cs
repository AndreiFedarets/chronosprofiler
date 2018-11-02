using System;
using Adenium.Layouting;

namespace Adenium
{
    public interface IViewModel : IDisposable
    {
        string ViewModelUid { get; }

        Guid InstanceId { get; }

        string DisplayName { get; }

        IContainerViewModel Parent { get; }

        IContainerViewModel LogicalParent { get; }

        IMenuCollection Menus { get; }
    }
}
