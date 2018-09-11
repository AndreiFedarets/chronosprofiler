using System;

namespace Chronos
{
    [Serializable]
    public class ApplicationStateEventArgs : EventArgs
    {
        public ApplicationStateEventArgs(ApplicationState previousState, ApplicationState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }

        public ApplicationState PreviousState { get; private set; }

        public ApplicationState CurrentState { get; private set; }

        public static void RaiseEvent(EventHandler<ApplicationStateEventArgs> handler, object sender,
            ApplicationState previousState, ApplicationState currentState)
        {
            EventExtensions.RaiseEventSafeAndSilent(handler, sender, () => new ApplicationStateEventArgs(previousState, currentState));
        }
    }
}
