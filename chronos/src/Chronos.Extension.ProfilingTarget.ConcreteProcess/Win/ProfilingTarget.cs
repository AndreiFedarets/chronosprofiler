using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Chronos.Client;
using Chronos.Core;
using Chronos.Extensibility;
using Chronos.Host;
using Rhiannon.Extensions;
using Rhiannon.Resources;
using Rhiannon.Win32;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.ConcreteProcess.Win
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
			IViewBase view = _viewsManager.Resolve(Constants.ViewNames.Win.ConcreteProcess);
			if (_currentViewModel != null)
			{
				_currentViewModel.PropertyChanged -= OnCurrentViewModelPropertyChanged;
			}
			_currentViewModel = (IViewModel) view.ViewModel;
			if (_currentViewModel != null)
			{
				_currentViewModel.PropertyChanged += OnCurrentViewModelPropertyChanged;
				_currentViewModel.FileFullName = _activationSettings.ProcessFullName;
			}
			return view;
		}

		private void OnCurrentViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Changed.SafeInvoke();
		}

        public void Start(string configurationName, SessionState initialState, ClrEventsMask events, bool profileSqlQueries, FilterType filterType, List<string> filters)
		{
			string fileFullName = _currentViewModel.FileFullName;
		    string arguments = _currentViewModel.Arguments;
		    bool profileChildProcess = _currentViewModel.ProfileChildProcess;
			_configurationSettings.InitialState = initialState;
			_configurationSettings.TargetProcessName = Path.GetFileName(fileFullName);
			_configurationSettings.Events = events;
            _configurationSettings.ProfileSql = profileChildProcess;
			_configurationSettings.FilterType = filterType;
			_configurationSettings.FilterItems = filters;
		    _configurationSettings.ProfileChildProcess = profileChildProcess;
			_activationSettings.ProcessFullName = fileFullName;
			_activationSettings.ConsoleSession = NativeMethods.WTSGetActiveConsoleSessionId();
		    _activationSettings.ProcessArguments = arguments;
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
			return _currentViewModel != null && File.Exists(_currentViewModel.FileFullName);
		}
	}
}
