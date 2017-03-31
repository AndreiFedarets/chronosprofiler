using System;
using System.Collections.Generic;

namespace Chronos.Daemon.Internal
{
    internal class ThreadStreamPool : IDisposable
    {
        private readonly List<ThreadStream> _streams;
        private readonly IThreadStreamProcessor _processor;
        private readonly Guid _daemonToken;
        private readonly uint _callPageSize;

        public ThreadStreamPool(Guid daemonToken, IThreadStreamProcessor processor, uint callPageSize, uint capacity)
        {
            _streams = new List<ThreadStream>();
            _daemonToken = daemonToken;
            _processor = processor;
            _callPageSize = callPageSize;
            for (int i = 0; i < capacity; i++)
            {
                CreateStream();
            }
        }

        private void CreateStream()
        {
            lock (_streams)
            {
                ThreadStream stream = new ThreadStream((uint)_streams.Count, _daemonToken, _processor, _callPageSize);
                stream.Connected += OnCurrentStreamConnected;
                _streams.Add(stream);
            }
        }

        private void OnCurrentStreamConnected(object sender, EventArgs eventArgs)
        {
            ThreadStream stream = (ThreadStream) sender;
            stream.Connected -= OnCurrentStreamConnected;
            CreateStream();
        }

        public void Dispose()
        {
            lock (_streams)
            {
                foreach (ThreadStream stream in _streams)
                {
                    stream.Dispose();
                }
            }
        }
    }
}
