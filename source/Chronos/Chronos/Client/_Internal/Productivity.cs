using Chronos.Extensibility;

namespace Chronos.Client
{
    internal sealed class Productivity : PropertyChangedBase, IProductivity, IProductivityAdapter, IWrapper
    {
        private IProductivityAdapter _adapter;
        private readonly string _applicationCode;
        private readonly ProductivityDefinition _definition;
        private readonly IExportLoader _exportLoader;

        public Productivity(ProductivityDefinition definition, IExportLoader exportLoader, string applicationCode)
        {
            _definition = definition;
            _exportLoader = exportLoader;
            _applicationCode = applicationCode;
        }

        private IProductivityAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(_applicationCode);
                    _adapter = _exportLoader.Load<IProductivityAdapter>(exportDefinition);
                }
                return _adapter;
            }
        }

        public ProductivityDefinition Definition
        {
            get
            {
                VerifyDisposed();
                return _definition;
            }
        }

        //public bool IsAvailable
        //{
        //    get
        //    {
        //        lock (Lock)
        //        {
        //            VerifyDisposed();
        //            return _supportedHostApplications.Any();
        //        }
        //    }
        //}

        object IWrapper.UndrelyingObject
        {
            get { return Adapter; }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _adapter.TryDispose();
                base.Dispose();
            }
        }
    }
}
