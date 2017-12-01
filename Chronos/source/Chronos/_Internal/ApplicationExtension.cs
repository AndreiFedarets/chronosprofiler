using Chronos.Extensibility;

namespace Chronos
{
    internal sealed class ApplicationExtension : RemoteBaseObject, IApplicationExtension, IApplicationExtensionAdapter, IWrapper
    { 
        private IApplicationExtensionAdapter _adapter;
        private readonly IExportLoader _exportLoader;
        private readonly ApplicationExtensionDefinition _definition;

        public ApplicationExtension(ApplicationExtensionDefinition definition, IExportLoader exportLoader)
        {
            _definition = definition;
            _exportLoader = exportLoader;
        }

        private IApplicationExtensionAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(Constants.ApplicationCodeName.Core);
                    if (exportDefinition == null)
                    {
                        _adapter = new EmptyApplicationExtensionAdapter();
                    }
                    else
                    {
                        _adapter = _exportLoader.Load<IApplicationExtensionAdapter>(exportDefinition);
                    }
                }
                return _adapter;
            }
        }

        public ApplicationExtensionDefinition Definition
        {
            get
            {
                VerifyDisposed();
                return _definition;
            }
        }

        object IWrapper.UndrelyingObject
        {
            get { return Adapter; }
        }

        public void BeginInitialize(IChronosApplication application)
        {
            Adapter.BeginInitialize(application);
        }

        public void EndInitialize()
        {
            Adapter.EndInitialize();
        }

        public void BeginShutdown()
        {
            Adapter.BeginShutdown();
        }

        public void EndShutdown()
        {
            Adapter.EndShutdown();
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
    }
}
