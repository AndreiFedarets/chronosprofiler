namespace Chronos
{
    /// <summary>
    /// Represents state of session
    /// </summary>
    public enum SessionState : byte
    {
        /// <summary>
        /// Session is not started yet or already closed
        /// </summary>
        Closed = 0,
        /// <summary>
        /// Session is active and profiled process is launched
        /// </summary>
        Profiling = 1,
        /// <summary>
        /// Session is active but profiled process already closed
        /// </summary>
        Decoding = 2,
    }
}
