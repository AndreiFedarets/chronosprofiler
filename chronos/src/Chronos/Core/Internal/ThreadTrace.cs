using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core.Internal
{
    internal class ThreadTrace : IThreadTrace
    {
        private readonly IEventsTreeBuilder _eventsTreeBuilder;
        private readonly ICallstackCollection _callstacks;
        private readonly ICallstackLoader _callstackLoader;
        private List<IEvent> _events;
        private byte[] _data;

        public ThreadTrace(ThreadInfo threadInfo, ICallstackCollection callstacks, ICallstackLoader callstackLoader, IEventsTreeBuilder eventsTreeBuilder)
        {
            ThreadInfo = threadInfo;
            _callstacks = callstacks;
            _callstackLoader = callstackLoader;
            _eventsTreeBuilder = eventsTreeBuilder;
        }

        public ThreadInfo ThreadInfo { get; private set; }

        public IEvent Parent
        {
            get { return null; }
        }

        public uint Hits
        {
            get { return 1; }
        }

        public uint Time { get; private set; }

        public long StackFullTime { get; internal set; }

        public EventType EventType
        {
            get { return EventType.ThreadTrace; }
        }

        public bool HasChildren
        {
            get { return _data.Length > 0; }
        }

        public uint Unit
        {
            get { return ThreadInfo.Id; }
        }

        public double Percent
        {
            get
            {
                if (StackFullTime == 0)
                {
                    return 0;
                }
                return Math.Round((((double)Time) / ((double)StackFullTime)) * 100, 1);
            }
        }

        public IEnumerable<IEvent> Children
        {
            get { return _events ?? (_events = Load()); }
        }

        public void Reload()
        {
            CallstackInfo[] callstacks = _callstacks.ThreadCallstacks(ThreadInfo.Id);
            _data = _callstackLoader.LoadCallstacks(callstacks);
            Time = (uint)callstacks.Sum(x => x.EndLifetime - x.BeginLifetime);
            StackFullTime = 0;
        }

        private List<IEvent> Load()
        {
            if (_data == null)
            {
                Reload();
            }
            List<IEvent> events = _eventsTreeBuilder.BuildChildren(this, _data, Time, ThreadInfo);
            return events;
        }
    }
}
