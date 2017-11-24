namespace Chronos
{
    public interface IProfilingTimer
    {

        /// <summary>
        /// Get current profiling time in ms
        /// </summary>
        uint CurrentTime { get; }

        uint BeginProfilingTime { get; }
    }
}
