namespace Chronos
{
    public interface IApplicationExtensionAdapter
    {
        void BeginInitialize(IChronosApplication application);

        void EndInitialize();

        void BeginShutdown();

        void EndShutdown();
    }
}
