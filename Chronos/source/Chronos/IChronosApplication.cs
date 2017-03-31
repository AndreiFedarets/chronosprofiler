using System;

namespace Chronos
{
    public interface IChronosApplication
    {
        /// <summary>
        /// Information about environment where Application is running
        /// </summary>
        EnvironmentInformation EnvironmentInformation { get; }

        /// <summary>
        /// Unique identifier of application
        /// </summary>
        Guid Uid { get; }

        /// <summary>
        /// Get current state of application
        /// </summary>
        ApplicationState ApplicationState { get; }

        /// <summary>
        /// Get Host application startup time
        /// </summary>
        TimeSpan StartupTime { get; }

        /// <summary>
        /// Get container for shared services
        /// </summary>
        IServiceContainer ServiceContainer { get; }

        event EventHandler<ApplicationStateEventArgs> ApplicationStateChanged;

        void Run();

        void Close();

        /// <summary>
        /// Send test message.
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Message which was sent</returns>
        string Ping(string message);
    }
}
