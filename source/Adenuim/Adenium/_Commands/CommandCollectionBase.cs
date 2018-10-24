using System.Collections.Generic;

namespace Chronos.Client.Win.Commands
{
    public class CommandCollectionBase : ICommandCollection
    {
        private readonly IDictionary<string, IExtendedCommand> _commands;

        public CommandCollectionBase()
        {
            _commands = new Dictionary<string, IExtendedCommand>();
        }

        public IExtendedCommand this[string name]
        {
            get
            {
                IExtendedCommand command;
                _commands.TryGetValue(name, out command);
                return command;
            }
        }

        public virtual void Configure()
        {
        }

        public void Register(string id, IExtendedCommand command)
        {
            _commands.Add(id, command);
        }

        public void Register(IExtendedCommand command)
        {
            _commands.Add(command.Id, command);
        }

        public T GetCommand<T>(string id)
        {
            return (T) this[id];
        }

        public IEnumerator<IExtendedCommand> GetEnumerator()
        {
            return _commands.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
