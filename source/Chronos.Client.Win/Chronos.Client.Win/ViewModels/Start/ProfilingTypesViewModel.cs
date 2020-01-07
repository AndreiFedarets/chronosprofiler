using Layex.Contracts;
using Layex.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.ViewModels.Start
{
    public class ProfilingTypesViewModel : ViewModel, IDialogContractSource
    {
        private readonly List<FrameworkViewModel> _frameworks;
        private object _selectedItem;
        private FrameworkViewModel _selectedFramework;
        private ProfilingTypeViewModel _selectedProfilingType;

        public ProfilingTypesViewModel(IApplicationBase application, ConfigurationSettings configurationSettings)
        {
            _frameworks = new List<FrameworkViewModel>();
            List<ProfilingTypeViewModel>  profilingTypes = new List<ProfilingTypeViewModel>();
            foreach (IFramework framework in application.Frameworks)
            {
                FrameworkViewModel viewModel = new FrameworkViewModel(framework, profilingTypes, configurationSettings);
                viewModel.ContractSourceChanged += OnContractSourceChanged;
                _frameworks.Add(viewModel);
            }
        }

        public bool DialogReady
        {
            get
            {
                //Any of frameworks is selected
                return _frameworks.Any(x => x.DialogReady);
            }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
                SelectedFramework = _selectedItem as FrameworkViewModel;
                SelectedProfilingType = _selectedItem as ProfilingTypeViewModel;
            }
        }

        public FrameworkViewModel SelectedFramework
        {
            get { return _selectedFramework; }
            set
            {
                _selectedFramework = value;
                NotifyOfPropertyChange(() => SelectedFramework);
            }
        }

        public ProfilingTypeViewModel SelectedProfilingType
        {
            get { return _selectedProfilingType; }
            set
            {
                _selectedProfilingType = value;
                NotifyOfPropertyChange(() => SelectedProfilingType);
            }
        }

        public override string DisplayName
        {
            get { return Properties.Resources.ProfilingTypesViewModel_DisplayName; }
        }

        public IEnumerable<FrameworkViewModel> Frameworks
        {
            get { return _frameworks; }
        }

        public event EventHandler ContractSourceChanged;

        public override void Dispose()
        {
            foreach (FrameworkViewModel framework in _frameworks)
            {
                framework.ContractSourceChanged -= OnContractSourceChanged;
                framework.Dispose();
            }
            base.Dispose();
        }

        private void OnContractSourceChanged(object sender, EventArgs e)
        {
            ContractSourceChanged.SafeInvoke(this, EventArgs.Empty);
        }
    }
}
