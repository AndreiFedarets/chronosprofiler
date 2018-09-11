using System.Collections.Generic;

namespace Chronos.Client.Win.Commands
{
    public interface ICommandCollection : IEnumerable<IExtendedCommand>
    {
        IExtendedCommand this[string name] { get; }

        void Register(string name, IExtendedCommand command);

        void Register(IExtendedCommand command);

        void Configure();
    }
}
