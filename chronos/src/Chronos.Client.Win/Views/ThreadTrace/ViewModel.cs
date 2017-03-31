using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.ThreadTrace
{
    public class ViewModel : ViewModelBase, IViewModel
    {
        private IEvent _selectedEvent;
        private IThreadTraceCollection _threadTraces;

        public ViewModel(IProcessShadow processShadow, IEventNameFormatter eventNameFormatter)
        {
            ThreadTraces = processShadow.ThreadTraces;
            ThreadTraces.Reloaded += OnThreadTracesReloaded;
            EventNameFormatter = eventNameFormatter;
        }

        private void OnThreadTracesReloaded()
        {
            Invoke(() =>
            {
                IThreadTraceCollection traces = ThreadTraces;
                ThreadTraces = null;
                ThreadTraces = traces;
            });
        }

        public IEventNameFormatter EventNameFormatter { get; private set; }

        public IThreadTraceCollection ThreadTraces
        {
            get { return _threadTraces; }
            private set { SetPropertyAndNotifyChanged(() => ThreadTraces, ref _threadTraces, value); }
        }

        public IEvent SelectedEvent
        {
            get { return _selectedEvent; }
            set { SetPropertyAndNotifyChanged(() => SelectedEvent, ref _selectedEvent, value); }
        }

        public override void Dispose()
        {
            base.Dispose();
            ThreadTraces.Reloaded -= OnThreadTracesReloaded;
        }
    }
}
