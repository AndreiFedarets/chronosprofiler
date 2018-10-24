using System.Collections.Generic;
using System.Linq;
using Adenium;
using Chronos.Client.Win.ViewModels.Common;

namespace Chronos.Client.Win.ViewModels.Start
{
    public class ProfilingTargetContent : PlaceholderContent
    {
        private readonly StartPageViewModel _pageViewModel;
        private readonly IProfilingTarget _profilingTarget;
        private readonly IHostApplicationSelector _applicationSelector;

        public ProfilingTargetContent(IProfilingTarget profilingTarget, StartPageViewModel page, IHostApplicationSelector applicationSelector)
        {
            _profilingTarget = profilingTarget;
            _pageViewModel = page;
            _applicationSelector = applicationSelector;
            _applicationSelector.SelectionChanged += OnApplicationSelectorSelectionChanged;
        }

        private void OnApplicationSelectorSelectionChanged(object sender, System.EventArgs e)
        {
            NotifyViewModelChanged();
        }

        public override string DisplayName
        {
            get { return "Application Settings"; }
        }

        public override IViewModel CreateViewModel()
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
                    viewModel = adapter.CreateConfigurationViewModel(_pageViewModel);
                }
            }
            return viewModel;
        }
    }
}
