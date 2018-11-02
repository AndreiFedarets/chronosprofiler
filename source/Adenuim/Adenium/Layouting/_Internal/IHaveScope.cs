namespace Adenium.Layouting
{
    internal interface IHaveScope
    {
        IContainer ScopeContainer { get; }

        void AssignScopeContainer(IContainer container);
    }
}
