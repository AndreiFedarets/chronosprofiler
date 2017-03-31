using System;

namespace Chronos.Daemon.Internal
{
    internal class Task
    {
        private readonly Action _action;

        public Task(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action();
        }
    }
}
