namespace Chronos.Common.EventsTree
{
    public interface IReferenceAnalyzer
    {
        HeaderReference<uint, IEvent> FindReferences(byte eventType, uint unitId);
    }
}
