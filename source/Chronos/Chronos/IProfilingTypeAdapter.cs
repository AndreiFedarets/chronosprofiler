using Chronos.Storage;

namespace Chronos
{
    public interface IProfilingTypeAdapter : IServiceConsumer
    {
        void ConfigureForProfiling(ProfilingTypeSettings settings);

        void AttachStorage(IDataStorage storage);

        void StartProfiling(ProfilingTypeSettings settings);

        void StopProfiling();

        void LoadData();

        void SaveData();

        void ReloadData();
    }
}
