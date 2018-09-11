using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.ViewModels.Start
{
    public class ProfilingTypesViewModel : ViewModel, Contracts.Dialog.IContractSource
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
                _frameworks.Add(viewModel);
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
            get { return "Profiling Types"; }
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
                framework.Dispose();
            }
            base.Dispose();
        }
    }
}
