using System.Windows.Input;
using Chronos.Core;
using Chronos.Host;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Configurations.RecentConfigurations
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IConfigurationCollection _configurations;

		public ViewModel(IClientApplication application)
		{
			_configurations = application.Host.Configurations;
		}

		public MultithreadingObservableCollection<IConfiguration> Configurations { get; private set; }

		public ICommand RestartConfigurationCommand { get; private set; }
		public ICommand StopConfigurationCommand { get; private set; }
		public ICommand DeleteConfigurationCommand { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			RestartConfigurationCommand = new SyncCommand<IConfiguration>(RestartConfiguration, CanRestartConfiguration);
			StopConfigurationCommand = new SyncCommand<IConfiguration>(StopConfiguration, CanStopConfiguration);
			DeleteConfigurationCommand = new SyncCommand<IConfiguration>(DeleteConfiguration, CanDeleteConfiguration);

			Configurations = new MultithreadingObservableCollection<IConfiguration>(_configurations);
			_configurations.ConfigurationCreated += OnConfigurationCreated;
			_configurations.ConfigurationRemoved += OnConfigurationRemoved;
		}

		private void DeleteConfiguration(IConfiguration configuration)
		{
			configuration.Remove();
		}

		private bool CanDeleteConfiguration(IConfiguration configuration)
		{
			return configuration != null;
		}

		private void RestartConfiguration(IConfiguration configuration)
		{
			configuration.Activate();
		}

		private bool CanRestartConfiguration(IConfiguration configuration)
		{
			return configuration != null;
		}

		private void StopConfiguration(IConfiguration configuration)
		{
			configuration.Deactivate();
		}

		private bool CanStopConfiguration(IConfiguration configuration)
		{
			return configuration != null && configuration.State == ConfigurationState.Active;
		}

		private void OnConfigurationCreated(IConfiguration configuration)
		{
			Configurations.Add(configuration);
		}

		private void OnConfigurationRemoved(IConfiguration configuration)
		{
			Configurations.Remove(configuration);
		}

		public override void Dispose()
		{
			base.Dispose();
			_configurations.ConfigurationCreated += OnConfigurationCreated;
			_configurations.ConfigurationRemoved += OnConfigurationRemoved;
		}

	}
}
