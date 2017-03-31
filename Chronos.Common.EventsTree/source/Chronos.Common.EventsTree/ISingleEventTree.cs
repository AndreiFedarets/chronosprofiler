namespace Chronos.Common.EventsTree
{
    public interface ISingleEventTree : IEventTree
    {
        uint ThreadUid { get; }

        ulong EventTreeUid { get; }

        uint ThreadOsId { get; }
    }
}
