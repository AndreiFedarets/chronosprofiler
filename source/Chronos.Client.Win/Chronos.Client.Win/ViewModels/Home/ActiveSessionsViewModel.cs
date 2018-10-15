using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Chronos.Client.Win.Commands;

namespace Chronos.Client.Win.ViewModels.Home
{
    public class ActiveSessionsViewModel : ViewModel
    {
        private readonly ObservableCollection<SessionInformation> _collection;
        private readonly ISessionCollection _sessions;

        public ActiveSessionsViewModel(IMainApplication application)
        {
            _sessions = application.Sessions;
            _collection = new ObservableCollection<SessionInformation>();
            OpenSessionCommand = new SyncCommand<SessionInformation>(OpenSession);
            lock (_collection)
            {
                _sessions.SessionCreated += OnSessionCreated;
                _sessions.SessionRemoved += OnSessionRemoved;
                _sessions.SessionStateChanged += OnSessionStateChanged;
                foreach (ISession session in _sessions)
                {
                    if (session.IsActive)
                    {
                        SessionInformation sessionInformation = new SessionInformation(session);
                        _collection.Add(sessionInformation);
                    }
                }
            }
        }

        public override string DisplayName
        {
            get { return "Active Sessions"; }
        }

        public ICommand OpenSessionCommand { get; private set; }

        public IEnumerable<SessionInformation> ActiveSessions
        {
            get { return _collection; }
        }

        public override void Dispose()
        {
            base.Dispose();
            _sessions.SessionCreated -= OnSessionCreated;
            _sessions.SessionRemoved -= OnSessionRemoved;
            _sessions.SessionStateChanged -= OnSessionStateChanged;
        }

        private void OpenSession(SessionInformation sessionInformation)
        {
            //TODO: hide (move) this logic somewhere in MainApplication or SessionCollection
            ApplicationManager.Profiling.RunOrActivateApplication(sessionInformation.Session.Uid);
        }

        private void OnSessionCreated(object sender, SessionEventArgs eventArgs)
        {
            lock (_collection)
            {
                SessionInformation sessionInformation = _collection.FirstOrDefault(x => Equals(x.Session, eventArgs.Session));
                if (sessionInformation == null && eventArgs.Session.IsActive)
                {
                    sessionInformation = new SessionInformation(eventArgs.Session);
                    DispatcherHolder.Invoke(() => _collection.Add(sessionInformation));
                }
            }
        }

        private void OnSessionRemoved(object sender, SessionEventArgs eventArgs)
        {
            lock (_collection)
            {
                SessionInformation sessionInformation = _collection.FirstOrDefault(x => Equals(x.Session, eventArgs.Session));
                DispatcherHolder.Invoke(() => _collection.Remove(sessionInformation));
            }
        }

        private void OnSessionStateChanged(object sender, SessionEventArgs eventArgs)
        {
            SessionInformation sessionInformation;
            lock (_collection)
            {
                sessionInformation = _collection.FirstOrDefault(x => Equals(x.Session, eventArgs.Session));
                bool isActive = eventArgs.Session.IsActive;
                if (sessionInformation == null && isActive)
                {
                    sessionInformation = new SessionInformation(eventArgs.Session);
                    DispatcherHolder.Invoke(() => _collection.Add(sessionInformation));
                }
                else if (sessionInformation != null && !isActive)
                {
                    DispatcherHolder.Invoke(() => _collection.Remove(sessionInformation));
                }
                else if (sessionInformation != null)
                {
                    DispatcherHolder.Invoke(() => sessionInformation.NotifyChanged());
                }
            }
        }
    }
}
