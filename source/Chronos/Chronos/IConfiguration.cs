using System;

namespace Chronos
{
    public interface IConfiguration
    {
        /// <summary>
        /// Configuration unique identifier
        /// </summary>
        /// <seealso cref="Chronos.ConfigurationSettings.ConfigurationUid"/>
        Guid Uid { get; }

        /// <summary>
        /// Get display name of configuration
        /// This name is provided by user when configuration creating.
        /// </summary>
        /// <seealso cref="Chronos.ConfigurationSettings.ConfigurationName"/>
        string Name { get; }

        /// <summary>
        /// Get state of configuration.
        /// 'True' if configuration is active, otherwise 'False'
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Get Host Application where this Configuration was created
        /// </summary>
        Host.IApplication Application { get; }

        /// <summary>
        /// Get configuration settings.
        /// This property returns copy of configuration settings to leave them immutable.
        /// </summary>
        /// <seealso cref="ConfigurationSettings"/>
        ConfigurationSettings ConfigurationSettings { get; }

        /// <summary>
        /// Activate configuration.
        /// Profiling will be started.
        /// </summary>
        void Activate();

        /// <summary>
        /// Deactivate configuration. Profiling will be stopped.
        /// All related sessions will be closed.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Remove configuration from the list of recent configurations.
        /// If configuration is active it will be deactivated.
        /// All sessions created from this configuration will also be removed.
        /// </summary>
        void Remove();
    }
}
