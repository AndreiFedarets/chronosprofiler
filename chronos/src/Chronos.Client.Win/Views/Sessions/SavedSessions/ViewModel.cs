 using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Chronos.Host;
using Rhiannon.Threading;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Sessions.SavedSessions
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IProcessShadowRenderer _processShadowRenderer;
		private readonly ISessionCollection _sessions;
		private readonly ITaskFactory _taskFactory;

		public ViewModel(IClientApplication application, ITaskFactory taskFactory, IProcessShadowRenderer processShadowRenderer)
		{
			_sessions = application.Host.Sessions;
			_taskFactory = taskFactory;
			_processShadowRenderer = processShadowRenderer;
		}

		public ICommand OpenSessionCommand { get; private set; }

		public MultithreadingObservableCollection<ISession> Sessions { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			IEnumerable<ISession> sessions = _sessions.Where(x => x.IsSaved);
			Sessions = new MultithreadingObservableCollection<ISession>(sessions);
			_sessions.SessionRemoved += OnSessionRemoved;
			_sessions.SessionStateChanged += OnSessionStateChanged;
			OpenSessionCommand = new AsyncCommand<ISession>(OpenSession, CanOpenSession, _taskFactory);
		}

		private void OnSessionRemoved(ISession session)
		{
			if (Sessions.Contains(session))
			{
				Sessions.Remove(session);
			}
		}

		private void OnSessionStateChanged(ISession session)
		{
			if (session.IsSaved && !Sessions.Contains(session))
			{
				Sessions.Add(session);
			}
		}

		private void OpenSession(ISession session)
		{
			_taskFactory.ThreadFactory.Invoke(() => _processShadowRenderer.Render(session));
		}

		private bool CanOpenSession(ISession session)
		{
			return session != null;
		}

		public override void Dispose()
		{
			base.Dispose();
			_sessions.SessionRemoved -= OnSessionRemoved;
			_sessions.SessionStateChanged -= OnSessionStateChanged;
		}
	}
}
