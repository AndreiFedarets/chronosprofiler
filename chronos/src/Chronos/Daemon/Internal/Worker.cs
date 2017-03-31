using System.Collections.Generic;
using System.Threading;
using Rhiannon.Extensions;

namespace Chronos.Daemon.Internal
{
    internal class Worker : IWorker
    {
        private readonly ConcurrentQueue<Task> _tasks;
        private readonly List<Thread> _threads;
        private bool _working;

        public Worker()
        {
            _tasks = new ConcurrentQueue<Task>();
            _threads = InitializeThreads(GetThreadsCount());
            Start();
        }

        private static uint GetThreadsCount()
        {
            return 1;
        }

        private List<Thread> InitializeThreads(uint threadsCount)
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < threadsCount; i++)
            {
                Thread thread = new Thread(DoWork);
                threads.Add(thread);
            }
            return threads;
        }

        public void Start()
        {
            _working = true;
            foreach (Thread thread in _threads)
            {
                thread.Start();
            }
        }

        public void Enqueue(Task task)
        {
            _tasks.Enqueue(task);
        }

        private void DoWork()
        {
            while (true)
            {
                Task task = _tasks.DequeueOrDefault();
                if (task == null)
                {
                    if (_working)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    task.Execute();
                }
            }
        }
    }
}
