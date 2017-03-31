using System.Collections.Generic;
using System.Linq;

namespace Chronos.Common.EventsTree
{
    public sealed class EventTreeMerger : IEventTreeMerger
    {
        private readonly AgentLibrary _agentLibrary;

        public EventTreeMerger()
        {
            _agentLibrary = new AgentLibrary();
        }

        public IEventTree[] Merge(IEnumerable<ISingleEventTree> source, EventTreeMergeType mergeType)
        {
            IEventTree[] result = null;
            switch (mergeType)
            {
                case EventTreeMergeType.None:
                    result = source.ToArray();
                    break;
                case EventTreeMergeType.Root:
                    result = MergeByRoots(source);
                    break;
                case EventTreeMergeType.Thread:
                    result = MergeByThreads(source);
                    break;
                default:
                    throw new TempException("Unknown MergeType value");
            }
            return result;
        }

        public void Dispose()
        {
            _agentLibrary.Dispose();
        }

        private IEventTree[] MergeByThreads(IEnumerable<ISingleEventTree> source)
        {
            IGrouping<uint, ISingleEventTree>[] groups = source.GroupBy(x => x.ThreadUid).ToArray();
            IEventTree[] result = new IEventTree[groups.Length];
            for (int i = 0; i < groups.Length; i++)
            {
                ISingleEventTree[] items = groups[i].ToArray();
                uint beginLifetime = items.Min(x => x.BeginLifetime);
                uint endLifetime = items.Max(x => x.EndLifetime);
                uint threadOsId = items[0].ThreadOsId;
                uint threadUid = items[0].ThreadUid;
                List<byte[]> data = items.Select(x => x.GetBinaryData()).ToList();
                uint hits = 1;
                uint time = (uint)data.Sum(x => NativeEventHelper.GetTime(x));
                byte[] mergedData = _agentLibrary.MergeEventTrees(data, NativeEventHelper.CreateEvent(ThreadEventTreeMessage.EventType, 0, threadUid, time, hits));
                ThreadEventTree threadEventTree = new ThreadEventTree(threadUid, threadOsId, beginLifetime, endLifetime, mergedData);
                result[i] = threadEventTree;
            }
            return result;
        }

        private IEventTree[] MergeByRoots(IEnumerable<ISingleEventTree> source)
        {
            IGrouping<ulong, ISingleEventTree>[] groups = source.GroupBy(x => x.EventHash).ToArray();
            IEventTree[] result = new IEventTree[groups.Length];
            for (int i = 0; i < groups.Length; i++)
            {
                ISingleEventTree[] items = groups[i].ToArray();
                uint beginLifetime = items.Min(x => x.BeginLifetime);
                uint endLifetime = items.Max(x => x.EndLifetime);
                List<byte[]> data = items.Select(x => x.GetBinaryData()).ToList();
                byte[] mergedData = _agentLibrary.MergeEventTrees(data);
                MergedEventTree mergedEventTree = new MergedEventTree(beginLifetime, endLifetime, mergedData);
                result[i] = mergedEventTree;
            }
            return result;
        }
    }
}
