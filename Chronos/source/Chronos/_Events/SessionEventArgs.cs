using System;

namespace Chronos
{
    [Serializable]
    public sealed class SessionEventArgs : EventArgs
    {
        public SessionEventArgs(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }

        public static void RaiseEvent(EventHandler<SessionEventArgs> handler, object sender, ISession session)
        {
            EventExtensions.RaiseEventSafeAndSilent(handler, sender, () => new SessionEventArgs(session));
        }
    }
}
