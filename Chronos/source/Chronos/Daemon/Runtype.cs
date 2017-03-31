namespace Chronos.Daemon
{
    /// <summary>
    /// Provides possible physical placement for Daemon
    /// </summary>
    public static class Runtype
    {
        /// <summary>
        /// In current process.
        /// </summary>
        public const string Inplace = "inplace";

        /// <summary>
        /// In standalone hidden process.
        /// </summary>
        public const string Application = "application";
    }
}
