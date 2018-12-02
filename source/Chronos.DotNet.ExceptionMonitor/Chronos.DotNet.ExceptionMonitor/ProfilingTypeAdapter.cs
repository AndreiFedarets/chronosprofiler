using System.IO;
using Chronos.Communication.Native;
using Chronos.Marshaling;
using Chronos.Storage;

namespace Chronos.DotNet.ExceptionMonitor
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, IManagedDataHandler
    {
        private readonly ExceptionCollection _exceptions;
        private IDataStorage _storage;

        static ProfilingTypeAdapter()
        {
            MarshalingManager.RegisterMarshaler(new Marshaling.ExceptionInfoMarshaler());
        }

        public ProfilingTypeAdapter()
        {
            _exceptions = new ExceptionCollection();
        }

        public void LoadData()
        {
            _exceptions.Load(_storage);
        }

        public void ReloadData()
        {
        }

        public void SaveData()
        {
            _exceptions.Save(_storage);
        }

        public void ConfigureForProfiling(ProfilingTypeSettings settings)
        {
        }

        public void StartProfiling(ProfilingTypeSettings settings)
        {
        }

        public void StopProfiling()
        {
        }

        public void AttachStorage(IDataStorage storage)
        {
            _storage = storage;
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register(_exceptions);
        }

        public void ImportServices(IServiceContainer container)
        {
        }

        public bool HandlePackage(NativeArray array)
        {
            using (Stream stream = array.OpenRead())
            {
                while (stream.Position < stream.Length)
                {
                    UnitType unitType = (UnitType) Int32Marshaler.Demarshal(stream);
                    switch (unitType)
                    {
                        case UnitType.Exception:
                            ExceptionNativeInfo[] exceptions = MarshalingManager.Demarshal<ExceptionNativeInfo[]>(stream);
                            _exceptions.Update(exceptions);
                            break;
                        default:
                            throw new TempException("Unknown UnitType");
                    }
                }
            }
            return true;
        }
    }
}
