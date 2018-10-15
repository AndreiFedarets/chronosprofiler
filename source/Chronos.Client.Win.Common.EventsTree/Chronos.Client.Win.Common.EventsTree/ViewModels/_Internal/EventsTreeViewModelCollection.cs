using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    internal sealed class EventsTreeViewModelCollection : ReadOnlyCollection<IEventsTreeViewModel>, IEventsTreeViewModelCollection
    {
        private IEventMessageBuilder _eventMessageBuilder;
        private ProfilingViewModel _profilingViewModel;
        private readonly Lazy<EventsTreeViewModel> _mainEventsTreeViewModel;

        public EventsTreeViewModelCollection()
            : base(new List<IEventsTreeViewModel>())
        {
            _mainEventsTreeViewModel = new Lazy<EventsTreeViewModel>(GetMainEventsTreeViewModel);
        }

        public void Initialize(ProfilingViewModel profilingViewModel, IEventMessageBuilder eventMessageBuilder)
        {
            _profilingViewModel = profilingViewModel;
            _eventMessageBuilder = eventMessageBuilder;
        }
        
        public IEventsTreeViewModel Open()
        {
            _profilingViewModel.Activate(_mainEventsTreeViewModel.Value);
            return _mainEventsTreeViewModel.Value;
        }

        public IEventsTreeViewModel Open(IEventTreeCollection collection)
        {
            EventsTreeViewModel viewModel = new EventsTreeViewModel(collection, _eventMessageBuilder);
            _profilingViewModel.Activate(viewModel);
            Items.Add(viewModel);
            return viewModel;
        }

        private EventsTreeViewModel GetMainEventsTreeViewModel()
        {
            IProfilingApplication application = _profilingViewModel.Application;
            IEventTreeCollection eventTrees = application.ServiceContainer.Resolve<IEventTreeCollection>();
            IEventMessageBuilder messageBuilder = application.ServiceContainer.Resolve<IEventMessageBuilder>();
            EventsTreeViewModel viewModel = new EventsTreeViewModel(eventTrees, messageBuilder);
            Items.Add(viewModel);
            return viewModel;
        }
    }
}
