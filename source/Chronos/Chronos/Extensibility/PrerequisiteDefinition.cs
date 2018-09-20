namespace Chronos.Extensibility
{
    public sealed class PrerequisiteDefinition : ExportDefinition
    {
        internal PrerequisiteDefinition(string baseDirectory, string entryPoint)
            : base(string.Empty, baseDirectory, entryPoint, string.Empty, string.Empty, LoadBehavior.OnStartup)
        {
        }
    }
}
