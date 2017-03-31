namespace Chronos.Common.EventsTree
{
    public interface IThreadEventTree : IEventTree
    {
        uint ThreadUid { get; }

        uint ThreadOsId { get; }
    }
}
