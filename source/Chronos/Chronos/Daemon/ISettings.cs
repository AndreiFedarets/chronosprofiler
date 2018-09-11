namespace Chronos.Daemon
{
    /// <summary>
    /// Represents Daemon application settings provider
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Get running type of Daemon application
        /// </summary>
        /// <seealso cref="Runtype"/>
        string Runtype { get; }
    }
}
