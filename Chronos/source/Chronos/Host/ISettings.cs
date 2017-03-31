namespace Chronos.Host
{
    /// <summary>
    /// Represents Host application settings provider
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Get running type of Host application (how to launch Host Application)
        /// </summary>
        /// <seealso cref="Runtype"/>
        string Runtype { get; }
    }
}
