using System;

namespace Chronos
{
    [Serializable]
    public class SessionStateEventArgs : EventArgs
    {
        public SessionStateEventArgs(SessionState previousState, SessionState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }

        public SessionState PreviousState { get; private set; }

        public SessionState CurrentState { get; private set; }

        public static void RaiseEvent(EventHandler<SessionStateEventArgs> handler, object sender, 
                                      SessionState previousState, SessionState currentState)
        {
            EventExtensions.RaiseEventSafeAndSilent(handler, sender, () => new SessionStateEventArgs(previousState, currentState));
        }
    }
}
