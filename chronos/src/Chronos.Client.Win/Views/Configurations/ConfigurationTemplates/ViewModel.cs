using System.Windows.Input;
using Chronos.Extensibility;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Configurations.ConfigurationTemplates
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IViewsManager _viewsManager;

		public ViewModel(IViewsManager viewsManager, IClientApplication application)
		{
			_viewsManager = viewsManager;
			ProfilingTargets = application.ProfilingTargets;
		}

		public IProfilingTargetCollection ProfilingTargets { get; private set; }

		public ICommand CreateConfigurationCommand { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			CreateConfigurationCommand = new SyncCommand<IProfilingTarget>(CreateConfiguration, CanCreateConfiguration);
		}

		private void CreateConfiguration(IProfilingTarget profilingTarget)
		{
			IWindow window = _viewsManager.ResolveAndWrap(ViewNames.Configurations.CreateConfiguration, profilingTarget);
			window.OpenDialog();
		}

		private bool CanCreateConfiguration(IProfilingTarget profilingTarget)
		{
			return profilingTarget != null;
		}
	}
}
