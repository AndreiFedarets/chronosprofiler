namespace Chronos
{
    public sealed class EmptyApplicationExtensionAdapter : IApplicationExtensionAdapter
    {
        public void BeginInitialize(IChronosApplication application)
        {
        }

        public void EndInitialize()
        {
        }

        public void BeginShutdown()
        {
        }

        public void EndShutdown()
        {
        }
    }
}
