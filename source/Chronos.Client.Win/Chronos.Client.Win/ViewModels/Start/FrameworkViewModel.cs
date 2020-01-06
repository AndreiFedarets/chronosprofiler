using Caliburn.Micro;
using Layex.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.ViewModels.Start
{
    public sealed class FrameworkViewModel : PropertyChangedBase, IDialogContractSource, IDisposable
    {
        private readonly FrameworkSettingsCollection _frameworksSettings;
        private readonly List<ProfilingTypeViewModel> _profilingTypes;
        private FrameworkSettings _frameworkSettings;
        private int _references;

        public FrameworkViewModel(IFramework framework, List<ProfilingTypeViewModel> profilingTypes, ConfigurationSettings configurationSettings)
        {
            _references = 0;
            Framework = framework;
            _frameworksSettings = configurationSettings.FrameworksSettings;
            _profilingTypes = new List<ProfilingTypeViewModel>();
            foreach (IProfilingType profilingType in Framework.ProfilingTypes)
            {
                ProfilingTypeViewModel viewModel = new ProfilingTypeViewModel(profilingType, this, profilingTypes, configurationSettings.ProfilingTypesSettings);
                viewModel.ContractSourceChanged += OnContractSourceChanged;
                profilingTypes.Add(viewModel);
                _profilingTypes.Add(viewModel);
            }
        }

        public bool DialogReady
        {
            get { return _profilingTypes.Any(x => x.DialogReady); }
        }

        public IFramework Framework { get; private set; }

        public IEnumerable<ProfilingTypeViewModel> ProfilingTypes
        {
            get { return _profilingTypes; }
        }

        public bool IsVisible
        {
            get { return !Framework.IsHidden; }
        }

        public bool IsEnabled
        {
            get { return _references > 0; }
            set
            {
                if (value)
                {
                    AddReference();
                }
                else
                {
                    RemoveRefrence();
                }
            }
        }

        public event EventHandler ContractSourceChanged;

        public void Dispose()
        {
            foreach (ProfilingTypeViewModel profilingType in _profilingTypes)
            {
                profilingType.ContractSourceChanged -= OnContractSourceChanged;
            }
        }

        internal void AddReference()
        {
            _references++;
            if (!_frameworksSettings.Contains(Framework.Definition.Uid))
            {
                if (_frameworkSettings == null)
                {
                    _frameworkSettings = _frameworksSettings.GetOrCreate(Framework.Definition.Uid);
                }
                else
                {
                    _frameworksSettings.Add(_frameworkSettings);
                }
            }
        }

        internal void RemoveRefrence()
        {
            _references--;
            if (!IsEnabled)
            {
                _frameworksSettings.Remove(Framework.Definition.Uid);
            }
        }

        private void OnContractSourceChanged(object sender, EventArgs e)
        {
            ContractSourceChanged.SafeInvoke(this, EventArgs.Empty);
        }
    }
}
