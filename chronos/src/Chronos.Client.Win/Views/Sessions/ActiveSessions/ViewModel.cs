using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Chronos.Host;
using Rhiannon.Threading;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Sessions.ActiveSessions
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

		public MultithreadingObservableCollection<ISession> Sessions { get; private set; }

		public ICommand OpenSessionCommand { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			OpenSessionCommand = new AsyncCommand<ISession>(OpenSession, CanOpenSession, _taskFactory);
			IEnumerable<ISession> sessions = _sessions.Where(x => x.IsActive);
			Sessions = new MultithreadingObservableCollection<ISession>(sessions);
			_sessions.SessionStateChanged += OnSessionStateChanged;
		}

		private void OpenSession(ISession session)
		{
			_taskFactory.ThreadFactory.Invoke(() => _processShadowRenderer.Render(session));
		}

		private bool CanOpenSession(ISession session)
		{
			return session != null;
		}

		private void OnSessionStateChanged(ISession session)
		{
			if (session.IsActive && !Sessions.Contains(session))
			{
				Sessions.Add(session);
				OpenSession(session);
			}
			else if (!session.IsActive && Sessions.Contains(session))
			{
				Sessions.Remove(session);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			_sessions.SessionStateChanged -= OnSessionStateChanged;
		}
	}
}
