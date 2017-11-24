namespace Chronos.Client.Win
{
    public interface IProfilingApplication : IApplicationBase
    {
        IProfilingTimer ProfilingTimer { get; }

        void FlushData();
    }
}
