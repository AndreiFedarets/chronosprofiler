using System.Collections.Generic;
using System.Windows.Input;
using Rhiannon.Presentation;
using Rhiannon.Presentation.Commands;

namespace Chronos.Client.Win.Views.Options.ProfilingEvents
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IProfilingEventsConfigurationProvider _configurationProvider;
		private readonly IViewsManager _viewsManager;
		private ProfilingEventsConfiguration _selectedConfiguration;
		private ProfilingEventPresenterCollection _profilingEvents;
		private IEnumerable<ProfilingEventsConfiguration> _configurations;

		public ViewModel(IProfilingEventsConfigurationProvider configurationProvider, IViewsManager viewsManager)
		{
			_configurationProvider = configurationProvider;
			_viewsManager = viewsManager;
			Configurations = configurationProvider.Load();
			SelectedConfiguration = configurationProvider.GetDefault();
		}

		public ICommand SetAsDefaultConfigurationCommand { get; private set; }
		public ICommand DeleteConfigurationCommand { get; private set; }
		public ICommand SaveConfigurationCommand { get; private set; }
		public ICommand CreateConfigurationCommand { get; private set; }

		public IEnumerable<ProfilingEventsConfiguration> Configurations
		{
			get { return _configurations; }
			private set { SetPropertyAndNotifyChanged(() => Configurations, ref _configurations, value); }
		}

		public ProfilingEventsConfiguration SelectedConfiguration
		{
			get { return _selectedConfiguration; }
			set
			{
				SetPropertyAndNotifyChanged(() => SelectedConfiguration, ref _selectedConfiguration, value);
				ProfilingEvents = new ProfilingEventPresenterCollection(value);
			}
		}

		public ProfilingEventPresenterCollection ProfilingEvents
		{
			get { return _profilingEvents; }
			private set { SetPropertyAndNotifyChanged(() => ProfilingEvents, ref _profilingEvents, value); }
		}

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			SetAsDefaultConfigurationCommand = new SyncCommand<ProfilingEventsConfiguration>(SetAsDefaultConfiguration, CanSetAsDefaultConfiguration);
			DeleteConfigurationCommand = new SyncCommand<ProfilingEventsConfiguration>(DeleteConfiguration, CanDeleteConfiguration);
			SaveConfigurationCommand = new SyncCommand<ProfilingEventsConfiguration>(SaveConfiguration);
			CreateConfigurationCommand = new SyncCommand<ProfilingEventsConfiguration>(e => CreateConfiguration());
		}

		private bool CanSetAsDefaultConfiguration(ProfilingEventsConfiguration configuration)
		{
			return configuration != null && !configuration.IsDefault;
		}

		private void SetAsDefaultConfiguration(ProfilingEventsConfiguration configuration)
		{
			_configurationProvider.SetAsDefault(configuration);
			Configurations = _configurationProvider.Load();
			SelectedConfiguration = _configurationProvider.GetDefault();
		}

		private bool CanDeleteConfiguration(ProfilingEventsConfiguration configuration)
		{
			return configuration != null && !configuration.IsDefault;
		}

		private void DeleteConfiguration(ProfilingEventsConfiguration configuration)
		{
			_configurationProvider.Delete(configuration);
			Configurations = _configurationProvider.Load();
			SelectedConfiguration = _configurationProvider.GetDefault();
		}

		private void SaveConfiguration(ProfilingEventsConfiguration configuration)
		{
			_configurationProvider.Save(configuration);
			ProfilingEventsConfiguration selectedConfiguration = SelectedConfiguration;
			Configurations = _configurationProvider.Load();
			SelectedConfiguration = selectedConfiguration;
		}

		private void CreateConfiguration()
		{
			IWindow window = _viewsManager.ResolveAndWrap<Rhiannon.Presentation.Views.EnterName.IView>();
			window.OpenDialog();
			string value = ((Rhiannon.Presentation.Views.EnterName.IView)window.View).Value;
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			value = value.Trim();
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			ProfilingEventsConfiguration configuration = new ProfilingEventsConfiguration(value);
			_configurationProvider.Create(configuration);
			Configurations = _configurationProvider.Load();
			SelectedConfiguration = configuration;
		}
	}
}
