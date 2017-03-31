using System;
using System.Collections.Generic;
using System.Drawing;
using Chronos.Client;
using Chronos.Core;
using Chronos.Extensibility;
using Chronos.Host;
using Rhiannon.Extensions;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService.Win
{
	public class ProfilingTarget : IProfilingTarget
	{
		private readonly IViewsManager _viewsManager;
		private readonly ConfigurationSettings _configurationSettings;
		private readonly ActivationSettings _activationSettings;
		private readonly IClientApplication _application;
		private readonly IResourceProvider _resourceProvider;
		private IViewModel _currentViewModel;

		public ProfilingTarget(IViewsManager viewsManager, IClientApplication application, IResourceProvider resourceProvider)
		{
			_configurationSettings = new ConfigurationSettings();
			_configurationSettings.UseFastHooks = true;
			_activationSettings = new ActivationSettings();
			_activationSettings.SessionActivatorCode = Constants.ActivatorCode;
			_viewsManager = viewsManager;
			_application = application;
			_resourceProvider = resourceProvider;
		}

		public string DisplayName
		{
			get { return (string)_resourceProvider[Constants.ResourcesKeys.DisplayNameKey]; }
		}

        public object Icon
        {
            get
            {
                Bitmap bitmap = (Bitmap)_resourceProvider[Constants.ResourcesKeys.IconKey];
                return bitmap.ToBitmapSource();
            }
        }

		public event Action Changed;

		public object GetView()
		{
			IViewBase view = _viewsManager.Resolve(Constants.ViewNames.Win.InternetInformationService);
			if (_currentViewModel != null)
			{
				_currentViewModel.PropertyChanged -= OnCurrentViewModelPropertyChanged;
			}
			_currentViewModel = (IViewModel) view.ViewModel;
			if (_currentViewModel != null)
			{
				_currentViewModel.PropertyChanged += OnCurrentViewModelPropertyChanged;
                _currentViewModel.SelectedApplicationPool = GetSelectedApplicationPool(_activationSettings.AppPoolName);
			}
			return view;
		}

        private IApplicationPool GetSelectedApplicationPool(string applicationPoolName)
        {
            InternetInformationService internetInformationService = new InternetInformationService();
            return internetInformationService.ApplicationPools[applicationPoolName];
        }

		private void OnCurrentViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Changed.SafeInvoke();
		}

        public void Start(string configurationName, SessionState initialState, ClrEventsMask events, bool profileSqlQueries, FilterType filterType, List<string> filters)
		{
			IApplicationPool applicationPool = _currentViewModel.SelectedApplicationPool;
			_configurationSettings.InitialState = initialState;
			_configurationSettings.TargetProcessName = Constants.HostProcessName;
            _configurationSettings.ProcessTargetArguments = string.Format("-ap \"{0}\"", applicationPool.Name);
			_configurationSettings.Events = events;
            _configurationSettings.ProfileSql = profileSqlQueries;
			_configurationSettings.FilterType = filterType;
			_configurationSettings.FilterItems = filters;
            _activationSettings.AppPoolName = applicationPool.Name;
			if (string.IsNullOrEmpty(configurationName))
			{
				configurationName = _configurationSettings.TargetProcessName;
			}
			_configurationSettings.Name = configurationName;
			IConfiguration configuration = _application.Host.Configurations.Create(_configurationSettings, _activationSettings);
			_configurationSettings.RefreshToken();
			configuration.Activate();
		}

		public bool CanStart()
		{
			return _currentViewModel != null && _currentViewModel.SelectedApplicationPool != null;
		}
	}
}
