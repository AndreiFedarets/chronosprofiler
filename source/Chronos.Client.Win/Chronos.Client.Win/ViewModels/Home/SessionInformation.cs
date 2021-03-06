﻿using Adenium;

namespace Chronos.Client.Win.ViewModels.Home
{
    public sealed class SessionInformation : PropertyChangedBaseEx
    {
        public SessionInformation(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }

        public SessionState SessionState
        {
            get { return Session.State; }
        }

        public ProcessInformation Process
        {
            get { return Session.GetProfiledProcessInformation(); }
        }

        public void NotifyChanged()
        {
            NotifyOfPropertyChange(() => SessionState);
            NotifyOfPropertyChange(() => Process);
        }
    }
}
