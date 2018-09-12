using System;

namespace Chronos.Client.Win.ViewModels
{
    public interface IViewModelActivator
    {
        Guid ViewModelTypeId { get; }

        ViewModel CreateInstance();
    }
}
