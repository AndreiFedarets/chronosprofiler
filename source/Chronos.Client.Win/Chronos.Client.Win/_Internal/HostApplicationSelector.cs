using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;

namespace Chronos.Client.Win
{
    internal sealed class HostApplicationSelector : PropertyChangedBase, IHostApplicationSelector
    {
        private readonly Host.IApplicationCollection _applications;
        private Host.IApplication _selectedApplication;

        public HostApplicationSelector(Host.IApplicationCollection applications)
        {
            _applications = applications;
            SelectedApplication = applications.FirstOrDefault();
        }

        public Host.IApplication SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value;
                NotifyOfPropertyChange(() => SelectedApplication);
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IEnumerator<Host.IApplication> GetEnumerator()
        {
            return _applications.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event EventHandler SelectionChanged;
    }
}
