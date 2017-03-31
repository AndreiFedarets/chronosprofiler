using System;
using System.Collections.Generic;

namespace Chronos
{
    /// <summary>
    /// Represents collection of all available configurations.
    /// </summary>
    public interface IConfigurationCollection : IEnumerable<IConfiguration>
    {
        /// <summary>
        /// Get Configuration by unique identifier
        /// </summary>
        /// <param name="uid">Configuration unique identifier</param>
        /// <returns>Configuration</returns>
        /// <exception cref="ConfigurationNotFoundException">Configuration with provided token was not found.</exception>
        IConfiguration this[Guid uid] { get; }

        /// <summary>
        /// Get count of Configurations
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Occurs when configuration was created.
        /// </summary>
        event EventHandler<ConfigurationEventArgs> ConfigurationCreated;

        /// <summary>
        /// Occurs when configuration was removed.
        /// </summary>
        event EventHandler<ConfigurationEventArgs> ConfigurationRemoved;

        /// <summary>
        /// Create new configuration and add it to the collection.
        /// </summary>
        /// <param name="configurationSettings">Configuration settings</param>
        /// <returns>Created configuration</returns>
        IConfiguration Create(ConfigurationSettings configurationSettings);

        /// <summary>
        /// Check that configuration with provided unique identifier exists
        /// </summary>
        /// <param name="uid">Configuration unique identifier</param>
        /// <returns>'True' if configuration is presented in the collection, otherwise 'False'</returns>
        bool Contains(Guid uid);
    }
}
