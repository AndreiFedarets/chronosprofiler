namespace Chronos.Common.EventsTree
{
    internal sealed class ReferencesAnalyzer : IReferenceAnalyzer
    {
        private readonly IEventTreeCollection _eventTrees;

        public ReferencesAnalyzer(IEventTreeCollection eventTree)
        {
            _eventTrees = eventTree;
        }

        public HeaderReference<uint, IEvent> FindReferences(byte eventType, uint unitId)
        {
            HeaderReference<uint, IEvent> references = new HeaderReference<uint, IEvent>(eventType);
            foreach (IEventTree tree in _eventTrees)
            {
                Reference<uint, IEvent> reference = references[tree.ThreadUid];
                FindReferencesRecurcive(tree, reference, eventType, unitId);
            }
            return references;
        }

        private void FindReferencesRecurcive(IEvent current, Reference<uint, IEvent> reference, byte eventType, uint unitId)
        {
            if (current.EventType == eventType)
            {
                if (current.Unit == unitId)
                {
                    reference.Add(current);
                }
            }
            foreach (IEvent @event in current.Children)
            {
                FindReferencesRecurcive(@event, reference, eventType, unitId);
            }
        }
    }
}
