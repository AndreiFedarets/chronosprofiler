using Chronos.Communication.Native;
using Chronos.Extensibility;
using Chronos.Storage;

namespace Chronos.Daemon
{
    internal sealed class ProfilingTypeFacade
    {
        private readonly IProfilingType _profilingType;
        private readonly ProfilingTypeSettings _settings;

        public ProfilingTypeFacade(IProfilingType profilingType, ProfilingTypeSettings settings)
        {
            _profilingType = profilingType;
            _settings = settings;
        }

        public byte DataMarker
        {
            get { return _settings.DataMarker; }
        }

        public ProfilingTypeDefinition Definition
        {
            get { return _profilingType.Definition; }
        }

        public void AttachStorage(IDataStorage storage)
        {
            _profilingType.GetSafeAdapter().AttachStorage(storage);
        }

        public void StartProfiling()
        {
            _profilingType.GetSafeAdapter().StartProfiling(_settings);
        }

        public void StopProfiling()
        {
            _profilingType.GetSafeAdapter().StopProfiling();
        }

        public IDataHandler GetDataHandler()
        {
            return _profilingType.GetRealAdapter() as IDataHandler;
        }

        public void ImportServices(IServiceContainer container)
        {
            _profilingType.GetSafeAdapter().ImportServices(container);
        }

        public void ExportServices(IServiceContainer container)
        {
            _profilingType.GetSafeAdapter().ExportServices(container);
        }

        public void SaveData()
        {
            _profilingType.GetSafeAdapter().SaveData();
        }

        public void LoadData()
        {
            _profilingType.GetSafeAdapter().LoadData();
        }

        public void ReloadData()
        {
            _profilingType.GetSafeAdapter().ReloadData();
        }
    }
}
