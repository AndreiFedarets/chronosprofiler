using System;

namespace Chronos.Common
{
    public static class Constants
    {
        public static readonly Guid FrameworkUid;

        static Constants()
        {
            FrameworkUid = new Guid("DEF6FD49-CD6D-484F-A2F3-4406CE9178F9");
        }
    }
}
