using System.Windows.Input;
using System;

namespace Adenium
{
    public interface IExtendedCommand : ICommand, IDisposable
    {
        string Id { get; }
    }
}
