using System.Windows.Input;
using System;

namespace Chronos.Client.Win.Commands
{
    public interface IExtendedCommand : ICommand, IDisposable
    {
        string Id { get; }
    }
}
