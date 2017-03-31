using System;

namespace Chronos.Core
{
    [Serializable]
    public sealed class ExceptionInfo : UnitBase
    {
        public ExceptionInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName, ulong threadId, bool isCatched)
            : base(id, managedId, beginLifetime, endLifetime, fullName)
        {
            ThreadId = threadId;
            IsCatched = isCatched;
        }

        public ExceptionInfo()
        {
            ThreadId = 0;
            IsCatched = false;
        }

        public ulong ThreadId { get; set; }

        public bool IsCatched { get; set; }

        public string Message { get; set; }

        public uint[] Stack { get; set; }
    }
}
