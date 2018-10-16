using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    internal sealed class EventsTreeViewModelCollection : ReadOnlyCollection<IEventsTreeViewModel>, IEventsTreeViewModelCollection
    {
        private IProfilingApplication _application;
        private readonly Lazy<EventsTreeViewModel> _mainEventsTreeViewModel;

        public EventsTreeViewModelCollection()
            : base(new List<IEventsTreeViewModel>())
        {
            _mainEventsTreeViewModel = new Lazy<EventsTreeViewModel>(GetMainEventsTreeViewModel);
        }

        public void Initialize(IProfilingApplication application)
        {
            _application = application;
        }
        
        public IEventsTreeViewModel Open()
        {
            _application.MainViewModel.ActivateItem(_mainEventsTreeViewModel.Value);
            return _mainEventsTreeViewModel.Value;
        }

        public IEventsTreeViewModel Open(IEventTreeCollection collection)
        {
            IEventMessageBuilder messageBuilder = _application.ServiceContainer.Resolve<IEventMessageBuilder>();
            EventsTreeViewModel viewModel = new EventsTreeViewModel(collection, messageBuilder);
            _application.MainViewModel.ActivateItem(viewModel);
            Items.Add(viewModel);
            return viewModel;
        }

        private EventsTreeViewModel GetMainEventsTreeViewModel()
        {
            IEventTreeCollection eventTrees = _application.ServiceContainer.Resolve<IEventTreeCollection>();
            IEventMessageBuilder messageBuilder = _application.ServiceContainer.Resolve<IEventMessageBuilder>();
            EventsTreeViewModel viewModel = new EventsTreeViewModel(eventTrees, messageBuilder);
            Items.Add(viewModel);
            return viewModel;
        }
    }
}
