using Chronos.Extensibility;
using Chronos.Prerequisites;

namespace Chronos
{
    internal sealed class Framework : RemoteBaseObject, IFramework, IFrameworkAdapter, IWrapper
    {
        private IFrameworkAdapter _adapter;
        private readonly IExportLoader _exportLoader;
        private readonly FrameworkDefinition _definition;
        private readonly IPrerequisiteCollection _prerequisites;

        public Framework(FrameworkDefinition definition, IExportLoader exportLoader)
        {
            _definition = definition;
            _exportLoader = exportLoader;
            _prerequisites = new PrerequisiteCollection(definition.Prerequisites, exportLoader);
            Initialize();
        }

        private IFrameworkAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Core);
                    _adapter = _exportLoader.Load<IFrameworkAdapter>(exportDefinition);
                }
                return _adapter;
            }
        }

        public FrameworkDefinition Definition
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

        void IFrameworkAdapter.ConfigureForProfiling(ConfigurationSettings configurationSettings)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Adapter.ConfigureForProfiling(configurationSettings);
            }
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

        private void Initialize()
        {
            ExportDefinition exportDefinition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Core);
            if (exportDefinition.LoadBehavior == LoadBehavior.OnStartup)
            {
                _adapter = _exportLoader.Load<IFrameworkAdapter>(exportDefinition);
            }
        }
    }
}
