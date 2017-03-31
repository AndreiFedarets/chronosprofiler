namespace Chronos.Client.Win.Common.EventsTree
{
    public interface IReferencesAnalyzer
    {
        HeaderReference<uint, IEvent> FindReferences(byte eventType, uint unitId);
    }
}
