using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Chronos.Core
{
    public class CallstackCollection : UnitCollection<CallstackInfo>, ICallstackCollection
    {
        private static int _index;

        public override uint UnitType
        {
            get { return (uint) Core.UnitType.Callstack; }
        }

        public uint TotalTime
        {
            get { return (uint) this.Sum(x => x.EndLifetime - x.BeginLifetime); }
        }

        public CallstackInfo[] ThreadCallstacks(uint threadId)
        {
            List<CallstackInfo> callstacks = Snapshot();
            return callstacks.Where(x => x.ThreadId == threadId).ToArray();
        }

        internal static uint GenerateCallstackId()
        {
            uint id = (uint)_index;
            Interlocked.Increment(ref _index);
            return id;
        }

    }
}
