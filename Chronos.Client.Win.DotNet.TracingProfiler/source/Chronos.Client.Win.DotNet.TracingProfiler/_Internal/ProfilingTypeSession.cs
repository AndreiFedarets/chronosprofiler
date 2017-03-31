using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using Chronos.Client.Extensibility;
using Chronos.Client.Win.DotNet.BasicProfiler;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.DotNet.TracingProfiler
{
    internal class ProfilingTypeSession : IProfilingTypeSession
    {
        private readonly Dispatcher _dispatcher;
        private readonly EventTreeCollection _eventTree;
        private readonly EventFormatter _eventFormatter;
        private ICallstackInfoCollection _callstacks;
        private IClientThreadCollection _threads;
        private IClientFunctionCollection _functions;
        private DotNetPerformanceAdapter _menuAdapter;
        private readonly SafeEventSubscriber<EventArgs> _updatedEventSubscriber;

        public ProfilingTypeSession()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _eventTree = new EventTreeCollection();
            _eventFormatter = new EventFormatter();
            _updatedEventSubscriber = new SafeEventSubscriber<EventArgs>(OnUpdated);
        }

        public void IntegrateViewModel(object profilingResultsViewModel)
        {
            ProfilingResultsViewModel viewModel = (ProfilingResultsViewModel)profilingResultsViewModel;
            _menuAdapter = new DotNetPerformanceAdapter(viewModel.Session);
            viewModel.Menu.AddMenuItem(new PerformanceMenuItem()).AddMenuItem(_menuAdapter);
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register<IEventFormatter>(_eventFormatter);
            container.Register<IEventTreeCollection>(_eventTree);
        }

        public void ImportServices(IServiceContainer container)
        {
            _callstacks = container.Resolve<ICallstackInfoCollection>();
            _callstacks.Updated += _updatedEventSubscriber.OnEvent;
            _threads = container.Resolve<IClientThreadCollection>();
            _functions = container.Resolve<IClientFunctionCollection>();
            _eventFormatter.SetNameFormatter(0x0D, GetFunctionName);
            _eventFormatter.SetNameFormatter(0xFF, GetThreadName);
        }

        private string GetFunctionName(IEvent @event)
        {
            ClientFunctionInfo functionInfo = _functions[@event.Unit];
            if (functionInfo == null)
            {
                return "<UNKNOWN FUNCTION>";
            }
            return functionInfo.FullName;
        }

        private string GetThreadName(IEvent @event)
        {
            ClientThreadInfo threadInfo = _threads[@event.Unit];
            if (threadInfo == null)
            {
                return "<UNKNOWN THREAD>";
            }
            return threadInfo.Name;
        }

        private void OnUpdated(object sender, EventArgs eventArgs)
        {
            List<EventTree> trees = new List<EventTree>();
            foreach (ClientThreadInfo threadInfo in _threads)
            {
                byte[] data = _callstacks.GetUnitedCallstack(threadInfo.Uid);
                EventTree eventTree = new EventTree(threadInfo.Uid, data);
                trees.Add(eventTree);
            }
            //_dispatcher.BeginInvoke((ThreadStart) (() =>
            //{
                _eventTree.Clear();
                foreach (EventTree tree in trees)
                {
                    _eventTree.Add(tree);
                }
                if (_menuAdapter.IsViewModelInitialized)
                {
                    _dispatcher.BeginInvoke((ThreadStart) (() =>_menuAdapter.ViewModel.Reload()));
                }
            //}));
        }
    }
}
