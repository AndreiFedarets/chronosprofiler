using System;
using System.Collections.Generic;

namespace Chronos
{
    /// <summary>
    /// Represents collection of all available configurations.
    /// </summary>
    public interface ISessionCollection : IEnumerable<ISession>
    {
        /// <summary>
        /// Get session by unique identifier.
        /// </summary>
        /// <param name="uid">Session unique identifier</param>
        /// <returns>Session</returns>
        /// <exception cref="SessionNotFoundException">Session with provided token was not found.</exception>
        ISession this[Guid uid] { get; }

        /// <summary>
        /// Get count of Sessions
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Occurs when session was created.
        /// </summary>
        event EventHandler<SessionEventArgs> SessionCreated;

        /// <summary>
        /// Occurs when session was removed.
        /// </summary>
        event EventHandler<SessionEventArgs> SessionRemoved;

        /// <summary>
        /// Occurs when state of session was changed.
        /// </summary>
        event EventHandler<SessionEventArgs> SessionStateChanged;

        /// <summary>
        /// Check that sessions with provided unique identifier exists.
        /// </summary>
        /// <param name="uid">Session unique identifier</param>
        /// <returns>'True' if session is presented in the collection, otherwise 'False'</returns>
        bool Contains(Guid uid);
    }
}
