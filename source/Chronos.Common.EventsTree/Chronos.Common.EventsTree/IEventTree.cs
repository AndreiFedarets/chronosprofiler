namespace Chronos.Common.EventsTree
{
    public interface IEventTree : IEvent
    {
        uint BeginLifetime { get; }

        uint EndLifetime { get; }

        byte[] GetBinaryData();
    }
}
