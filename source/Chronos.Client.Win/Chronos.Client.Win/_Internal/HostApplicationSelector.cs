using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win
{
    internal sealed class HostApplicationSelector : PropertyChangedBaseEx, IHostApplicationSelector
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
                EventHandler handler = SelectionChanged;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
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
