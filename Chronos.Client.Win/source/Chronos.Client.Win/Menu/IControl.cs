using System.ComponentModel;

namespace Chronos.Client.Win.Menu
{
    public interface IControl : INotifyPropertyChanged
    {
        string Id { get; }

        bool? IsEnabled { get; }

        bool? IsVisible { get; }
    }
}
