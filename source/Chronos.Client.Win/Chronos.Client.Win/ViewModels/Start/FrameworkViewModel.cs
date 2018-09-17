using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.ViewModels.Start
{
    public sealed class FrameworkViewModel : PropertyChangedBaseEx, Contracts.Dialog.IContractSource
    {
        private readonly IFramework _framework;
        private readonly FrameworkSettingsCollection _frameworksSettings;
        private readonly List<ProfilingTypeViewModel> _profilingTypes;
        private FrameworkSettings _frameworkSettings;
        private int _references;

        public FrameworkViewModel(IFramework framework, List<ProfilingTypeViewModel> profilingTypes, ConfigurationSettings configurationSettings)
        {
            _references = 0;
            _framework = framework;
            _frameworksSettings = configurationSettings.FrameworksSettings;
            _profilingTypes = new List<ProfilingTypeViewModel>();
            foreach (IProfilingType profilingType in _framework.ProfilingTypes)
            {
                ProfilingTypeViewModel viewModel = new ProfilingTypeViewModel(profilingType, this, profilingTypes, configurationSettings.ProfilingTypesSettings);
                viewModel.ContractSourceChanged += OnContractSourceChanged;
                profilingTypes.Add(viewModel);
                _profilingTypes.Add(viewModel);
            }
        }

        public bool Ready
        {
            get { return _profilingTypes.Any(x => x.Ready); }
        }

        public IFramework Framework
        {
            get { return _framework; }
        }

        public IEnumerable<ProfilingTypeViewModel> ProfilingTypes
        {
            get { return _profilingTypes; }
        }

        public bool IsVisible
        {
            get { return !_framework.IsHidden; }
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

        public override void Dispose()
        {
            foreach (ProfilingTypeViewModel profilingType in _profilingTypes)
            {
                profilingType.ContractSourceChanged -= OnContractSourceChanged;
            }
            base.Dispose();
        }

        internal void AddReference()
        {
            _references++;
            if (!_frameworksSettings.Contains(_framework.Definition.Uid))
            {
                if (_frameworkSettings == null)
                {
                    _frameworkSettings = _frameworksSettings.GetOrCreate(_framework.Definition.Uid);
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
                _frameworksSettings.Remove(_framework.Definition.Uid);
            }
        }

        private void OnContractSourceChanged(object sender, EventArgs e)
        {
            ContractSourceChanged.SafeInvoke(this, EventArgs.Empty);
        }
    }
}
