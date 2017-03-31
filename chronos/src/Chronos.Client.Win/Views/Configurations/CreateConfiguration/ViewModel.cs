using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Chronos.Core;
using Chronos.Extensibility;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Configurations.CreateConfiguration
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private SessionState _selectedInitialState;
		private IProfilingStrategy _selectedProfilingStrategy;
		private object _profilingTargetView;
		private object _profilingStrategyView;
		private string _sessionName;

		public ViewModel(IProfilingTarget profilingTarget, 
			IClientApplication application)
		{
			SelectedProfilingTarget = profilingTarget;
			SelectedProfilingTarget.Changed += OnProfilingAdapterChanged;
			ProfilingTargetView = profilingTarget.GetView();
			//ProfilingStrategies = GetProfilingStrategies(application.ProfilingStrategies);
			ProfilingStrategies = application.ProfilingStrategies;
		}

		//private IDocumentCollection GetProfilingStrategies(IEnumerable<IProfilingStrategy> profilingStrategies)
		//{
		//    IDocumentCollection documents = new DocumentCollection();
		//    foreach (IProfilingStrategy profilingStrategy in profilingStrategies)
		//    {
		//        IViewBase view = (IViewBase)profilingStrategy.GetView();
		//        documents.Add(view, true, false, profilingStrategy);
		//    }
		//    return documents;
		//}

		public ICommand StartProfilingCommand { get; private set; }

		public string SessionName
		{
			get { return _sessionName; }
			set { SetPropertyAndNotifyChanged(() => SessionName, ref _sessionName, value); }
		}

		//public IDocumentCollection ProfilingStrategies { get; private set; }
		public IProfilingStrategyCollection ProfilingStrategies { get; private set; }

		public object ProfilingTargetView
		{
			get { return _profilingTargetView; }
			private set { SetPropertyAndNotifyChanged(() => ProfilingTargetView, ref _profilingTargetView, value); }
		}

		public object ProfilingStrategyView
		{
			get { return _profilingStrategyView; }
			private set { SetPropertyAndNotifyChanged(() => ProfilingStrategyView, ref _profilingStrategyView, value); }
		}

		public IProfilingTarget SelectedProfilingTarget { get; private set; }

		public IProfilingStrategy SelectedProfilingStrategy
		{
			get { return _selectedProfilingStrategy; }
			set
			{
				if (_selectedProfilingStrategy != null)
				{
					_selectedProfilingStrategy.Changed -= OnProfilingAdapterChanged;
				}
				_selectedProfilingStrategy = value;
				if (_selectedProfilingStrategy != null)
				{
					_selectedProfilingStrategy.Changed += OnProfilingAdapterChanged;
					ProfilingStrategyView = _selectedProfilingStrategy.GetView();
				}
				else
				{
					ProfilingStrategyView = null;
				}
				NotifyPropertyChanged(() => SelectedProfilingStrategy);
			}
		}

		//public IProfilingStrategy SelectedProfilingStrategy
		//{
		//    get
		//    {
		//        IDocument document = ProfilingStrategies.ActiveDocument;
		//        IProfilingStrategy profilingStrategy = (IProfilingStrategy)document.UnderlyingEntity;
		//        return profilingStrategy;
		//    }
		//}

		public SessionState SelectedInitialState
		{
			get { return _selectedInitialState; }
			set { SetPropertyAndNotifyChanged(() => SelectedInitialState, ref _selectedInitialState, value); }
		}

		public IEnumerable<SessionState> InitialStates { get; private set; }

		private void OnProfilingAdapterChanged()
		{
			ForceNotify();
		}

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			SelectedProfilingStrategy = ProfilingStrategies.FirstOrDefault();
			InitialStates = new[] {SessionState.Profiling, SessionState.Paused, SessionState.Closed};
			SelectedInitialState = SessionState.Profiling;
			StartProfilingCommand = new SyncCommand(StartProfiling);
		}

		private void StartProfiling()
		{
			ClrEventsMask events = SelectedProfilingStrategy.EventsMask;
		    bool profileSqlQueries = SelectedProfilingStrategy.ProfileSqlQueries;
            FilterType filterType = SelectedProfilingStrategy.FilterType;
		    List<string> filterItems = SelectedProfilingStrategy.Filters;
            SelectedProfilingTarget.Start(SessionName, SelectedInitialState, events, profileSqlQueries, filterType, filterItems);
			View.Close();
		}

		private bool CanStartProfiling()
		{
			return SelectedProfilingTarget != null && SelectedProfilingTarget.CanStart() && SelectedProfilingStrategy != null;
		}
	}
}
