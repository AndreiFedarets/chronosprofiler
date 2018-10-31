using System.Collections.Generic;
using System.Linq;
using Adenium;
using Adenium.ViewModels;
using Chronos.Client.Win.ViewModels.Common;

namespace Chronos.Client.Win.ViewModels.Start
{
    public sealed class ProfilingTargetPlaceholderViewModel : PlaceholderViewModel
    {
        private readonly IMainApplication _application;
        private readonly IHostApplicationSelector _applicationSelector;
        private readonly IProfilingTarget _profilingTarget;
        private readonly ConfigurationSettings _settings;

        public ProfilingTargetPlaceholderViewModel(IMainApplication application, IProfilingTarget profilingTarget, IHostApplicationSelector applicationSelector, ConfigurationSettings settings)
        {
            _application = application;
            _profilingTarget = profilingTarget;
            _applicationSelector = applicationSelector;
            _settings = settings;
            _applicationSelector.SelectionChanged += OnApplicationSelectorSelectionChanged;
        }

        protected override IViewModel CreateViewModel()
        {
            Host.IApplication application = _applicationSelector.SelectedApplication;
            if (application == null)
            {
                return null;
            }
            IViewModel viewModel = null;
            Chronos.IProfilingTarget profilingTarget = application.ProfilingTargets[_profilingTarget.Definition.Uid];
            List<PrerequisiteValidationResult> validationResults = profilingTarget.Prerequisites.Validate(true);
            if (validationResults.Any())
            {
                viewModel = new PrerequisitesValidationResultViewModel(validationResults);
            }
            else
            {
                IProfilingTargetAdapter adapter = _profilingTarget.GetWinAdapter();
                if (adapter != null)
                {
                    viewModel = adapter.CreateConfigurationViewModel(_application, _settings, _applicationSelector);
                }
            }
            return viewModel;
        }

        private void OnApplicationSelectorSelectionChanged(object sender, System.EventArgs e)
        {
            InvalidateViewModel();
        }

        public override void Dispose()
        {
            _applicationSelector.SelectionChanged -= OnApplicationSelectorSelectionChanged;
            base.Dispose();
        }
    }
}
