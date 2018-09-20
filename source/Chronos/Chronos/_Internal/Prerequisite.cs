using Chronos.Extensibility;

namespace Chronos
{
    internal class Prerequisite : RemoteBaseObject, IPrerequisite, IPrerequisiteAdapter, IWrapper
    {
        private IPrerequisiteAdapter _adapter;
        private readonly IExportLoader _exportLoader;
        private readonly PrerequisiteDefinition _definition;

        public Prerequisite(PrerequisiteDefinition definition, IExportLoader exportLoader)
        {
            _definition = definition;
            _exportLoader = exportLoader;
        }

        public PrerequisiteDefinition Definition
        {
            get
            {
                VerifyDisposed();
                return _definition;
            }
        }

        private IPrerequisiteAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    _adapter = _exportLoader.Load<IPrerequisiteAdapter>(Definition);
                }
                return _adapter;
            }
        }

        object IWrapper.UndrelyingObject
        {
            get { return Adapter; }
        }

        public PrerequisiteValidationResult Validate()
        {
            VerifyDisposed();
            return Adapter.Validate();
        }
    }
}
