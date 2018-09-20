using Chronos.Extensibility;
using Chronos.Prerequisites;

namespace Chronos
{
    internal sealed class ProfilingTarget : RemoteBaseObject, IProfilingTarget, IProfilingTargetAdapter, IWrapper
    {
        private IProfilingTargetAdapter _adapter;
        private readonly IExportLoader _exportLoader;
        private readonly ProfilingTargetDefinition _definition;
        private readonly IPrerequisiteCollection _prerequisites;

        public ProfilingTarget(ProfilingTargetDefinition definition, IExportLoader exportLoader)
        {
            _definition = definition;
            _exportLoader = exportLoader;
            _prerequisites = new PrerequisiteCollection(definition.Prerequisites, exportLoader);
            Initialize();
        }

        private IProfilingTargetAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Core);
                    _adapter = _exportLoader.Load<IProfilingTargetAdapter>(exportDefinition);
                }
                return _adapter;
            }
        }

        public ProfilingTargetDefinition Definition
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
            get { return Adapter; }
        }

        public string GetAgentDll(ProcessPlatform processPlatform)
        {
            VerifyDisposed();
            ExportDefinition definition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Agent);
            string entryPoint = definition.GetEntryPoint(processPlatform, true);
            return entryPoint;
        }

        IProfilingTargetController IProfilingTargetAdapter.CreateController(ConfigurationSettings settings)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return Adapter.CreateController(settings);
            }
        }

        bool IProfilingTargetAdapter.CanStartProfiling(ConfigurationSettings settings, int processId)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return Adapter.CanStartProfiling(settings, processId);
            }
        }

        void IProfilingTargetAdapter.ProfilingStarted(ConfigurationSettings configurationSettings, SessionSettings sessionSettings, int processId)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.ProfilingStarted(configurationSettings, sessionSettings, processId);
            }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                //Do not use Adapter because it may cause initialization (but it's not needed here)
                _adapter.TryDispose();
                base.Dispose();
            }
        }

        private void Initialize()
        {
            ExportDefinition exportDefinition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Core);
            if (exportDefinition.LoadBehavior == LoadBehavior.OnStartup)
            {
                _adapter = _exportLoader.Load<IProfilingTargetAdapter>(exportDefinition);
            }
        }
    }
}
