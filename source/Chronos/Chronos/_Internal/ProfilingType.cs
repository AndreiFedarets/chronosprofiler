using Chronos.Extensibility;
using Chronos.Prerequisites;
using Chronos.Storage;

namespace Chronos
{
    internal sealed class ProfilingType : RemoteBaseObject, IProfilingType, IProfilingTypeAdapter, IWrapper
    {
        private IProfilingTypeAdapter _adapter;
        private readonly IExportLoader _exportLoader;
        private readonly ProfilingTypeDefinition _definition;
        private readonly IPrerequisiteCollection _prerequisites;

        public ProfilingType(ProfilingTypeDefinition definition, IExportLoader exportLoader)
        {
            _definition = definition;
            _exportLoader = exportLoader;
            _prerequisites = new PrerequisiteCollection(definition.Prerequisites, exportLoader);
            Initialize();
        }

        public ProfilingTypeDefinition Definition
        {
            get
            {
                VerifyDisposed();
                return _definition;
            }
        }

        public bool HasAgent
        {
            get
            {
                VerifyDisposed();
                return Definition.Exports.ContainsApplication(Constants.ApplicationCodeName.Agent);
            }
        }

        private IProfilingTypeAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Core);
                    _adapter = _exportLoader.Load<IProfilingTypeAdapter>(exportDefinition);
                }
                return _adapter;
            }
        }

        public IPrerequisiteCollection Prerequisites
        {
            get
            {
                VerifyDisposed();
                return _prerequisites;
            }
        }

        object IWrapper.UndrelyingObject
        {
            get
            {
                lock (Lock)
                {
                    return Adapter;
                }
            }
        }

        public string GetAgentDll(ProcessPlatform processPlatform)
        {
            VerifyDisposed();
            ExportDefinition definition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Agent);
            string entryPoint = definition.GetEntryPoint(processPlatform, true);
            return entryPoint;
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                //Do not use Agent because it may cause initialization (but it's not needed here)
                _adapter.TryDispose();
                base.Dispose();
            }
        }

        void IProfilingTypeAdapter.ConfigureForProfiling(ProfilingTypeSettings settings)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.ConfigureForProfiling(settings);
            }
        }

        void IProfilingTypeAdapter.StartProfiling(ProfilingTypeSettings settings)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.StartProfiling(settings);
            }
        }

        void IProfilingTypeAdapter.StopProfiling()
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.StopProfiling();
            }
        }

        void IProfilingTypeAdapter.LoadData()
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.LoadData();
            }
        }

        void IProfilingTypeAdapter.SaveData()
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.SaveData();
            }
        }

        void IProfilingTypeAdapter.ReloadData()
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.ReloadData();
            }
        }

        void IProfilingTypeAdapter.AttachStorage(IDataStorage storage)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.AttachStorage(storage);
            }
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.ExportServices(container);
            }
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.ImportServices(container);
            }
        }

        private void Initialize()
        {
            ExportDefinition exportDefinition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Core);
            if (exportDefinition.LoadBehavior == LoadBehavior.OnStartup)
            {
                _adapter = _exportLoader.Load<IProfilingTypeAdapter>(exportDefinition);
            }
        }
    }
}
