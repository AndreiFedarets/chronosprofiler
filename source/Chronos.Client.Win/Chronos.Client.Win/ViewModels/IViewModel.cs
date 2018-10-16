using System;
using Chronos.Client.Win.Menu;

namespace Chronos.Client.Win.ViewModels
{
    public interface IViewModel
    {
        Guid TypeId { get; }

        Guid InstanceId { get; }

        string DisplayName { get; }

        IContainerViewModel Parent { get; }

        IMenu ContextMenu { get; }
    }
}
