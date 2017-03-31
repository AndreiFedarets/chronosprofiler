using System;
using System.Collections.Generic;
using System.Linq;
using Rhiannon.Extensions;

namespace Chronos.Core.Internal
{
    internal class ThreadTraceCollection : IThreadTraceCollection
    {
        private readonly IEventsTreeBuilder _eventsTreeBuilder;
        private readonly IThreadCollection _threads;
        private readonly ICallstackLoader _callstackLoader;
        private readonly ICallstackCollection _callstacks;
        private Dictionary<uint, IThreadTrace> _traces;

        public ThreadTraceCollection(IThreadCollection threads, ICallstackCollection callstacks, ICallstackLoader callstackLoader)
        {
            _threads = threads;
            _callstacks = callstacks;
            _callstackLoader = callstackLoader;
            _eventsTreeBuilder = new EventsTreeBuilder();
            _traces = new Dictionary<uint, IThreadTrace>();
        }

        public IThreadTrace this[ThreadInfo threadInfo]
        {
            get { return _traces[threadInfo.Id]; }
        }

        public long TotalTime
        {
            get { return _traces.Values.Sum(x => x.Time); }
        }

        public event Action Reloaded;

        public void Reload()
        {
            List<ThreadInfo> threads = _threads.Snapshot();
            if (_traces.Count != threads.Count)
            {
                foreach (ThreadInfo threadInfo in threads)
                {
                    if (!_traces.ContainsKey(threadInfo.Id))
                    {
                        ThreadTrace threadTrace = new ThreadTrace(threadInfo, _callstacks, _callstackLoader, _eventsTreeBuilder);
                        _traces.Add(threadInfo.Id, threadTrace);
                    }
                }
            }
            foreach (IThreadTrace threadTrace in _traces.Values)
            {
                threadTrace.Reload();
            }
            List<IThreadTrace> traces = new List<IThreadTrace>(_traces.Values);
            traces.Sort(CompareThreadTraces);
            _traces = traces.ToDictionary(x => x.ThreadInfo.Id, x => x);
            long totalTime = _traces.Values.Sum(x => x.Time);
            foreach (ThreadTrace threadTrace in _traces.Values)
            {
                threadTrace.StackFullTime = totalTime;
            }
            Reloaded.SafeInvoke();
        }

        private int CompareThreadTraces(IThreadTrace threadTrace1, IThreadTrace threadTrace2)
        {
            return threadTrace1.ThreadInfo.Id < threadTrace2.ThreadInfo.Id ? -1 : 1;
        }

        public IEnumerator<IThreadTrace> GetEnumerator()
        {
            return _traces.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
