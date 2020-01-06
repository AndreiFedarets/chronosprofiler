using Caliburn.Micro;
using Chronos.Extensibility;
using Layex.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.ViewModels.Start
{
    public sealed class ProfilingTypeViewModel : PropertyChangedBase, IDialogContractSource
    {
        private readonly ProfilingTypeSettingsCollection _profilingTypesSettings;
        private readonly List<ProfilingTypeViewModel> _profilingTypes;
        private readonly FrameworkViewModel _framework;
        //private object _settingsViewModel;
        private ProfilingTypeSettings _profilingTypeSettings;
        private int _references;
        private bool _isCheckedManually;

        public ProfilingTypeViewModel(IProfilingType profilingType, FrameworkViewModel framework,
            List<ProfilingTypeViewModel> profilingTypes, ProfilingTypeSettingsCollection profilingTypesSettings)
        {
            _references = 0;
            ProfilingType = profilingType;
            _framework = framework;
            _profilingTypes = profilingTypes;
            _profilingTypesSettings = profilingTypesSettings;
        }

        public bool DialogReady
        {
            get { return IsChecked; }
        }

        public IProfilingType ProfilingType { get; private set; }

        //public object SettingsViewModel
        //{
        //    get
        //    {
        //        if (_settingsViewModel == null && IsChecked)
        //        {
        //            _settingsViewModel = ProfilingType.GetAdapter().CreateSettingsPresentation(_profilingTypeSettings);
        //        }
        //        return _settingsViewModel;
        //    }
        //}

        public bool IsVisible
        {
            get { return !ProfilingType.Definition.IsHidden; }
        }

        public bool IsEnabled
        {
            get { return !ProfilingType.IsTechnical; }
        }

        public bool IsChecked
        {
            get { return _isCheckedManually || _references > 0; }
            set
            {
                if (_references > 0)
                {
                    return;
                }
                _isCheckedManually = value;
                if (value)
                {
                    AddReference(false);
                }
                else
                {
                    RemoveReference(false);
                }
                NotifyContractSourceChanged();
            }
        }

        private int References
        {
            get { return _references; }
            set
            {
                _references = value;
                NotifyOfPropertyChange(() => References);
                NotifyOfPropertyChange(() => IsChecked);
            }
        }

        public event EventHandler ContractSourceChanged;

        internal void AddReference(bool automaticReference)
        {
            if (automaticReference)
            {
                References++;
            }
            _framework.AddReference();
            //If settings collection doesn't contain setings block then add it
            if (!_profilingTypesSettings.Contains(ProfilingType.Definition.Uid))
            {
                if (_profilingTypeSettings == null)
                {
                    _profilingTypeSettings = _profilingTypesSettings.GetOrCreate(ProfilingType.Definition.Uid);
                    _profilingTypeSettings.FrameworkUid = ProfilingType.Definition.FrameworkUid;
                }
                else
                {
                    _profilingTypesSettings.Add(_profilingTypeSettings);
                }
            }
            //Look through profiling types and enable dependencies of current profiling type
            IEnumerable<DependencyDefinition> dependencies =ProfilingType.Definition.Dependencies;
            foreach (ProfilingTypeViewModel profilingTypeViewModel in _profilingTypes)
            {
                //profilingTypeViewModel is current profiling type - do nothing
                if (profilingTypeViewModel == this)
                {
                    continue;
                }
                ProfilingTypeViewModel viewModel = profilingTypeViewModel;
                //If profilingTypeViewModel is dependency for current profiling type?
                //If yes - enable it
                if (dependencies.Any(x => viewModel.ProfilingType.Definition.Uid == x.Uid))
                {
                    viewModel.AddReference(true);
                }
            }
        }

        internal void RemoveReference(bool automaticReference)
        {
            if (automaticReference)
            {
                References--;
            }
            _framework.RemoveRefrence();
            //If is not enabled (last reference was removed) then remove settings block from the collection
            if (!IsChecked)
            {
                _profilingTypesSettings.Remove(ProfilingType.Definition.Uid);
            }
            //Look through profiling types and enable dependencies of current profiling type
            IEnumerable<DependencyDefinition> dependencies = ProfilingType.Definition.Dependencies;
            foreach (ProfilingTypeViewModel profilingTypeViewModel in _profilingTypes)
            {
                //profilingTypeViewModel is current profiling type - do nothing
                if (profilingTypeViewModel == this)
                {
                    continue;
                }
                ProfilingTypeViewModel viewModel = profilingTypeViewModel;
                //If profilingTypeViewModel is dependency for current profiling type?
                //If yes - disable it
                if (dependencies.Any(x => viewModel.ProfilingType.Definition.Uid == x.Uid))
                {
                    viewModel.RemoveReference(true);
                }
            }
        }

        private void NotifyContractSourceChanged()
        {
            ContractSourceChanged.SafeInvoke(this, EventArgs.Empty);
        }

        //internal void SetSelection(SelectionType selectionType)
        //{
        //    _selectionType = selectionType;
        //    if (SelectionTypeToBoolConverter.Convert(_selectionType))
        //    {
        //        if (_profilingTypeSettings == null)
        //        {
        //            _profilingTypeSettings = _profilingTypeSettingsCollection.GetOrCreate(ProfilingType.Uid);
        //        }
        //        else
        //        {
        //            _profilingTypeSettingsCollection.Add(_profilingTypeSettings);
        //        }
        //    }
        //    else
        //    {
        //        _profilingTypeSettingsCollection.Remove(ProfilingType.Uid);
        //    }
        //    if (_isUpdating)
        //    {
        //        return;
        //    }
        //    _isUpdating = true;
        //    //Remove SelectionType.Auto if it is for all profiling types
        //    foreach (ProfilingTypeViewModel viewModel in _profilingTypes)
        //    {
        //        //if ((viewModel.SelectionType & SelectionType.Auto) == SelectionType.Auto)
        //        //{
        //        //    viewModel.SelectionType = viewModel.SelectionType ^ SelectionType.Auto;
        //        //}
        //    }
        //    //Update SelectionType.Auto for all profiling types
        //    foreach (ProfilingTypeViewModel viewModel in _profilingTypes)
        //    {
        //        IEnumerable<ExtensionDependency> dependencies = viewModel.ProfilingType.Extension.Dependencies;
        //        foreach (ProfilingTypeViewModel v in _profilingTypes)
        //        {
        //            if (v == viewModel)
        //            {
        //                continue;
        //            }
        //            //if (dependencies.Any(x => v.ProfilingType.Extension.Uid == x.Uid))
        //            //{
        //            //    v.SelectionType = v.SelectionType | SelectionType.Auto;
        //            //}
        //        }
        //    }
        //    _isUpdating = false;
        //}

        //public void SwitchTo(IHostApplication hostApplication)
        //{
        //    _profilingTypeSettingsViewModel.SwitchTo(hostApplication);
        //}
    }
}
