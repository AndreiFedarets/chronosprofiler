
namespace Chronos.DotNet.TracingProfiler
{
    public static class Constants
    {
        public static class EventType
        {
            public const byte ManagedFunctionCall = 0x0D;
            public const byte ManagedToUnmanagedTransaction = 0x0E;
            public const byte UnmanagedToManagedTransaction = 0x0F;
            public const byte ThreadCreate = 0x09;
            public const byte ThreadDestroy = 0x0A;
        }
    }
}
