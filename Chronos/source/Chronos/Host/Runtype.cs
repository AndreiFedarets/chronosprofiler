namespace Chronos.Host
{
    /// <summary>
    /// Provides possible physical placement for Host Server
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

        /// <summary>
        /// In Windows Service
        /// </summary>
        public const string Service = "service";
    }
}
