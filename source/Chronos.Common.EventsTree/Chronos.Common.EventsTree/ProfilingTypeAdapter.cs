using Chronos.Communication.Native;
using Chronos.Storage;
using System;

namespace Chronos.Common.EventsTree
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, INativeDataHandler
    {
        private readonly EventTreeCollection _eventTrees;
        private readonly EventMessageBuilder _eventMessageBuilder;
        private readonly AgentLibrary _agentLibrary;
        private DataHandler _dataHandler;

        public ProfilingTypeAdapter()
        {
            _agentLibrary = new AgentLibrary();
            _eventTrees = new EventTreeCollection();
            _eventMessageBuilder = new EventMessageBuilder();
            _eventMessageBuilder.RegisterMessage(ThreadEventTreeMessage.EventType, new ThreadEventTreeMessage());
        }

        public void ConfigureForProfiling(Chronos.ProfilingTypeSettings settings)
        {
            ProfilingTypeSettings profilingTypeSettings = new ProfilingTypeSettings(settings);
            profilingTypeSettings.EventsBufferSize = Constants.EventsBufferSize;
            profilingTypeSettings.EventsMaxDepth = Constants.EventsMaxDepth;
        }

        public void StartProfiling(Chronos.ProfilingTypeSettings settings)
        {
            _eventTrees.ReadOnly = false;
            _dataHandler = new DataHandler(_agentLibrary, _eventTrees);
        }

        public IntPtr DataHandlerPointer
        {
            get { return _dataHandler.DataHandlerPointer; }
        }

        public void StopProfiling()
        {
            _dataHandler.Dispose();
            _dataHandler = null;
        }

        public void LoadData()
        {
            
        }

        public void SaveData()
        {
            
        }

        public void ReloadData()
        {
            _eventTrees.FlushData();
        }

        public void AttachStorage(IDataStorage storage)
        {
            _eventTrees.SetDependencies(storage);
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register(_eventTrees);
            container.Register(_eventMessageBuilder);
        }

        public void ImportServices(IServiceContainer container)
        {
        }

        public void Dispose()
        {
            _dataHandler.Dispose();
            _eventTrees.Dispose();
            _agentLibrary.Dispose();
        }
    }
}
