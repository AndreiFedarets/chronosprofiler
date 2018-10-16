using System;

namespace Chronos.Client.Win.ViewModels
{
    public interface IViewModel
    {
        Guid TypeId { get; }

        Guid InstanceId { get; }

        IContainerViewModel Parent { get; }
    }
}
