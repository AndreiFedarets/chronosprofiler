using Chronos.Core;
using Rhiannon.Windows.Presentation.Documents;

namespace Chronos.Client.Win
{
    public class ProcessShadowNavigator : IProcessShadowNavigator
    {
        private IDocument _threadTraceDocument;
		private readonly IProcessShadow _processShadow;

		public ProcessShadowNavigator(IProcessShadow processShadow)
        {
			_processShadow = processShadow;
        }

        public void Initialize(IDocument threadTraceDocument)
        {
            _threadTraceDocument = threadTraceDocument;
        }

        public void Navigate(IEvent @event)
        {
            if (_threadTraceDocument == null || @event == null)
            {
                return;
            }
            Views.ThreadTrace.View view = _threadTraceDocument.View as Views.ThreadTrace.View;
            if (view == null)
            {
                return;
            }
            if (!_threadTraceDocument.IsActive)
            {
                _threadTraceDocument.IsActive = true;
            }
            view.Invoke(() => view.SelectedEvent = @event);
        }

        public void NavigateLoaded(AssemblyInfo unit)
        {
            NavigateFirst(EventType.AssemblyLoad, unit.Id);
        }

        public void NavigateFirst(EventType eventType, uint unit)
        {
            IEvent @event = FindFirst(eventType, unit);
            Navigate(@event);
        }

        private IEvent FindFirst(EventType eventType, uint unit)
        {
			foreach (IThreadTrace threadTrace in _processShadow.ThreadTraces)
            {
                IEvent @event = FindFirstEvent(threadTrace, eventType, unit);
                if (@event != null)
                {
                    return @event;
                }
            }
            return null;
        }

        private IEvent FindFirstEvent(IEvent current, EventType eventType, uint unit)
        {
            if (current.EventType == eventType)
            {
                if (current.Unit == unit)
                {
                    return current;
                }
            }
            foreach (IEvent @event in current.Children)
            {
                IEvent target = FindFirstEvent(@event, eventType, unit);
                if (target != null)
                {
                    return target;
                }
            }
            return null;
        }
    }
}
